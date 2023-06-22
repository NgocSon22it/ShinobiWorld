using System.Collections;
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
        StartCoroutine(CheckAndFixDependenciesAsync());
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
            Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
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
                PhotonNetwork.NickName = user.DisplayName;
                Account_DAO.ChangeStateOnline(user.UserId, true);
                References.accountRefer = Account_DAO.GetAccountByID(References.accountRefer.ID);

                if (!PhotonNetwork.IsConnected)
                {
                    PhotonNetwork.ConnectUsingSettings(); 
                }
                Debug.LogFormat("{0} Successfully Auto Logged In", user.DisplayName);
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
        var email = UIManager.Instance.emailLoginField.text;
        var password = UIManager.Instance.passwordLoginField.text;
        StartCoroutine(LoginAsync(email, password));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        if (email == "")
        {
            Debug.LogError("ShinobiWorld " + Message.EmailEmpty);
            UIManager.Instance.OpenPopupPanel(Message.EmailEmpty);
        }
        else if (password == "")
        {
            Debug.LogError("ShinobiWorld " + Message.PasswordEmpty);
            UIManager.Instance.OpenPopupPanel(Message.PasswordEmpty);

        }
        else
        {
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                Debug.LogError("ShinobiWorld " + loginTask.Exception);

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

                Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);

                if (user.IsEmailVerified)
                {
                    References.accountRefer.ID = user.UserId;

                    PhotonNetwork.NickName = user.DisplayName; //Set name user

                    var isOnline = Account_DAO.StateOnline(user.UserId);

                    if (isOnline)
                    {
                        UIManager.Instance.OpenPopupPanel(Message.Logined);
                    }
                    else
                    {
                        Account_DAO.ChangeStateOnline(user.UserId, true);
                        References.accountRefer = Account_DAO.GetAccountByID(References.accountRefer.ID);
                        if (!PhotonNetwork.IsConnected)
                        {
                            PhotonNetwork.ConnectUsingSettings();
                        }
                    }
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
        var name = UIManager.Instance.nameRegisterField.text;
        var email = UIManager.Instance.emailRegisterField.text;
        var password = UIManager.Instance.passwordRegisterField.text;
        var confirmPassword = UIManager.Instance.confirmPasswordRegisterField.text;

        StartCoroutine(RegisterAsync(name, email, password, confirmPassword));
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("ShinobiWorld " + Message.NameEmpty);
            UIManager.Instance.OpenPopupPanel(Message.NameEmpty);
        }
        else if (name.Length < 4 || name.Length > 16)
        {
            Debug.LogError("ShinobiWorld " + Message.NameInvalid);
            UIManager.Instance.OpenPopupPanel(Message.NameInvalid);
        }
        else if (email == "")
        {
            Debug.LogError("ShinobiWorld " + Message.EmailEmpty);
            UIManager.Instance.OpenPopupPanel(Message.EmailEmpty);

        }
        else if (password == "")
        {
            Debug.LogError("ShinobiWorld " + Message.PasswordEmpty);
            UIManager.Instance.OpenPopupPanel(Message.PasswordEmpty);

        }
        else if (password.Length < 8)
        {
            Debug.LogError("ShinobiWorld " + "Password must be at least 8 characters");
            UIManager.Instance.OpenPopupPanel(Message.PasswordInvalid);

        }
        else if (password.Contains(" "))
        {
            Debug.LogError("ShinobiWorld " + "Password mustn't be space");
            UIManager.Instance.OpenPopupPanel(Message.PasswordInvalid);
        }
        else if (password != confirmPassword)
        {
            Debug.LogError("ShinobiWorld " + Message.PasswordNotMatch);
            UIManager.Instance.OpenPopupPanel(Message.PasswordNotMatch);
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError("ShinobiWorld " + registerTask.Exception);

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
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError("ShinobiWorld " + updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                    UIManager.Instance.OpenPopupPanel(Message.ErrorSystem);
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
                        SendEmailForVerification();
                    }
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
        Debug.Log("Number of players on master server: " + playerCount);
        UIManager.Instance.OpenGamePanel();
    }

    public void OpenGameScene()
    {
        if (playerCount > 0 && playerCount < References.Maxserver)
        {
            if (Account_DAO.IsFirstLogin(user.UserId))
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

    private void OnApplicationQuit()
    {
        if (References.accountRefer != null && PhotonNetwork.IsConnectedAndReady)
        {
            Account_DAO.ChangeStateOnline(References.accountRefer.ID, false);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Handle the disconnect cause
        if (cause == DisconnectCause.MaxCcuReached)
        {
            Debug.LogError("Failed to connect to Photon: Full");
        }
        else if (cause == DisconnectCause.ExceptionOnConnect)
        {
            Debug.LogError("Failed to connect to Photon: Exception on connect");
        }
        else
        {
            Debug.LogError("Failed to connect to Photon: " + cause.ToString());
        }
    }

    public void Logout()
    {
        if (auth != null && user != null)
        {
            Account_DAO.ChangeStateOnline(user.UserId, false);
            auth.SignOut();
            PhotonNetwork.Disconnect();
        }
    }
}
