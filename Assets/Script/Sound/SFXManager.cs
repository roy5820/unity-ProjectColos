using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    public AudioClip playerDash;
    public AudioClip playerColorChange;

    public AudioClip playerNomalAttack;
    public AudioClip playerGreenSkill;
    public AudioClip playerRedSkill;
    public AudioClip playerBlueSkill;
    
    AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
