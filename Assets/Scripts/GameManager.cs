﻿using System;
using System.IO;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    
    private AudioSource _audioSource;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject PauseBtn;
    [SerializeField] private GameObject Map;
    [SerializeField] private Text CoinLb;

    private float coin = 0f;
    private void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.volume = PlayerPrefs.GetFloat("Volume");
        coin = PlayerPrefs.GetFloat("Coin");
        CoinLb.text = coin.ToString();
        Map.SetActive(true);
    }


    public void CloseMap()
    {
        Map.SetActive(false);
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

    public void onPauseGame()
    {
        PausePanel.SetActive(true);
        PauseBtn.SetActive(false);
        Time.timeScale = 0;
    }

    public void onContinueGame()
    {
        PausePanel.SetActive(false);
        PauseBtn.SetActive(true);
        Time.timeScale = 1f;
    }
    public PlayerModel LoadUserData()
    {
        return new PlayerModel()
        {
           CharacterType = float.Parse( PlayerPrefs.GetString("TypeChar")),
           Coin =  PlayerPrefs.GetFloat("Coin"),
           Name = PlayerPrefs.GetString("UserName")
        };
    }
}