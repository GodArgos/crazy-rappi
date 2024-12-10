using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TransitionCanvasLogic : MonoBehaviour
{
    [SerializeField] private ReadyMenuManager readyMenu;

    public void EndCinematic()
    {
        readyMenu.EndTransition();
    }
}
