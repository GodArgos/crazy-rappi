using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TransitionCanvasLogic : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.gameObject.SetActive(false);
    }

    public void ActivateStartTrasition()
    {
        StartCoroutine(ActivationCoroutine());
    }

    private IEnumerator ActivationCoroutine()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        videoPlayer.gameObject.SetActive(false);
    }
}
