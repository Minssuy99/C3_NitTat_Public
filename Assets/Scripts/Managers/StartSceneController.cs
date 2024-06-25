using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    [SerializeField] private Text subTitle;
    [SerializeField] string[] subTitleMessages;

    [SerializeField] GameObject[] OptionUIs;
    [SerializeField] Slider _musicSlider, _sfxSlider;

    void Start()
    {
        UIManager.Instance.PrintRandomText(subTitle, subTitleMessages);
    }

    public void OptionUIToggle()
    {
        UIManager.Instance.OptionUIInit(OptionUIs);
        AudioManager.Instance.PlaySFX(SoundName.Button);
        UIManager.Instance.OptionUI(OptionUIs[0]);
    }

    public void GameExit()
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        GameSceneManager.Instance.GameExit();
    }

    public void GameStart()
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        GameSceneManager.Instance.ChangeScene("LobbyScene");
    }

    public void OpenSound()
    {
        UIManager.Instance.OpenSound(OptionUIs);
    }

    public void CloseSound()
    {
        UIManager.Instance.CloseSound(OptionUIs);
    }

    public void OpenScreen()
    {
        UIManager.Instance.OpenScreen(OptionUIs);
    }

    public void CloseScreen()
    {
        UIManager.Instance.CloseScreen(OptionUIs);
    }

    public void MusicVolume()
    {
        UIManager.Instance.MusicVolume(_musicSlider);
    }

    public void MusicSFX()
    {
        UIManager.Instance.MusicSFX(_sfxSlider);
    }

    public void ScreenToggle()
    {
        UIManager.Instance.FullToggle();
    }
}
