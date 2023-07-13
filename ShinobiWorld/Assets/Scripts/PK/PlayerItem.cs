using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    [SerializeField] TMP_Text PlayerNameTxt;

    Player player;

    public void SetUp(Player _player)
    {
        player = _player;
        SetPLayerData(player);

    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (targetPlayer != null && targetPlayer == player)
        {
            SetPLayerData(targetPlayer);
        }
    }
    public void SetPLayerData(Player player)
    {
        PlayerNameTxt.text = player.NickName;

        /*IsReady = (bool)player.CustomProperties["IsReady"];

        PlayerLevelTxt.text = "Level " + Account.Level.ToString();

        if (IsReady)
        {
            IsReadyGameobject.SetActive(true);
            NotReadyGameobject.SetActive(false);
        }
        else
        {
            IsReadyGameobject.SetActive(false);
            NotReadyGameobject.SetActive(true);
        }*/

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
