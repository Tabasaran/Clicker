
using UnityEngine;
using UnityEngine.UI;

public class MusicScaleController : MonoBehaviour
{
    private Slider musicScaleSlader;

    void Start()
    {
        musicScaleSlader = GetComponent<Slider>();
        musicScaleSlader.value = PlayerPrefs.GetFloat("musicScale", 1f);
    }
    public void ChangeMusicScale()
    {
        MusicManager.instance.musicScale = musicScaleSlader.value;
        MusicManager.instance.audioSource.volume = musicScaleSlader.value;
        PlayerPrefs.SetFloat("musicScale", musicScaleSlader.value);
    }

}
