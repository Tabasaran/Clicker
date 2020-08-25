using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{


    public static MusicManager instance;

    // Vars for audio
    private AudioSource audioSource;
    public List<AudioClip> sounds;
    private float musicScale;

    void Awake()
    {
        musicScale = 0.5f;
        MakeSingleton();

        audioSource = GetComponent<AudioSource>();



        // Setting the music state standard to 1 so music starts playing on game load
        if (!PlayerPrefs.HasKey("Game Initialized"))
        {
            PlayerPrefs.SetInt(" Game Initialized", 123);
        }

        StartCoroutine(ChangeSong());

    }

    IEnumerator ChangeSong()
    {
        int randomIndex = Random.Range(0, sounds.Count);
        audioSource.PlayOneShot(sounds[randomIndex], musicScale);
        yield return new WaitForSecondsRealtime(sounds[randomIndex].length);
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