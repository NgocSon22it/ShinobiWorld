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
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;
    public GameObject registrationPanel;


    private void Awake()
    {
        CreateInstance();
        Canvas.SetActive(true);
    }

    private void CreateInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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

    void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        //emailVerificationPanel.SetActive(false);
        gamePanel.SetActive(false);
        popupPanel.SetActive(false);

        //clear input
        passwordLoginField.text = "";
        emailLoginField.text = "";
        confirmPasswordRegisterField.text = "";
        passwordRegisterField.text = "";
        emailRegisterField.text = "";
        nameRegisterField.text = "";
    }
}
