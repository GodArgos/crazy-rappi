using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressSFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;

    public void ButtonSound()
    {
        audioSource.PlayOneShot(clip);
    }
}
