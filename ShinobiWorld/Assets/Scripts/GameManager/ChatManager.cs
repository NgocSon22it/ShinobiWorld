using Assets.Scripts.Database.DAO;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;

    [SerializeField] GameObject ChatRoom;
    [SerializeField] TMP_Text ChatDisPlay;
    [SerializeField] TMP_InputField ChatField;

    [SerializeField] GameObject TypingChatObject;

    [SerializeField] Scrollbar VerticalScroll;

    public bool IsTypingChat;

    string CurrentChat;

    public static ChatManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        Debug.Log("Connected To Chat");
        isConnected = true;
        chatClient.Subscribe(new string[] { "Akatsucana" });
    }

    public void OnDisconnected()
    {

    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string mess = "";
        // Get the current time
        DateTime currentTime = DateTime.Now;

        // Get the current time as a string in the format "HH:mm"
        string currentTimeString = currentTime.ToString("HH:mm");

        for (int i = 0; i < senders.Length; i++)
        {
            mess = string.Format("[{2}] {0}: {1}", senders[i], messages[i], currentTimeString);

            ChatDisPlay.text += "\n " + mess;

            Debug.Log(mess);
        }
        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        ChatRoom.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {

    }

    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }

    [SerializeField] string userID;
    bool isConnected;

    public void ConnectToChat()
    {
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(References.accountRefer.Name));
        Debug.Log("Connecting");
    }

    public void TypeChatOnValueChange(string value)
    {
        CurrentChat = value;
        Debug.Log(CurrentChat);
    }

    public void SummitPublicChat()
    {
        chatClient.PublishMessage("Akatsucana", CurrentChat);
        ChatField.text = "";
        CurrentChat = "";
    }

    public void ToggleTyping(bool value)
    {
        IsTypingChat = value;
        TypingChatObject.SetActive(value);
        Game_Manager.Instance.IsBusy = value;
    }

    public void FocusTyping()
    {
        // Select the input field
        ChatField.Select();
        // Set the focus to the input field
        ChatField.ActivateInputField();      
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && IsTypingChat == false)
        {
            ToggleTyping(true);
            FocusTyping();
        }
        else if (string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
        {
            ToggleTyping(false);
        }

        if (isConnected)
        {
            chatClient.Service();
            
        }

        if (!string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
        {
            SummitPublicChat();          
            FocusTyping();
            VerticalScroll.value = 0f;
        }
    }
}
