using Assets.Scripts.Database.DAO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject Canvas;

    [Header("Panel")]

    //public GameObject emailVerificationPanel;

    public GameObject gamePanel;

    public GameObject popupPanel;

    //public TMP_Text message;

    public TMP_Text messagePopup;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public GameObject loginPanel;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;
    public GameObject registrationPanel;

    // Reset Variables
    [Space]
    [Header("Reset")]
    public TMP_InputField emailResetField;
    public GameObject resetPanel;

    [Space]
    [Header("Wifi")]
    public GameObject LostWifiPanel;

    private void Awake()
    {
        CreateInstance();
        Canvas.SetActive(true);
    }

    public bool IsWiFiConnected()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return true; 
            }
        }

        return false; 
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void OpenLostWifiPanel()
    {
        LostWifiPanel.SetActive(true);
    }

    public void CloseLostWifiPanel()
    {
        LostWifiPanel.SetActive(false);
    }

    public void OpenLoginPanel()
    {
        ClearUI();
        loginPanel.SetActive(true);
    }

    public void OpenRegistrationPanel()
    {
        ClearUI();
        registrationPanel.SetActive(true);
    }

    public void OpenGamePanel()
    {
        ClearUI();
        gamePanel.SetActive(true);
    }

    public void OpenPopupPanel(string message)
    {
        popupPanel.SetActive(true);
        messagePopup.text = message;
    }

    public void ClosePopupPanel()
    {
        popupPanel.SetActive(false);
    }

    public void OpenResetPanel()
    {
        ClearUI();
        resetPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        resetPanel.SetActive(false);
        //emailVerificationPanel.SetActive(false);
        gamePanel.SetActive(false);
        popupPanel.SetActive(false);

        //clear input
        passwordLoginField.text = "";
        emailLoginField.text = "";
        confirmPasswordRegisterField.text = "";
        passwordRegisterField.text = "";
        emailRegisterField.text = "";

        emailResetField.text = "";
    }
}
