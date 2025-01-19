using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AttackerBehavior : MonoBehaviour
{
    public Transform tiger; // Reference to the tiger's Transform
    public Transform player; // Reference to the player's Transform
    public Transform hideout;
    public float affinityRadius = 10f; // Detection range for the player
    public float stopDuration = 5f; // Time to stop when the player is spotted
    public float resumeChaseDelay = 5f; // Time to wait before chasing the tiger again
    [SerializeField] int attackPrevented = 0; // Number of times the player has prevented the attack
    [SerializeField] TextMeshProUGUI preventionCounterText;
    private NavMeshAgent navMeshAgent; // NavMeshAgent component
    private Animator animator;
    private bool isChasingTiger = true; // Whether the attacker is chasing the tiger
    private bool isStopping = false; // Whether the attacker is stopping near the player
    private bool hasEncounteredPlayer = false; // Whether the player encounter logic has already occurred

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartChasingTiger();
    }

    void Update()
    {
        preventionCounterText.text = "Attacks Prevented: " + attackPrevented;
        if (isStopping) return;

        if (isChasingTiger)
        {
            navMeshAgent.SetDestination(tiger.position);

            // Play running animation when moving
            if (navMeshAgent.velocity.magnitude > 0.1f)
                animator.SetBool("IsRunning", true);
            else
                animator.SetBool("IsRunning", false);
        }

        // Check if the player is within the affinity radius and hasn't been encountered before
        if (!hasEncounteredPlayer && Vector3.Distance(transform.position, player.position) < affinityRadius)
        {
            StartCoroutine(HandlePlayerEncounter());
            attackPrevented++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttackTiger"))
        {
            Debug.Log("Tiger hit!");
            navMeshAgent.isStopped = true;
            StartCoroutine(PunchTiger());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("AttackTiger"))
        {
            navMeshAgent.isStopped = false;
        }
    }

    IEnumerator PunchTiger()
    {
        animator.SetBool("IsRunning", false);
        navMeshAgent.isStopped = true; // Stop the NavMeshAgent

        animator.SetBool("attack", true); // Start punching animation

        while (!IsPlayerInRange())
        {
            // Continue punching as long as the player is not in range
            yield return new WaitForSeconds(1f); // Repeat every second
        }

        animator.SetBool("attack", false); // Stop punching animation
        navMeshAgent.isStopped = false; // Resume NavMeshAgent
    }

    IEnumerator HandlePlayerEncounter()
    {
        hasEncounteredPlayer = true; // Mark player as encountered
        isStopping = true;
        isChasingTiger = false;

        // Stop and play idle animation
        animator.SetBool("IsRunning", false);
        animator.SetBool("attack", false);

        // Wait near the player for the specified duration
        yield return new WaitForSeconds(stopDuration);

        // Start running to the hideout
        navMeshAgent.isStopped = false; // Resume NavMeshAgent
        animator.SetBool("IsRunning", true);
        navMeshAgent.SetDestination(hideout.position);
        Debug.Log("Running to hideout");

        // Allow further updates and state changes
        isStopping = false;
    }

    void StartChasingTiger()
    {
        isChasingTiger = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(tiger.position);
    }

    void StartRunningAway()
    {
        isChasingTiger = false;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(hideout.position);
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < affinityRadius;
    }
}
