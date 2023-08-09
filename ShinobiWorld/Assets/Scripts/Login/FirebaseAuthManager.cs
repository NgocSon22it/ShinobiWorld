﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using TMPro;
using Assets.Scripts.Database.DAO;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using Assets.Scripts.GameManager;

public class FirebaseAuthManager : MonoBehaviourPunCallbacks
{
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    private int playerCount;


    private void Start()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            StartCoroutine(CheckAndFixDependenciesAsync());
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }
    }


    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();


        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        dependencyStatus = dependencyTask.Result;

        if (dependencyStatus == DependencyStatus.Available)
        {
            InitializeFirebase();

            yield return new WaitForEndOfFrame();

            StartCoroutine(CheckForAutoLogin());
        }
        else
        {
            Debug.Log("Could not resolve all firebase dependencies: " + dependencyStatus);
        }
    }


    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private IEnumerator CheckForAutoLogin()
    {
        if (user != null)
        {
            var reloadUserTask = user.ReloadAsync();

            yield return new WaitUntil(() => reloadUserTask.IsCompleted);

            AutoLogin();
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }

    private void AutoLogin()
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
            {
                References.accountRefer.ID = user.UserId;
                

                var isOnline = Account_DAO.StateOnline(user.UserId);

                if (isOnline)
                {
                    if( Disconnect.Instance.ReadFile() ) AutoLogin();
                    else UIManager.Instance.OpenPopupPanel(Message.Logined);
                }
                else
                {
                    Account_DAO.ChangeStateOnline(user.UserId, true);
                    References.accountRefer = Account_DAO.GetAccountByID(References.accountRefer.ID);
                    PhotonNetwork.NickName = References.accountRefer.Name;
                    References.InitSaveValue();
                    if (!PhotonNetwork.IsConnected)
                    {
                        PhotonNetwork.ConnectUsingSettings();
                    }
                }

                Debug.LogFormat("{0} Successfully Auto Logged In", PhotonNetwork.NickName);
                Debug.LogFormat("{0} Successfully Auto Logged In", user.UserId);
            }
            else
            {
                SendEmailForVerification();
            }
        }
        else
        {
            UIManager.Instance.OpenLoginPanel();
        }
    }


    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.DisplayName);
                UIManager.Instance.OpenLoginPanel();
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.DisplayName);
            }
        }
    }

    public void Login()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            var email = UIManager.Instance.emailLoginField.text;
            var password = UIManager.Instance.passwordLoginField.text;
            StartCoroutine(LoginAsync(email, password));
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        if (email == "")
        {
            Debug.Log("ShinobiWorld " + Message.EmailEmpty);
            UIManager.Instance.OpenPopupPanel(Message.EmailEmpty);
        }
        else if (password == "")
        {
            Debug.Log("ShinobiWorld " + Message.PasswordEmpty);
            UIManager.Instance.OpenPopupPanel(Message.PasswordEmpty);

        }
        else
        {
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.Log("ShinobiWorld " + loginTask.Exception);

                FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;


                string failedMessage = "Login Failed! Because ";

                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += Message.EmailInvalid;
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += Message.PasswordWrong;
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += Message.EmailEmpty;
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += Message.PasswordEmpty;
                        break;
                    case AuthError.UserNotFound:
                        failedMessage += Message.EmailNotExist;
                        break;
                    default:
                        Debug.Log(failedMessage += authError.ToString() + "default");
                        failedMessage = Message.ErrorSystem;
                        break;
                }

                UIManager.Instance.OpenPopupPanel(failedMessage);

            }
            else
            {
                user = loginTask.Result;


                if (user.IsEmailVerified)
                {
                    References.accountRefer.ID = user.UserId;
                    var isOnline = Account_DAO.StateOnline(user.UserId);

                    if (isOnline)
                    {
                        if (Disconnect.Instance.ReadFile())
                        {
                            StopAllCoroutines();
                            Login();
                        }
                        else UIManager.Instance.OpenPopupPanel(Message.Logined);
                    }
                    else
                    {
                        Account_DAO.ChangeStateOnline(user.UserId, true);
                        References.accountRefer = Account_DAO.GetAccountByID(References.accountRefer.ID);
                        PhotonNetwork.NickName = References.accountRefer.Name;
                        References.InitSaveValue();
                        if (!PhotonNetwork.IsConnected)
                        {
                            PhotonNetwork.ConnectUsingSettings();
                        }
                    }
                    Debug.LogFormat("{0} You Are Successfully Logged In", PhotonNetwork.NickName);

                }
                else
                {
                    SendEmailForVerification();
                }

            }
        }
    }

    public void Register()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            var email = UIManager.Instance.emailRegisterField.text;
            var password = UIManager.Instance.passwordRegisterField.text;
            var confirmPassword = UIManager.Instance.confirmPasswordRegisterField.text;

            StartCoroutine(RegisterAsync(email, password, confirmPassword));
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }
    }

    private IEnumerator RegisterAsync(string email, string password, string confirmPassword)
    {
        if (email == "")
        {
            Debug.Log("ShinobiWorld " + Message.EmailEmpty);
            UIManager.Instance.OpenPopupPanel(Message.EmailEmpty);

        }
        else if (password == "")
        {
            Debug.Log("ShinobiWorld " + Message.PasswordEmpty);
            UIManager.Instance.OpenPopupPanel(Message.PasswordEmpty);

        }
        else if (password.Length < 6)
        {
            Debug.Log("ShinobiWorld " + "Password must be at least 8 characters");
            UIManager.Instance.OpenPopupPanel(Message.PasswordInvalid);

        }
        //else if (password.Contains(" "))
        //{
        //    Debug.Log("ShinobiWorld " + "Password mustn't be space");
        //    UIManager.Instance.OpenPopupPanel(Message.PasswordInvalid);
        //}
        else if (password != confirmPassword)
        {
            Debug.Log("ShinobiWorld " + Message.PasswordNotMatch);
            UIManager.Instance.OpenPopupPanel(Message.PasswordNotMatch);
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.Log("ShinobiWorld " + registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += Message.EmailInvalid;
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += Message.PasswordWrong;
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += Message.EmailEmpty;
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += Message.PasswordEmpty;
                        break;
                    case AuthError.EmailAlreadyInUse:
                        failedMessage += Message.EmailAlready;
                        break;
                    default:
                        Debug.Log(failedMessage += authError.ToString() + "default");
                        failedMessage = Message.ErrorSystem;
                        break;
                }

                UIManager.Instance.OpenPopupPanel(failedMessage);

            }
            else
            {
                Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                if (user.IsEmailVerified)
                {
                    UIManager.Instance.OpenLoginPanel();
                }
                else
                {
                    Account_DAO.CreateAccount(user.UserId);
                    MailBox_DAO.AddMailbox(user.UserId, References.MailSystem, true);
                    SendEmailForVerification();
                }
            }
        }
    }

    public void SendEmailForVerification()
    {
        StartCoroutine(SendEmailForVerificatioAsync());
    }

    private IEnumerator SendEmailForVerificatioAsync()
    {
        if (user != null)
        {
            var sendEmailTask = user.SendEmailVerificationAsync();

            yield return new WaitUntil(() => sendEmailTask.IsCompleted);

            if (sendEmailTask.Exception != null)
            {
                FirebaseException firebaseException = sendEmailTask.Exception.GetBaseException() as FirebaseException;

                AuthError error = (AuthError)firebaseException.ErrorCode;

                string errorMessage = Message.ErrorSystem;

                switch (error)
                {
                    case AuthError.Cancelled:
                        errorMessage = Message.VerifyEmailCanceled;
                        break;
                    case AuthError.TooManyRequests:
                        errorMessage = Message.VerifyEmailTooManyRequests;
                        break;
                    case AuthError.InvalidRecipientEmail:
                        errorMessage = Message.EmailInvalid;
                        break;
                }
                UIManager.Instance.OpenPopupPanel(errorMessage);
            }
            else
            {
                Debug.Log("Email has successfully sent");
                UIManager.Instance.OpenLoginPanel();
                UIManager.Instance.OpenPopupPanel(string.Format(Message.EmailMessage, user.Email));
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        playerCount = PhotonNetwork.CountOfPlayers;
        UIManager.Instance.OpenGamePanel();

    }


    public void OpenGameScene()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            if (playerCount >= 0 && playerCount < References.Maxserver)
            {
                References.PlayerSpawnPosition = References.HouseAddress[House.Hokage.ToString()];
                References.IsFirstLogin = Account_DAO.IsFirstLogin(user.UserId);
                if (References.IsFirstLogin)
                {
                    PhotonNetwork.LoadLevel(Scenes.Creator);
                }
                else
                {
                    PhotonNetwork.LoadLevel(Scenes.Konoha);
                }

            }
            else
            {
                UIManager.Instance.OpenPopupPanel(Message.Maxplayer);
            }
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }


    }

    private void OnApplicationQuit()
    {
        if (References.accountRefer != null && PhotonNetwork.IsConnectedAndReady)
        {
            References.UpdateAccountToDB();
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (cause == DisconnectCause.MaxCcuReached)
        {
            UIManager.Instance.OpenPopupPanel(Message.Maxplayer);
        }
        else if (cause == DisconnectCause.ExceptionOnConnect)
        {
            Debug.Log("Failed to connect to Photon: Exception on connect");
        }
        else
        {
            Debug.Log("Failed to connect to Photon: " + cause.ToString());
        }
    }


    public void Logout()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            if (auth != null && user != null)
            {
                References.UpdateAccountToDB();
                Account_DAO.ChangeStateOnline(user.UserId, false);
                auth.SignOut();
                PhotonNetwork.Disconnect();
            }
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }      
    }


    public void ResetPassword()
    {
        if (UIManager.Instance.IsWiFiConnected())
        {
            var email = UIManager.Instance.emailResetField.text;

            StartCoroutine(ResetPassword(email));
        }
        else
        {
            UIManager.Instance.OpenLostWifiPanel();
        }

    }


    private IEnumerator ResetPassword(string email)
    {
        if (email == "")
        {
            Debug.Log("ShinobiWorld " + Message.EmailEmpty);
            UIManager.Instance.OpenPopupPanel(Message.EmailEmpty);

        }
        else
        {
            var resetTask = auth.SendPasswordResetEmailAsync(email);

            yield return new WaitUntil(() => resetTask.IsCompleted);

            if (resetTask.Exception != null)
            {
                Debug.Log("ShinobiWorld " + resetTask.Exception);

                FirebaseException firebaseException = resetTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Gửi email đặt lại mật khẩu không thành công! ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += Message.EmailInvalid;
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += Message.EmailEmpty;
                        break;
                    default:
                        Debug.Log(failedMessage += authError.ToString() + "default");
                        failedMessage = Message.ErrorSystem;
                        break;
                }

                UIManager.Instance.OpenPopupPanel(failedMessage);

            }
            else
            {
                UIManager.Instance.OpenLoginPanel();
                UIManager.Instance.OpenPopupPanel(string.Format(Message.EmailResetPassMessage, email));
            }
        }
    }
}
