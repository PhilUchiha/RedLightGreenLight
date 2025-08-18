using UnityEngine;

public class MonsterAI : MonoBehaviour
{
  public LightController lightController;
  public GameObject player;
  public UnityEngine.AI.NavMeshAgent navMeshAgent;
  private Animator animator;

  public MonsterFootsteps footsteps;

  private bool wasGreenLastFrame = true;

  private float footstepTimer = 0f;
  public float footstepInterval = 0.5f;

  private void Start()
  {
    if (navMeshAgent == null)
      navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

    animator = GetComponentInChildren<Animator>();

    if (footsteps == null)
      footsteps = GetComponent<MonsterFootsteps>();
  }

  private void Update()
  {
    if (lightController == null || navMeshAgent == null || player == null)
      return;

    bool isWalking = !lightController.isGreen && !navMeshAgent.isStopped;

    if (animator != null)
      animator.SetBool("isWalking", isWalking);

    if (lightController.isGreen != wasGreenLastFrame)
    {
      navMeshAgent.isStopped = lightController.isGreen;

      if (!lightController.isGreen)
        navMeshAgent.SetDestination(player.transform.position);
    }

    if (isWalking)
    {
      navMeshAgent.SetDestination(player.transform.position);

      footstepTimer -= Time.deltaTime;
      if (footstepTimer <= 0f && footsteps != null)
      {
        footsteps.PlayFootstep();
        footstepTimer = footstepInterval;
      }
    }
    else
    {
      footstepTimer = 0f;
    }

    wasGreenLastFrame = lightController.isGreen;
  }
}
