using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AttackerBehavior : MonoBehaviour
{
    public Transform tiger; // Reference to the tiger's Transform
    public Transform player; // Reference to the player's Transform
    public float affinityRadius = 10f; // Detection range for the player
    public float stopDuration = 10f; // Time to stop when the player is spotted
    public float resumeChaseDelay = 30f; // Time to wait before chasing the tiger again

    private NavMeshAgent navMeshAgent; // NavMeshAgent component
    private Animator animator;
    private bool isChasingTiger = true; // Whether the attacker is chasing the tiger
    private bool isStopping = false; // Whether the attacker is stopping near the player

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartChasingTiger();
    }

    void Update()
    {
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

        // Check if the player is within the affinity radius
        if (!isChasingTiger && Vector3.Distance(transform.position, player.position) < affinityRadius)
        {
            StartCoroutine(HandlePlayerEncounter());
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
        isStopping = true;
        isChasingTiger = false;

        // Stop and play idle animation
        navMeshAgent.isStopped = true;
        animator.SetBool("IsRunning", false);

        // Wait near the player for the specified duration
        yield return new WaitForSeconds(stopDuration);

        // Run away from the player
        Vector3 runAwayDirection = (transform.position - player.position).normalized;
        Vector3 runAwayTarget = transform.position + runAwayDirection * 10f;
        navMeshAgent.SetDestination(runAwayTarget);

        yield return new WaitForSeconds(resumeChaseDelay);

        // Resume chasing the tiger
        StartChasingTiger();
        isStopping = false;
    }

    void StartChasingTiger()
    {
        isChasingTiger = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(tiger.position);
    }

    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) < affinityRadius;
    }
}
