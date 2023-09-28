using System;
using System.IO;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    
    private AudioSource _audioSource;

    // [SerializeField] private Text CoinLb;

    // private float coin = 0f;
    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("Volume");
        // coin = PlayerPrefs.GetFloat("Coin");
        // CoinLb.text = coin.ToString();
    
    }


    public void OnValueChanged(float value)
    {
        _audioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", _audioSource.volume);
    }

    public void onExitGame()
    {
        SceneManager.LoadScene("Menu");
    }

  // public PlayerModel LoadUserData()
  //   {
  //       return new PlayerModel()
  //       {
  //          CharacterType = float.Parse( PlayerPrefs.GetString("TypeChar")),
  //          Coin =  PlayerPrefs.GetFloat("Coin"),
  //          Name = PlayerPrefs.GetString("UserName")
  //       };
  //   }
}