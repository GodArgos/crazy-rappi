using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] music;
    [SerializeField]
    private AudioClip radioInterference;

    private AudioSource src;
    private int currentIndex = 0;
    private bool isPlayingInterference = false;

    private void Start()
    {
        src = GetComponent<AudioSource>();
        src.clip = music[0];
        src.Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPlayingInterference)
        {
            StartCoroutine(PlayInterferenceThenRadio());
        }
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
}
