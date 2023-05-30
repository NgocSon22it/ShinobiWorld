using Assets.Scripts.Database.DAO;
using Assets.Scripts.Database.Entity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using TMPro;
using System.Security.Principal;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviourPunCallbacks, IPunObservable
{
    // Entity
    protected Boss_Entity boss_Entity = new Boss_Entity();
    int CurrentHealth;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] TMP_Text HealthNumber;

    //Component
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2d;
    public Animator animator;

    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Start()
    {
        SetUpComponent();

        if (photonView.IsMine)
        {
            if (boss_Entity.ID != null)
            {
                boss_Entity = Boss_DAO.GetBossByID(boss_Entity.ID);
                CurrentHealth = boss_Entity.Health;
            }
        }   

        LoadHealthUI();
    }

    public void LoadHealthUI()
    {
        HealthNumber.text = CurrentHealth + " / " + boss_Entity.Health;
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)boss_Entity.Health;
    }

    public void TakeDamage(string UserID, int Damage)
    {
        photonView.RPC(nameof(TakeDamageSync), RpcTarget.AllBuffered, UserID, Damage);
    }

    IEnumerator DamageAnimation()
    {

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.2f);
        spriteRenderer.color = Color.white;
    }

    [PunRPC]
    public void TakeDamageSync(string UserID, int Damage)
    {
        CurrentHealth -= Damage;
        StartCoroutine(DamageAnimation());
        LoadHealthUI();
        if(CurrentHealth < 0)
        {
            Debug.Log(UserID);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CurrentHealth);
            stream.SendNext(boss_Entity.Health);
        }
        else
        {
            CurrentHealth = (int)stream.ReceiveNext();
            boss_Entity.Health = (int)stream.ReceiveNext();

        }
    }
}
