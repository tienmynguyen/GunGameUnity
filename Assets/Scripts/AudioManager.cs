using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioSource defaulAudioSource;
    [SerializeField] private AudioSource bossAudioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip reLoadClip;
    [SerializeField] private AudioClip energyClip;

    public void PlayShootSound()
    {
        effectAudioSource.PlayOneShot(shootClip);
    }
    public void PlayReLoadSound()
    {
        effectAudioSource.PlayOneShot(reLoadClip);
    }
    public void PlayEnergySound()
    {
        effectAudioSource.PlayOneShot(energyClip);
    }
    public void PlayDefaulAudio()
    {
        bossAudioSource.Stop();
        defaulAudioSource.Play();
    }
    public void PlayBossAudio()
    {
        defaulAudioSource.Stop();
        bossAudioSource.Play();
    }
    public void StopAudio()
    {
        effectAudioSource.Stop();
        defaulAudioSource.Stop();
        bossAudioSource.Stop();
    }
}
