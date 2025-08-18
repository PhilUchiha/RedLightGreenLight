using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightController : MonoBehaviour
{
  public List<Light> controlledLights;       // All lights that should sync red/green
  public AudioClip greenLightSound;
  public AudioClip redLightSound;

  private AudioSource audioSource;

  public float minGreenDuration = 3f;
  public float maxGreenDuration = 6f;
  public float minRedDuration = 2f;
  public float maxRedDuration = 4f;
  public bool isGreen = true;
  private float timer;

  public float volume = 0.1f;
  public float pitch = 1f;

  private void Start()
  {
    // Auto-collect if list is empty and you want to use tags (optional)
    if (controlledLights == null || controlledLights.Count == 0)
    {
      GameObject[] taggedLights = GameObject.FindGameObjectsWithTag("SyncLight");
      controlledLights = new List<Light>();
      foreach (GameObject obj in taggedLights)
      {
        Light light = obj.GetComponent<Light>();
        if (light != null) controlledLights.Add(light);
      }
    }

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
      audioSource = gameObject.AddComponent<AudioSource>();
    }

    audioSource.volume = volume;
    audioSource.pitch = pitch;

    SetRandomLightDuration(true);
    SetLightsColor(Color.green);
    PlaySound(greenLightSound);
  }

  private void Update()
  {
    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
      if (isGreen)
      {
        SetLightsColor(Color.red);
        PlaySound(redLightSound);
        SetRandomLightDuration(false);
      }
      else
      {
        SetLightsColor(Color.green);
        PlaySound(greenLightSound);
        SetRandomLightDuration(true);
      }

      isGreen = !isGreen;
    }
  }

  private void SetLightsColor(Color color)
  {
    foreach (Light light in controlledLights)
    {
      if (light != null)
        light.color = color;
    }
  }

  private void SetRandomLightDuration(bool isGreenLight)
  {
    timer = isGreenLight
        ? Random.Range(minGreenDuration, maxGreenDuration)
        : Random.Range(minRedDuration, maxRedDuration);
  }

  private void PlaySound(AudioClip clip)
  {
    if (clip != null && audioSource != null)
    {
      audioSource.volume = volume;
      audioSource.pitch = pitch;
      audioSource.PlayOneShot(clip);
    }
  }
}
