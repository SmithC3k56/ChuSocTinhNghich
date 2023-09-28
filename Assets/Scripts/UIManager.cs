using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject btns;
    [SerializeField] private GameObject closeBtn;
    [SerializeField] private GameObject characterLayout;
    [SerializeField] private GameObject VolumeSetting;
    [SerializeField] private Sprite[] characters;
    [SerializeField] private Image character;
    [SerializeField] private Text sellLb;
    [SerializeField] private Text nameLb;
    [SerializeField] private Text GemLb;
    [SerializeField] private GameObject ErrorTx;


    private float _currentCount = 0;

    private string characterHas;

    // Start is called before the first frame update
    void Start()
    {
        var newUser = new PlayerModel()
        {
            Name = "Smith",
            Coin = 105,
            CharacterType = 0.5f,
            characterHas = "0"
        };
        SaveUserData(newUser);
        characterHas = PlayerPrefs.GetString("characterHas");
        onShowCharacter();
        onShowName();
        SetGem();
    }

    private void SetGem()
    {
        GemLb.text = PlayerPrefs.GetFloat("Coin").ToString();
    }

    public void SaveUserData(PlayerModel userData)
    {
        // string json = JsonUtility.ToJson(userData);
        // File.WriteAllText("userData.json", json);
        PlayerPrefs.SetString("UserName", userData.Name);
        PlayerPrefs.SetFloat("Coin", userData.Coin);
        // PlayerPrefs.SetString("TypeChar", userData.CharacterType.ToString());
        PlayerPrefs.SetString("characterHas", userData.characterHas);
    }

    public void ChooseOrBuy()
    {
        if (this.characterHas.Contains(this._currentCount.ToString()))
        {
            float typeChar = (_currentCount / 2);
            PlayerPrefs.SetString("TypeChar", typeChar.ToString());


            this.characterLayout.SetActive(false);
            this.btns.SetActive(true);
        }
        else
        {
            var coin = PlayerPrefs.GetFloat("Coin");
            switch (_currentCount)
            {
                case 1:
                    if (PlayerPrefs.GetFloat("Coin") < 100)
                    {
                        ErrorTx.SetActive(true);
                        StartCoroutine(DestroyTextAfterDelay(2f));
                    }
                    else
                    {
                        coin -= 100;
                        this.sellLb.text = "Choose";
                        characterHas = characterHas + "," + _currentCount;
                        PlayerPrefs.SetFloat("Coin", coin);
                        PlayerPrefs.SetString("characterHas", characterHas);
                        SetGem();
                    }

                    break;
                case 2:
                    if (PlayerPrefs.GetFloat("Coin") < 200)
                    {
                        ErrorTx.SetActive(true);
                        StartCoroutine(DestroyTextAfterDelay(2f));
                    }
                    else
                    {
                        coin -= 200;
                        this.sellLb.text = "Choose";
                        characterHas = characterHas + "," + _currentCount;
                        PlayerPrefs.SetFloat("Coin", coin);
                        PlayerPrefs.SetString("characterHas", characterHas);
                        SetGem();
                    }

                    break;
            }
        }
    }

    private IEnumerator DestroyTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ErrorTx.SetActive(false);
    }

    public void onShowCharacter()
    {
        character.sprite = this.characters[(int)_currentCount];
    }

    public void onShowName()
    {
        switch (this._currentCount)
        {
            case 0:
                this.sellLb.text = "Choose";
                this.nameLb.text = "Soc";
                break;
            case 1:
                this.nameLb.text = "Thoc";
                if (this.characterHas.Contains("1"))
                {
                    this.sellLb.text = "Choose";
                }
                else
                {
                    this.sellLb.text = "100";
                }

                break;
            case 2:
                this.nameLb.text = "Tho";
                if (this.characterHas.Contains("2"))
                {
                    this.sellLb.text = "Choose";
                }
                else
                {
                    this.sellLb.text = "200";
                }

                break;
            default:
                this.nameLb.text = "Smith";
                break;
        }
    }

    public void onPreCharacter()
    {
        if (_currentCount <= 0)
        {
            _currentCount = characters.Length - 1;
        }
        else
        {
            _currentCount--;
        }

        onShowCharacter();
        onShowName();
    }

    public void onNextCharacter()
    {
        if (_currentCount >= characters.Length - 1)
        {
            _currentCount = 0;
        }
        else
        {
            _currentCount++;
        }

        onShowCharacter();
        onShowName();
    }

    public void onPlayBtn()
    {
        SceneManager.LoadScene("Lv1");
    }

    public void onClose()
    {
        this.btns.SetActive(true);
        this.VolumeSetting.SetActive(false);
        this.characterLayout.SetActive(false);
        this.closeBtn.SetActive(false);
    }

    public void onChooseChacterBtn()
    {
        this.closeBtn.SetActive(true);
        this.btns.SetActive(false);
        this.characterLayout.SetActive(true);
    }

    public void onVolumnBtn()
    {
        this.closeBtn.SetActive(true);
        this.btns.SetActive(false);
        VolumeSetting.SetActive(true);
    }
}