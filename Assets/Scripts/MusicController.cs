using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] music;
    [SerializeField]
    private AudioClip radioInterference;

    private AudioSource src;
    private int currentIndex = 0;
    private bool isPlayingInterference = false;

    private DefaultInputMap playerInput;

    private void Awake()
    {
        playerInput = new DefaultInputMap();
    }

    private void Start()
    {
        ActionEventsSubscription();
        src = GetComponent<AudioSource>();
        src.clip = music[0];
        src.Play();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Tab) && !isPlayingInterference)
    //    {
    //        StartCoroutine(PlayInterferenceThenRadio());
    //    }
    //}

    public void ResumeAll()
    {
        src.Play();
    }

    public void PauseAll()
    {
        src.Pause();
    }

    public void StopAll()
    {
        src.Stop();
        StopAllCoroutines();
        this.enabled = false;
    }

    private void ChangeRadioChannel(InputAction.CallbackContext context)
    {
        StartCoroutine(PlayInterferenceThenRadio());
    }

    private IEnumerator PlayInterferenceThenRadio()
    {
        isPlayingInterference = true;

        src.clip = radioInterference;
        src.Play();

        yield return new WaitForSeconds(radioInterference.length);

        currentIndex++;
        if (currentIndex == music.Length)
        {
            currentIndex = 0;
        }
        src.clip = music[currentIndex];
        src.Play();

        isPlayingInterference = false;
    }

    #region Input System Methods
    private void ActionEventsSubscription()
    {
        ControllsSubscription();
    }

    private void ActionEventsUnSubscription()
    {
        ControllsUnSubscription();
    }

    private void OnEnable()
    {
        playerInput.InGame.Enable();
    }

    private void OnDisable()
    {
        playerInput.InGame.Disable();
    }

    #region Action Events
    private void ControllsSubscription()
    {
        playerInput.InGame.ChangeRadio.performed += ChangeRadioChannel;
    }

    private void ControllsUnSubscription()
    {
        playerInput.InGame.ChangeRadio.performed -= ChangeRadioChannel;
    }
    #endregion
    #endregion
}
