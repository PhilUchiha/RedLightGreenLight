using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
  public LightController lightController;
  public GameObject player;
  public NavMeshAgent navMeshAgent;
  private Animator animator;
  public MonsterFootsteps footsteps;

  [Header("Patrol Settings")]
  public Transform[] patrolPoints;
  private int currentPatrolIndex = 0;

  [Header("Detection Settings")]
  public float viewDistance = 15f;
  public float viewAngle = 60f;
  public LayerMask obstructionMask;

  private bool playerInSight = false;

  private float footstepTimer = 0f;
  public float footstepInterval = 0.5f;

  private enum State { Patrolling, Chasing }
  private State currentState = State.Patrolling;

  private void Start()
  {
    if (navMeshAgent == null)
      navMeshAgent = GetComponent<NavMeshAgent>();

    animator = GetComponentInChildren<Animator>();
    if (footsteps == null)
      footsteps = GetComponent<MonsterFootsteps>();

    if (patrolPoints.Length > 0)
      navMeshAgent.SetDestination(patrolPoints[0].position);
  }

  private void Update()
  {
    if (lightController == null || navMeshAgent == null || player == null) return;

    DetectPlayer();

    if (lightController.isGreen)
    {
      // Freeze movement
      navMeshAgent.isStopped = true;

      // Freeze animation on last frame (but only if it has started moving)
      if (animator != null && animator.GetCurrentAnimatorStateInfo(0).length > 0)
        animator.speed = 0f;

      return;
    }
    else
    {
      navMeshAgent.isStopped = false;
      if (animator != null) animator.speed = 1f;
    }

    switch (currentState)
    {
      case State.Patrolling:
        Patrol();
        if (playerInSight) currentState = State.Chasing;
        break;

      case State.Chasing:
        ChasePlayer();
        if (!playerInSight) currentState = State.Patrolling;
        break;
    }

    HandleFootsteps();
  }

  private void Patrol()
  {
    if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f && patrolPoints.Length > 0)
    {
      currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
      navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    animator?.SetBool("isWalking", true);
  }

  private void ChasePlayer()
  {
    navMeshAgent.SetDestination(player.transform.position);
    animator?.SetBool("isWalking", true);
  }

  private void DetectPlayer()
  {
    playerInSight = false;
    Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;

    if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
    {
      if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2f)
      {
        if (!Physics.Linecast(transform.position, player.transform.position, obstructionMask))
        {
          playerInSight = true;
        }
      }
    }
  }

  private void HandleFootsteps()
  {
    if (!navMeshAgent.isStopped && navMeshAgent.velocity.magnitude > 0.1f && footsteps != null)
    {
      footstepTimer -= Time.deltaTime;
      if (footstepTimer <= 0f)
      {
        footsteps.PlayFootstep();
        footstepTimer = footstepInterval;
      }
    }
    else
    {
      footstepTimer = 0f;
    }
  }
}
