using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelControlManager : MonoBehaviour
{
    [SerializeField] private List<CinemachineVirtualCamera> levelCameras;
    [SerializeField] private Animator transitionAnimator;
    [SerializeField] private AudioClip transitionClip;
    private AudioSource audioSource;
    private Dictionary<int, CinemachineVirtualCamera> indexedCameras;
    private int currentCamera = 0;
    private int maxPriority = 100;
    private int minPriority = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ArrangeCameras();
    }

    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            ChangeCameraLevel();
        }
    }

    private void ArrangeCameras()
    {
        indexedCameras = new Dictionary<int, CinemachineVirtualCamera>();

        for (int i = 0; i < levelCameras.Count;i++)
        {
            indexedCameras[i] = levelCameras[i];
        }

        ActivateNCamera(currentCamera);
    }

    private void ActivateNCamera(int index)
    {
        foreach (var cam in indexedCameras)
        {
            if (cam.Key == index)
            {
                cam.Value.Priority = maxPriority;
            }
            else
            {
                cam.Value.Priority = minPriority;
            }
        }
    }

    private void ChangeCameraLevel()
    {
        currentCamera++;
        if (currentCamera >= levelCameras.Count)
        {
            currentCamera = 0;
        }

        transitionAnimator.SetTrigger("LevelChanged");
        audioSource.PlayOneShot(transitionClip);
        ActivateNCamera(currentCamera);
    }

}
