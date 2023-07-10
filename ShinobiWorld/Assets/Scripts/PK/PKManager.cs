using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PKManager : MonoBehaviour
{
    public GameObject PKPanel, AcceptPanel;
    public TMP_Text PKContent, AcceptContent;
    public Button AcceptBtn, CancelBtn, CloseAcceptPanel;

    public static PKManager Instance;

    string senderName;

    private void Awake()
    {
        Instance = this;
        CancelBtn.onClick.AddListener(() => Close());
        AcceptBtn.onClick.AddListener(() => Accept());

    }


    public void Open(string SenderName)
    {
        PKPanel.SetActive(true);
        senderName = SenderName;
        PKContent.text = string.Format(Message.PKMessage, SenderName);

    }

    public void ShowAccept(string SenderName)
    {
        
        AcceptPanel.SetActive(true);
        AcceptContent.text = $"{senderName} đã chấp nhận lời thách đấu!";
        CloseAcceptPanel.onClick.AddListener(() => { AcceptPanel.SetActive(false); });
    }

    public void Accept()
    {
        Close();
        ChatManager.Instance.chatClient
                    .SendPrivateMessage(senderName,
                    string.Format(Message.PriviteMessage, TypePriviteMessage.PKRequest.ToString(), "1"));
    }

    public void Close()
    {
        PKPanel.SetActive(false);
    }




}
