using System;
using System.IO;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    
    private AudioSource _audioSource;
   [SerializeField] private Slider volumeSlider;

    // [SerializeField] private Text CoinLb;

    // private float coin = 0f;
    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("Volume");
        volumeSlider.value = _audioSource.volume;
        volumeSlider.onValueChanged.AddListener(OnChangeVolumn);
    }


    public void OnChangeVolumn(float value)
    {
        
        _audioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", _audioSource.volume);
    }

    public void onExitGame()
    {
        SceneManager.LoadScene("Menu");
    }


}