using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("# Audio Setting")]
    [SerializeField] Slider _musicSlider;
    [SerializeField] Slider _sfxSlider;

    [Header("# Menu Control")]
    [SerializeField] GameObject[] OptionUIs;
    [SerializeField] NetworkManager networkManager;

    bool isPaused = false;

    private void Start()
    {
        isPaused = false;
        networkManager = FindObjectOfType<NetworkManager>();
        AudioInit();
    }

    private void AudioInit()
    {
        _musicSlider.value = AudioManager.Instance.musicSource.volume;
        _sfxSlider.value = AudioManager.Instance.sfxSource.volume;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Pause()
    {
        OptionUIs[0].SetActive(true);
        GameManager.Instance.GameStop();
        isPaused = true;
    }

    public void GameExit()
    {
        AudioManager.Instance.PlaySFX(SoundName.Button);
        GameSceneManager.Instance.GameExit();
    }

    public void Resume()
    {
        OptionUIs[0].SetActive(false);
        GameManager.Instance.GameResume();
        isPaused = false;
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

    public void BackToStart()
    {
        NetworkManager.Instance.LeaveRobby();
    }
}
