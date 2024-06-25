using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // 텍스트 랜덤출력 기능
    public void PrintRandomText(Text uiText, string[] messages)
    {
        int randomIndex = Random.Range(0, messages.Length);
        string randomMessage = messages[randomIndex];
        uiText.text = randomMessage;
    }

    // 환경설정 On/Off
    public void OptionUI(GameObject OptionUI)
    {
        Animator OptionUIAnimator = OptionUI.GetComponent<Animator>();
        bool isOpen = OptionUIAnimator.GetBool("isOpen");
        OptionUIAnimator.SetBool("isOpen", !isOpen);
    }

    public void OptionUIInit(GameObject[] OptionUIs)
    {
        OptionUIs[1].SetActive(true);
        OptionUIs[2].SetActive(false);
        OptionUIs[3].SetActive(false);
    }

    public void OpenSound(GameObject[] OptionUIs)
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        OptionUIs[1].SetActive(false);
        OptionUIs[2].SetActive(true);
    }

    public void CloseSound(GameObject[] OptionUIs)
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        OptionUIs[1].SetActive(true);
        OptionUIs[2].SetActive(false);
    }

    public void OpenScreen(GameObject[] OptionUIs)
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        OptionUIs[1].SetActive(false);
        OptionUIs[3].SetActive(true);
    }
    public void CloseScreen(GameObject[] OptionUIs)
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        OptionUIs[1].SetActive(true);
        OptionUIs[3].SetActive(false);
    }

    public void MusicVolume(Slider _musicSlider)
    {
        AudioManager.Instance.MusicVolume(_musicSlider.value);
    }

    public void MusicSFX(Slider _sfxSlider)
    {
        AudioManager.Instance.SFXVolume(_sfxSlider.value);
    }

    public void FullToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
