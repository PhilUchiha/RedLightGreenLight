using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFootsteps : MonoBehaviour
{
  public AudioClip[] footstepSounds;
  public AudioSource audioSource;

  public float minPitch = 0.9f;
  public float maxPitch = 1.1f;

  private void Start()
  {
    if (audioSource == null)
      audioSource = GetComponent<AudioSource>();
  }

  public void PlayFootstep()
  {
    if (footstepSounds.Length == 0 || audioSource == null) return;

    AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
    audioSource.pitch = Random.Range(minPitch, maxPitch);
    audioSource.PlayOneShot(clip);
  }
}