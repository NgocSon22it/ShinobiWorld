using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject AudioPanel;

    [SerializeField] Toggle MusicCheckBox;
    [SerializeField] Toggle SoundCheckBox;

    [SerializeField] AudioMixer MusicAudioMixer;
    [SerializeField] AudioMixer SoundAudioMixer;

    public bool MusicStatus, SoundStatus;
    string MusicValue, SoundValue;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ToggleMusic()
    {
        if (MusicCheckBox.isOn)
        {
            MusicAudioMixer.SetFloat("Volume", 0f);
            MusicStatus = true;
        }
        else
        {
            MusicAudioMixer.SetFloat("Volume", -80f);
            MusicStatus = false;
        }
    }
    public void ToggleSound()
    {
        if (SoundCheckBox.isOn)
        {
            SoundAudioMixer.SetFloat("Volume", 0f);
            SoundStatus = true;

        }
        else
        {
            SoundAudioMixer.SetFloat("Volume", -80f);
            SoundStatus = false;

        }
    }

    public void LoadCustomSound()
    {
        MusicValue = CustomSetting_DAO.GetAccountCustomSettingBySettingID(References.accountRefer.ID, "Music_Background").Value;
        SoundValue = CustomSetting_DAO.GetAccountCustomSettingBySettingID(References.accountRefer.ID, "Music_Effects").Value;

        MusicStatus = MusicValue == "1" ? true : false;
        SoundStatus = SoundValue == "1" ? true : false;
        MusicCheckBox.isOn = MusicValue == "1" ? true : false;
        SoundCheckBox.isOn = SoundValue == "1" ? true : false;
    }

    public void OpenSoundSetting()
    {
        LoadCustomSound();

        AudioPanel.SetActive(true);
    }

    public void CloseSoundSetting()
    {
        if (MusicStatus)
        {          
            Account_DAO.ChangeKey(References.accountRefer.ID, "Music_Background", "1");
        }
        else
        {
            Account_DAO.ChangeKey(References.accountRefer.ID, "Music_Background", "0");
        }

        if (SoundStatus)
        {

            Account_DAO.ChangeKey(References.accountRefer.ID, "Music_Effects", "1");
        }
        else
        {
            Account_DAO.ChangeKey(References.accountRefer.ID, "Music_Effects", "0");
        }
        
        AudioPanel.SetActive(false);
    }

}
