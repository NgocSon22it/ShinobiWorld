using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Assets.Scripts.Database.DAO;
using Photon.Pun.UtilityScripts;
using Assets.Scripts.Database.Entity;
using ExitGames.Client.Photon;
using System.Security.Principal;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun.Demo.PunBasics;
using WebSocketSharp;

public class PlayerBase : MonoBehaviourPunCallbacks, IPunObservable
{

    public AccountSkill_Entity SkillOne_Entity = new AccountSkill_Entity();
    public AccountSkill_Entity SkillTwo_Entity = new AccountSkill_Entity();
    public AccountSkill_Entity SkillThree_Entity = new AccountSkill_Entity();

    public string WeaponName, SkillOneName, SkillTwoName, SkillThreeName;

    [Header("Player Instance")]
    [SerializeField] GameObject PlayerCameraPrefabs;
    [SerializeField] GameObject PlayerAllUIPrefabs;

    [SerializeField] TMP_Text PlayerNickName;
    [SerializeField] GameObject PlayerHealthChakraUI;

    public GameObject PlayerCameraInstance;
    public GameObject PlayerAllUIInstance;

    [SerializeField] public LayerMask AttackableLayer;
    //Attack
    [SerializeField] public Transform AttackPoint;

    //Skill
    public float SkillOneCooldown_Total;
    public float SkillOneCooldown_Current;

    public float SkillTwoCooldown_Total;
    public float SkillTwoCooldown_Current;

    public float SkillThreeCooldown_Total;
    public float SkillThreeCooldown_Current;

    //Attack
    public float AttackCooldown_Total;
    public float AttackCooldown_Current;

    //Enemy
    protected GameObject Enemy;

    //Mouse
    protected Vector3 targetPosition;

    //Direction
    protected Vector2 SkillDirection;

    //MainPoint
    [SerializeField] Transform MainPoint;

    [SerializeField] GameObject Quai;
    [SerializeField] GameObject Quai1;

    //Bonus
    public int DamageBonus, SpeedBonus;

    //Component
    public Animator animator;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;
    public PlayerInput playerInput;

    //Script Component
    public Player_Pool playerPool;
    public Player_LevelManagement player_LevelManagement;


