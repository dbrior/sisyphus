using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource scoreBlipSound;
    public AudioSource backgroundMusic;
    public AudioSource bossMusic;

    public void RampInto(AudioSource rampIn, AudioSource rampOut, float targetVolumeIn, float targetVolumeOut, float rampDuration)
    {
        StartCoroutine(RampVolume(rampIn, rampOut, targetVolumeIn, targetVolumeOut, rampDuration));
    }

    IEnumerator RampVolume(AudioSource audioSourceIn, AudioSource audioSourceOut, float targetVolumeIn, float targetVolumeOut, float rampDuration)
    {
        float currentTime = 0;
        audioSourceIn.volume = 0;
        float startVolumeIn = audioSourceIn.volume;
        float startVolumeOut = audioSourceOut.volume;

        while (currentTime < rampDuration)
        {
            currentTime += Time.deltaTime;
            audioSourceIn.volume = Mathf.Lerp(startVolumeIn, targetVolumeIn, currentTime / rampDuration);
            audioSourceOut.volume = Mathf.Lerp(startVolumeOut, 0, currentTime / rampDuration);
            yield return null; // Wait for the next frame
        }

        audioSourceIn.volume = targetVolumeIn; // Ensure the volume is set to the target volume at the end for fade-in
        audioSourceOut.volume = 0; // Ensure the volume is set to 0 at the end for fade-out
    }
}
