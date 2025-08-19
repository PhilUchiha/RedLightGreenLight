using UnityEngine;


public class MonsterCatchTrigger : MonoBehaviour
{
  public JumpScareManager jumpScareManager;

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      jumpScareManager.TriggerJumpScare();
    }
  }
}