    //Player Input
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;
    bool FacingRight = true;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] Image CurrentChakra_UI;

    [SerializeField] TMP_Text CurrentHealth_NumberUI;
    [SerializeField] TMP_Text CurrentChakra_NumberUI;

    //Sprite layout
    //Skin
    public SpriteRenderer Shirt;
    public SpriteRenderer RightFoot;
    public SpriteRenderer LeftFoot;
    public SpriteRenderer RightHand;
    public SpriteRenderer LeftHand;
    //Eye
    public SpriteRenderer Eye;
    //Hair
    public SpriteRenderer Hair;
    //Mouth
    public SpriteRenderer Mouth;

    //Walk Interaction
    public bool CanWalking;

    // Lag
    Vector3 realPosition;
    Quaternion realRotation;
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    Vector3 positionAtLastPacket = Vector3.zero;
    Quaternion rotationAtLastPacket = Quaternion.identity;


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer != null && targetPlayer.Equals(photonView.Owner))
        {
            if (changedProps.ContainsKey("Account"))
            {
                string accountJson = (string)changedProps["Account"];
                References.accountRefer = JsonUtility.FromJson<Account_Entity>(accountJson);
                SetUpAccountData();
            }

        }
    }

    public void SetUpAccountData()
    {
        PlayerNickName.text = photonView.Owner.NickName;
        LoadLayout();
        LoadAllAccountUI();
        LoadAccountWeapon();
        LoadAccountSkill();
    }

    public void SetUpComponent()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
        playerInput = GetComponent<PlayerInput>();
        playerPool = GetComponent<Player_Pool>();
        player_LevelManagement = GetComponent<Player_LevelManagement>();
    }

    public void LoadLayout()
    {
        if (References.accountRefer != null)
        {
            string image = References.listEye.Find(obj => obj.ID == References.accountRefer.EyeID).Image;
            Eye.sprite = Resources.Load<Sprite>(image);

            //Hair
            image = References.listHair.Find(obj => obj.ID == References.accountRefer.HairID).Image;
            Hair.sprite = Resources.Load<Sprite>(image);

            //Mouth
            image = References.listMouth.Find(obj => obj.ID == References.accountRefer.MouthID).Image;
            Mouth.sprite = Resources.Load<Sprite>(image);

            //Skin
            image = References.listSkin.Find(obj => obj.ID == References.accountRefer.SkinID).Image;

            Shirt.sprite = Resources.Load<Sprite>(image + "_Shirt");
            LeftHand.sprite = Resources.Load<Sprite>(image + "_LeftHand");
            RightHand.sprite = Resources.Load<Sprite>(image + "_RightHand");
            LeftFoot.sprite = Resources.Load<Sprite>(image + "_LeftFoot");
            RightFoot.sprite = Resources.Load<Sprite>(image + "_RightFoot");
        }
    }

    public void Start()
    {
        SetUpComponent();

        if (photonView.IsMine)
        {
            if (References.accountRefer != null)
            {
                AttackCooldown_Total = 0.5f;
                PlayerCameraInstance = Instantiate(PlayerCameraPrefabs);
                PlayerAllUIInstance = Instantiate(PlayerAllUIPrefabs);

                PlayerHealthChakraUI.SetActive(false);

                InvokeRepeating(nameof(RegenHealth), 1f, 1f);
                InvokeRepeating(nameof(RegenChakra), 1f, 1f);
                InvokeRepeating(nameof(RegenStrength), 1f, 360f);
            }
        }
        else
        {
            PlayerHealthChakraUI.SetActive(true);
        }
    }

    

    public void LoadAllAccountUI()
    {
        if (photonView.IsMine)
        {
            PlayerCameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;

            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadExperienceUI(References.accountRefer.Level, References.accountRefer.Exp, References.accountRefer.Level * 100);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadNameUI(photonView.Owner.NickName);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpCoinUI(References.accountRefer.Coin);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadPowerUI(Account_DAO.GetAccountPowerByID(References.accountRefer.ID));

            player_LevelManagement.GetComponent<Player_LevelManagement>().SetUpAccountEntity(References.accountRefer);
        }
    }

    public void RegenHealth()
    {
        HealAmountOfHealth(1);
    }

    public void RegenChakra()
    {
        HealAmountOfChakra(1);
    }

    public void HealAmountOfHealth(int Amount)
    {
        References.accountRefer.CurrentHealth += Amount;
        if (References.accountRefer.CurrentHealth >= References.accountRefer.Health)
        {
            References.accountRefer.CurrentHealth = References.accountRefer.Health;
        }
        LoadPlayerHealthUI();
    }


    public void HealAmountOfChakra(int Amount)
    {
        References.accountRefer.CurrentCharka += Amount;
        if (References.accountRefer.CurrentCharka >= References.accountRefer.Charka)
        {
            References.accountRefer.CurrentCharka = References.accountRefer.Charka;
        }
        LoadPlayerChakraUI();
    }

    public void EarnAmountOfExperience(int Amount)
    {
        if (photonView.IsMine)
        {
            player_LevelManagement.AddExperience(Amount);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadExperienceUI(References.accountRefer.Level, References.accountRefer.Exp, References.accountRefer.Level * 100);
        }
    }


    public void LoadPlayerChakraUI()
    {
        if (photonView.IsMine)
        {
            if (PlayerAllUIInstance != null)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
                LoadChakraUI((float)References.accountRefer.Charka, (float)References.accountRefer.CurrentCharka);
            }
        }
        else
        {
            CurrentChakra_UI.fillAmount = (float)References.accountRefer.CurrentCharka / (float)References.accountRefer.Charka;
            CurrentChakra_NumberUI.text = References.accountRefer.CurrentCharka + " / " + References.accountRefer.Charka;
        }
    }

    public void LoadPlayerHealthUI()
    {
        if (photonView.IsMine)
        {
            if (PlayerAllUIInstance != null)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
                LoadHealthUI((float)References.accountRefer.Health, (float)References.accountRefer.CurrentHealth);
            }
        }
        else
        {
            CurrentHealth_UI.fillAmount = (float)References.accountRefer.CurrentHealth / (float)References.accountRefer.Health;
            CurrentHealth_NumberUI.text = References.accountRefer.CurrentHealth + " / " + References.accountRefer.Health;
        }
    }
    public void Update()
    {
        if (photonView.IsMine)
        {
            animator.SetFloat("Horizontal", MoveDirection.x);
            animator.SetFloat("Vertical", MoveDirection.y);
            animator.SetFloat("Speed", MoveDirection.sqrMagnitude);
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 10;
            Attack();
            SkillOne();
            SkillTwo();
            SkillThree();

            if (Input.GetKeyDown(KeyCode.U))
            {
                PhotonNetwork.Instantiate("Boss/Normal/Bat/" + Quai.name, Vector3.zero, Quaternion.identity);
                PhotonNetwork.Instantiate("Boss/Normal/Fish/" + Quai1.name, Vector3.zero, Quaternion.identity);
            }

            if (!CanWalking)
            {
                MoveDirection = Vector2.zero;
            }
            else
            {
                MoveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();
            }

        }



    }

    public void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Walk();
        }
        else
        {
            rigidbody2d.isKinematic = true;
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            //Update remote player
            transform.position = Vector3.Lerp(positionAtLastPacket, realPosition, (float)(currentTime / timeToReachGoal));

        }
    }

    public void TakeDamage(int Damage)
    {
        References.accountRefer.CurrentHealth -= Damage;

        if (photonView.IsMine)
        {
            PlayerCameraInstance.GetComponent<Player_Camera>().StartShakeScreen(2, 1, 1);
        }

        if (References.accountRefer.CurrentHealth <= 0) References.accountRefer.CurrentHealth = 0;
        
        LoadPlayerHealthUI();
        Game_Manager.Instance.ReloadPlayerProperties();
        
        if (References.accountRefer.CurrentHealth <= 0)
        {
            Debug.Log("Die");
            Destroy(PlayerAllUIInstance);
            Destroy(PlayerCameraInstance);
            Game_Manager.Instance.DestroyPlayer();
        }
    }


    [PunRPC]
    public void FindClostestEnemy(int Range)
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");


        foreach (GameObject currentEnemy in allEnemy)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy && Vector2.Distance(currentEnemy.transform.position, transform.position) <= Range)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        Enemy = closestEnemy;
    }



    public void Walk()
    {
        Movement = new Vector3(MoveDirection.x, MoveDirection.y, 0f);
        transform.Translate(Movement * (References.accountRefer.Speed + SpeedBonus) * Time.fixedDeltaTime);

        if (Movement.x > 0 && !FacingRight)
        {
            Flip();
        }
        else if (Movement.x < 0 && FacingRight)
        {
            Flip();
        }
    }
    public void Flip()
    {
        FacingRight = !FacingRight;
        if (FacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            PlayerHealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            PlayerHealthChakraUI.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
        
    }

    public void FlipToMouse()
    {
        if (photonView.IsMine)
        {
            if (targetPosition.x > MainPoint.position.x && !FacingRight)
            {
                Flip();
            }
            else if (targetPosition.x < MainPoint.position.x && FacingRight)
            {
                Flip();
            }
        }
    }


    [PunRPC]
    public void TriggerAnimator(string TriggerName)
    {
        if (animator != null)
        {
            animator.SetTrigger(TriggerName);
        }
    }

    public void CallSyncAnimation(string TriggerName)
    {
        photonView.RPC(nameof(TriggerAnimator), RpcTarget.AllBuffered, TriggerName);
    }

    public void Animation_SetUpWalking(bool value)
    {
        if (photonView.IsMine)
        {
            CanWalking = value;
        }
    }

    #region Attack && Skill CanExecute
    public bool CanExecuteNormalAttack(float CurrentCooldown)
    {
        if (CurrentCooldown <= 0 && References.accountWeapon != null && photonView.IsMine)
        {
            return true;
        }

        return false;
    }

    public bool CanExecuteSkill(float CurrentCooldown, int Chakra)
    {
        if (CurrentCooldown <= 0 && References.accountRefer.CurrentCharka >= Chakra && photonView.IsMine)
        {
            return true;
        }

        return false;
    }
    #endregion

    #region Attack && Skill Resources
    public void Attack_Resources()
    {
        AttackCooldown_Current = AttackCooldown_Total;
    }

    public void SkillOne_Resources()
    {
        SkillOneCooldown_Current = SkillOneCooldown_Total;
        References.accountRefer.CurrentCharka -= SkillOne_Entity.Chakra;
        LoadPlayerChakraUI();
    }

    public void SkillTwo_Resources()
    {
        SkillTwoCooldown_Current = SkillTwoCooldown_Total;
        References.accountRefer.CurrentCharka -= SkillTwo_Entity.Chakra;
        LoadPlayerChakraUI();
    }

    public void SkillThree_Resources()
    {
        SkillThreeCooldown_Current = SkillThreeCooldown_Total;
        References.accountRefer.CurrentCharka -= SkillThree_Entity.Chakra;
        LoadPlayerChakraUI();
    }
    #endregion

    #region Attack && Skill Cooldown

    public void Attack()
    {
        if (References.accountWeapon != null)
        {
            if (AttackCooldown_Current > 0)
            {
                AttackCooldown_Current -= Time.deltaTime;
            }
        }

    }
    public void SkillOne()
    {
        if (SkillOne_Entity != null)
        {
            if (SkillOneCooldown_Current > 0)
            {
                SkillOneCooldown_Current -= Time.deltaTime;
            }
        }
    }
    public void SkillTwo()
    {
        if (SkillTwo_Entity != null)
        {
            if (SkillTwoCooldown_Current > 0)
            {
                SkillTwoCooldown_Current -= Time.deltaTime;
            }
        }
    }
    public void SkillThree()
    {
        if (SkillThree_Entity != null)
        {
            if (SkillThreeCooldown_Current > 0)
            {
                SkillThreeCooldown_Current -= Time.deltaTime;
            }
        }
    }

    #endregion

    #region Weapon Load

    public void LoadAccountWeapon()
    {
        if (!WeaponName.IsNullOrEmpty())
        {
            References.accountWeapon = AccountWeapon_DAO.GetAccountWeaponByID(References.accountRefer.ID);
        }

    }
    public void SetUpAccountWeaponName(string Weapon)
    {
        WeaponName = Weapon;
    }

    #endregion

    #region Skill Load

    public void LoadAccountSkill()
    {
        if (!SkillOneName.IsNullOrEmpty())
        {
            SkillOne_Entity = AccountSkill_DAO.GetAccountSkillByID(References.accountRefer.ID, SkillOneName);
        }
        if (!SkillTwoName.IsNullOrEmpty())
        {
            SkillTwo_Entity = AccountSkill_DAO.GetAccountSkillByID(References.accountRefer.ID, SkillTwoName);
        }
        if (!SkillThreeName.IsNullOrEmpty())
        {
            SkillThree_Entity = AccountSkill_DAO.GetAccountSkillByID(References.accountRefer.ID, SkillThreeName);
        }
    }

    public void SetUpAccountSkillName(string SkillOne, string SkillTwo, string SkillThree)
    {
        SkillOneName = SkillOne;
        SkillTwoName = SkillTwo;
        SkillThreeName = SkillThree;
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(MoveDirection);
            stream.SendNext(PlayerHealthChakraUI.GetComponent<RectTransform>().localScale);

            stream.SendNext(targetPosition);
            stream.SendNext(SkillDirection);


            stream.SendNext(References.accountRefer.CurrentHealth);
            stream.SendNext(References.accountRefer.CurrentCharka);
            stream.SendNext(References.accountRefer.Health);
            stream.SendNext(References.accountRefer.Charka);


        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            transform.localScale = (Vector3)stream.ReceiveNext();
            MoveDirection = (Vector2)stream.ReceiveNext();
            PlayerHealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();

            targetPosition = (Vector3)stream.ReceiveNext();
            SkillDirection = (Vector2)stream.ReceiveNext();


            References.accountRefer.CurrentHealth = (int)stream.ReceiveNext();
            References.accountRefer.CurrentCharka = (int)stream.ReceiveNext();
            References.accountRefer.Health = (int)stream.ReceiveNext();
            References.accountRefer.Charka = (int)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;

            LoadPlayerHealthUI();
            LoadPlayerChakraUI();

        }
    }

    public void RegenStrength()
    {
        HealAmountOfStrength(1);
    }

    public void HealAmountOfStrength(int Amount)
    {
        References.accountRefer.CurrentStrength += Amount;
        if (References.accountRefer.CurrentStrength >= References.accountRefer.Strength)
        {
            References.accountRefer.CurrentStrength = References.accountRefer.Strength;
        }
        LoadPlayerStrengthUI();
    }

    public void LoadPlayerStrengthUI()
    {
        if (photonView.IsMine)
        {
            if (PlayerAllUIInstance != null)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
            LoadStrengthUI(References.accountRefer.Strength, References.accountRefer.CurrentStrength);
            }
        }
    }

}
