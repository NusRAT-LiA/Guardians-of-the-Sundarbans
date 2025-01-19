using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class AttackerBehavior : MonoBehaviour
{
    public Transform tiger; // Reference to the tiger's Transform
    public Transform player; // Reference to the player's Transform
    public Transform hideout; // Reference to the hideout's Transform
    public float affinityRadius = 10f; // Detection range for the player
    public float stopDuration = 5f; // Time to stop when the player is spotted
    public float hideoutRadius = 10f; // Radius around hideout to reset the attacker's state
    public TextMeshProUGUI preventionCounterText; // Reference to the shared UI element for the total count
    public float minStartDelay = 2f; // Minimum random start delay
    public float maxStartDelay = 100f; // Maximum random start delay

    private NavMeshAgent navMeshAgent; // NavMeshAgent component
    private Animator animator;
    private bool isChasingTiger = true; // Whether the attacker is chasing the tiger
    private bool isStopping = false; // Whether the attacker is stopping near the player
    private bool hasEncounteredPlayer = false; // Whether the player encounter logic has already occurred

    // Static variable to keep track of the total count
    private static int totalAttackPrevented = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(StartAfterRandomDelay());
    }

    void Update()
    {
        // Update the total prevention count UI
        if (preventionCounterText != null)
        {
            preventionCounterText.text = "Total Attacks Prevented: " + totalAttackPrevented;
        }

        if (isStopping) return;

        if (isChasingTiger)
        {
            navMeshAgent.SetDestination(tiger.position);

            if (navMeshAgent.velocity.magnitude > 0.1f)
                animator.SetBool("IsRunning", true);
            else
                animator.SetBool("IsRunning", false);
        }

        if (!hasEncounteredPlayer && Vector3.Distance(transform.position, player.position) < affinityRadius)
        {
            StartCoroutine(HandlePlayerEncounter());
            totalAttackPrevented++; 
        }

        if (!isChasingTiger && Vector3.Distance(transform.position, hideout.position) < hideoutRadius)
        {
            ResetToChaseTiger();
        }
    }

    IEnumerator HandlePlayerEncounter()
    {
        hasEncounteredPlayer = true; 
        isStopping = true;
        isChasingTiger = false;
        navMeshAgent.isStopped = true;

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

    void ResetToChaseTiger()
    {
        Debug.Log("Reached hideout. Resetting to chase the tiger.");
        hasEncounteredPlayer = false;
        StartChasingTiger(); 
    }

    IEnumerator StartAfterRandomDelay()
    {
        float randomDelay = Random.Range(minStartDelay, maxStartDelay);
        yield return new WaitForSeconds(randomDelay);

        StartChasingTiger();
    }
}
