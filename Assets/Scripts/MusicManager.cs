using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{


    public static MusicManager instance;

    // Vars for audio
    public AudioSource audioSource;
    public List<AudioClip> musics;
    public float musicScale;
    public AudioClip killBird, pickedBonus, menu, timeOver;

    void Awake()
    {
        MakeSingleton();

        musicScale = PlayerPrefs.GetFloat("musicScale", 1f);

        audioSource = GetComponent<AudioSource>();
        if (audioSource.isPlaying == false)
        {
            StartCoroutine(ChangeSong());
        }
    }

    IEnumerator ChangeSong()
    {
        int randomIndex = Random.Range(0, musics.Count);

        audioSource.Stop();
        audioSource.PlayOneShot(musics[randomIndex], musicScale);

        yield return new WaitForSecondsRealtime(musics[randomIndex].length);

        StartCoroutine(ChangeSong());
    }

    public void ChangeMusic()
    {
        StopCoroutine(ChangeSong());
        audioSource.Stop();
        StartCoroutine(ChangeSong());
    }

    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}