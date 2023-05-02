using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] private AudioClip coinCollectSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCoinCollectSound()
    {
        PlaySound(coinCollectSound);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
