using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class JumpScareManager : MonoBehaviour
{
  public AudioClip jumpscareSound;
  public GameObject jumpscareVisual; // e.g., full-screen image or animation
  public float delayBeforeGameOver = 2f;

  private AudioSource audioSource;
  private bool hasTriggered = false;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    if (jumpscareVisual != null)
      jumpscareVisual.SetActive(false);
  }

  public void TriggerJumpScare()
  {
    if (hasTriggered) return;
    hasTriggered = true;

    // Stop time or disable movement here
    Time.timeScale = 0f;

    if (jumpscareVisual != null)
      jumpscareVisual.SetActive(true);

    if (audioSource != null && jumpscareSound != null)
      audioSource.PlayOneShot(jumpscareSound);

    // Wait, then load Game Over
    StartCoroutine(GameOverAfterDelay());
  }

  private IEnumerator GameOverAfterDelay()
  {
    // Wait using real time since timeScale = 0
    yield return new WaitForSecondsRealtime(delayBeforeGameOver);
    // Load game over scene or show UI
    SceneManager.LoadScene("GameOverScene"); // Replace with your scene or logic
  }
}
