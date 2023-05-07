using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject loginPanel;

    public GameObject registrationPanel;

    public GameObject emailVerificationPanel;

    public GameObject gamePanel;

    public TMP_Text message;

    private void Awake()
    {
        CreateInstance();
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

    void ClearUI()
    {
        loginPanel.SetActive(false);
        registrationPanel.SetActive(false);
        emailVerificationPanel.SetActive(false);
        gamePanel.SetActive(false);
    }

    public void ShowEmailVerificationPanel(bool isEmailSent, string emailId, string errorMessage)
    {
        ClearUI();
        emailVerificationPanel.SetActive(true);

        if(isEmailSent)
        {
            message.text = $"Please verify your email address \n Verification email has been sent to {emailId}";    
        }
        else
        {
            message.text = $"Couldn't sent email : {errorMessage}";
        }
    }
}
