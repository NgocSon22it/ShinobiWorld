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
    public ChatClient chatClient;

    [SerializeField] GameObject ChatRoom;
    [SerializeField] TMP_Text ChatDisPlay;
    [SerializeField] TMP_InputField ChatField;

    [SerializeField] GameObject TypingChatObject;

    [SerializeField] Scrollbar VerticalScroll;

    public string ServerName;

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
        Debug.Log("Kết nối đến chat thành công!");
        isConnected = true;
        chatClient.Subscribe(new string[] { ServerName });
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
        }
        InviteManager.Instance.CloseReceiveInvitePopup();

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            // Channel Name format [Sender : Recipient]
            string senderName = channelName.Split(new char[] { ':' })[0];

            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"{sender}: {message}");
                var mess = message.ToString().Split(new char[] { ':' });

                var type = (TypePrivateMessage)Enum.Parse(typeof(TypePrivateMessage), mess[0]);
                switch (type)
                {
                    case TypePrivateMessage.FriendRequest:
                        FriendManager.Instance.Notify.SetActive(true);
                        break;
                    case TypePrivateMessage.PK:
                        var PKMessage = mess[1];
                        var PKSceneName = mess[2];
                        var PKRoomName = mess[3];
                        var PKBet = mess[4];
                        InviteManager.Instance.OpenReceiveInvitePopup_PK(TypePrivateMessage.PK, sender + " " + PKMessage, PKSceneName, PKRoomName, PKBet);
                        break;
                    case TypePrivateMessage.Arena:
                        var ArenaMessage = mess[1];
                        var SceneName = mess[2];
                        var RoomName = mess[3];
                        var BossName = mess[4];
                        InviteManager.Instance.OpenReceiveInvitePopup_Arena(TypePrivateMessage.Arena, sender + " " + ArenaMessage, SceneName, RoomName, BossName, References.bossArenaType);
                        break;
                }
            }
        }
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

    public void ConnectToChat(string ServerName)
    {
        this.ServerName = ServerName;
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(References.accountRefer.Name));
        Debug.Log("Đang kết nối");
    }

    public void TypeChatOnValueChange(string value)
    {
        CurrentChat = value;
    }

    public void SummitPublicChat()
    {
        chatClient.PublishMessage(ServerName, CurrentChat);
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
        ChatField.Select();
        ChatField.ActivateInputField();
    }


    // Update is called once per frame
    void Update()
    {

        if (isConnected)
        {
            chatClient.Service();
            if (Input.GetKeyDown(KeyCode.Return) && IsTypingChat == false && Game_Manager.Instance.IsBusy == false)
            {
                ToggleTyping(true);
                FocusTyping();
            }
            else if (string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
            {
                ToggleTyping(false);
            }
        }
        if (!string.IsNullOrEmpty(CurrentChat) && Input.GetKeyDown(KeyCode.Return) && IsTypingChat == true)
        {
            SummitPublicChat();
            FocusTyping();
            VerticalScroll.value = 0f;
        }
    }
}
