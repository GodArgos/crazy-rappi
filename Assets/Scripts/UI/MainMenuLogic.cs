using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuLogic : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip introClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip outroClip;

    [Header("Video Transition")]
    [SerializeField] private VideoPlayer videoPlayer;


    // Non-Serialized or private variables
    private Animator animator;
    private AudioSource audioSource;
    private bool inputReceived = false;
    private bool readyforInput = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        videoPlayer.gameObject.SetActive(false);
        audioSource.PlayOneShot(introClip);
    }

    private void Update()
    {
        if (!readyforInput && !audioSource.isPlaying)
        {
            readyforInput = true;
            if (animator != null)
            {
                animator.SetTrigger("ReadyForInput");
            }
        }
        
        if (Keyboard.current.anyKey.wasPressedThisFrame && !inputReceived && readyforInput)
        {
            inputReceived = true;
            StartCoroutine(TriggerTransition());
        }
    }

    private IEnumerator TriggerTransition()
    {
        if (animator != null)
        {
            animator.SetTrigger("InputReceived");
        }

        if (audioSource != null && coinClip != null)
        {
            audioSource.PlayOneShot(coinClip);
        }

        while (audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (animator != null)
        {
            animator.SetTrigger("FinalTransition");
        }

    }

    public void FinalCinematic()
    {
        StartCoroutine(FinalCoroutine());
    }

    private IEnumerator FinalCoroutine()
    {
        

        if (audioSource != null && outroClip != null)
        {
            audioSource.PlayOneShot(outroClip);
        }

        yield return new WaitForSeconds(1f);

        if (videoPlayer != null)
        {
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
        }

        while (videoPlayer.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
