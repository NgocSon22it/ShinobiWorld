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

public class PlayerBase : MonoBehaviourPunCallbacks, IPunObservable
{

    [Header("Player Entity")]
    public Account_Entity AccountEntity = new Account_Entity();
    public AccountWeapon_Entity AccountWeapon_Entity = new AccountWeapon_Entity();

    public AccountSkill_Entity SkillOne_Entity = new AccountSkill_Entity();
    public AccountSkill_Entity SkillTwo_Entity = new AccountSkill_Entity();
    public AccountSkill_Entity SkillThree_Entity = new AccountSkill_Entity();

    public string WeaponName, SkillOneName, SkillTwoName, SkillThreeName;

    [Header("Player Instance")]
    [SerializeField] GameObject PlayerControlPrefabs;
    [SerializeField] GameObject PlayerCameraPrefabs;
    [SerializeField] GameObject PlayerAllUIPrefabs;

    [SerializeField] TMP_Text PlayerNickName;
    [SerializeField] GameObject PlayerHealthChakraUI;

    public GameObject PlayerControlInstance;
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

    //Take Damage
    private bool Hurting;

    //Enemy
    protected GameObject Enemy;

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
        if(AccountEntity != null)       
        {        
            string image = References.listEye.Find(obj => obj.ID == AccountEntity.EyeID).Image;
            Eye.sprite = Resources.Load<Sprite>(image);

            //Hair
            image = References.listHair.Find(obj => obj.ID == AccountEntity.HairID).Image;
            Hair.sprite = Resources.Load<Sprite>(image);

            //Mouth
            image = References.listMouth.Find(obj => obj.ID == AccountEntity.MouthID).Image;
            Mouth.sprite = Resources.Load<Sprite>(image);

            //Skin
            image = References.listSkin.Find(obj => obj.ID == AccountEntity.SkinID).Image;

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
            photonView.RPC(nameof(SetUpAccount), RpcTarget.AllBuffered);

            if (AccountEntity != null)
            {             

                PlayerControlInstance = Instantiate(PlayerControlPrefabs);
                PlayerCameraInstance = Instantiate(PlayerCameraPrefabs);
                PlayerAllUIInstance = Instantiate(PlayerAllUIPrefabs);

                PlayerCameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;

                PlayerControlInstance.GetComponent<Player_ButtonManagement>().SetUpPlayer(this.gameObject);

                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadExperienceUI(AccountEntity.Level, AccountEntity.Exp, AccountEntity.Level * 100);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadNameUI(photonView.Owner.NickName);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpCoinUI(AccountEntity.Coin);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadStrengthUI(AccountEntity.Strength, AccountEntity.CurrentStrength);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadPowerUI(Account_DAO.GetAccountPowerByID(AccountEntity.ID));

                player_LevelManagement.GetComponent<Player_LevelManagement>().SetUpAccountEntity(AccountEntity);               
                PlayerHealthChakraUI.SetActive(false);

                InvokeRepeating(nameof(RegenHealth), 1f, 1f);
                InvokeRepeating(nameof(RegenChakra), 1f, 1f);
            }
        }
        else
        {
            PlayerHealthChakraUI.SetActive(true);
            
        }
        LoadLayout();
        PlayerNickName.text = photonView.Owner.NickName;
        LoadPlayerHealthUI();
        LoadPlayerChakraUI();
    }

    public void RegenHealth()
    {
        HealAmountOfHealth(1);
    }

    public void RegenChakra()
    {
        HealAmountOfChakra(1);
    }

    [PunRPC]
    public void SetUpAccount()
    {
        AccountEntity = References.accountRefer;
    }


    public void HealAmountOfHealth(int Amount)
    {
        AccountEntity.CurrentHealth += Amount;
        if (AccountEntity.CurrentHealth >= AccountEntity.Health)
        {
            AccountEntity.CurrentHealth = AccountEntity.Health;
        }
        LoadPlayerHealthUI();
    }


    public void HealAmountOfChakra(int Amount)
    {
        AccountEntity.CurrentCharka += Amount;
        if (AccountEntity.CurrentCharka >= AccountEntity.Charka)
        {
            AccountEntity.CurrentCharka = AccountEntity.Charka;
        }
        LoadPlayerChakraUI();
    }

    public void EarnAmountOfExperience(int Amount)
    {
        if (photonView.IsMine)
        {
            player_LevelManagement.AddExperience(Amount);
            PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().LoadExperienceUI(AccountEntity.Level, AccountEntity.Exp, AccountEntity.Level * 100);
        }
    }


    public void LoadPlayerChakraUI()
    {
        if (photonView.IsMine)
        {
            if (PlayerAllUIInstance != null)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
                LoadChakraUI((float)AccountEntity.Charka, (float)AccountEntity.CurrentCharka);
            }
        }
        else
        {
            CurrentChakra_UI.fillAmount = (float)AccountEntity.CurrentCharka / (float)AccountEntity.Charka;
            CurrentChakra_NumberUI.text = AccountEntity.CurrentCharka + " / " + AccountEntity.Charka;
        }
    }

    public void LoadPlayerHealthUI()
    {
        if (photonView.IsMine)
        {
            if (PlayerAllUIInstance != null)
            {
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
                LoadHealthUI((float)AccountEntity.Health, (float)AccountEntity.CurrentHealth);
            }
        }
        else
        {
            CurrentHealth_UI.fillAmount = (float)AccountEntity.CurrentHealth / (float)AccountEntity.Health;
            CurrentHealth_NumberUI.text = AccountEntity.CurrentHealth + " / " + AccountEntity.Health;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (CanWalking && photonView.IsMine)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }
    }

    public void Update()
    {       
        if (photonView.IsMine)
        {
            animator.SetFloat("Horizontal", MoveDirection.x);
            animator.SetFloat("Vertical", MoveDirection.y);
            animator.SetFloat("Speed", MoveDirection.sqrMagnitude);

            if (!CanWalking)
            {
                MoveDirection = Vector2.zero;
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

    [PunRPC]
    public void TakeDamage(int Damage)
    {
        if (Hurting) { return; }
        AccountEntity.CurrentHealth -= Damage;

        StartCoroutine(DamageAnimation());

        if (photonView.IsMine)
        {
            PlayerCameraInstance.GetComponent<Player_Camera>().StartShakeScreen(2, 2, 1);
        }
        LoadPlayerHealthUI();
        if (AccountEntity.CurrentHealth <= 0)
        {
            Debug.Log("Die");
        }
    }

    public IEnumerator DamageAnimation()
    {
        Hurting = true;
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = Color.red;

            yield return new WaitForSeconds(.1f);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.1f);
        }
        Hurting = false;
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
        transform.Translate(Movement * AccountEntity.Speed * Time.fixedDeltaTime);

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


    public void FlipToEnemy()
    {
        if (Enemy != null)
        {
            if (Enemy.transform.position.x > AttackPoint.position.x && !FacingRight)
            {
                Flip();
            }
            else if (Enemy.transform.position.x < AttackPoint.position.x && FacingRight)
            {
                Flip();
            }
        }
    }


    [PunRPC]
    public void TriggerAnimator(string TriggerName)
    {
        animator.SetTrigger(TriggerName);
    }

    public void CallSyncAnimation(string TriggerName)
    {
        photonView.RPC(nameof(TriggerAnimator), RpcTarget.AllBuffered, TriggerName);
    }

    public void Animation_SetUpWalking(bool value)
    {
        CanWalking = value;
    }

    public bool CanExecuteSkill(float CurrentCooldown, int Chakra)
    {
        if (CurrentCooldown <= 0 && AccountEntity.CurrentCharka >= Chakra && photonView.IsMine)
        {
            return true;
        }

        return false;
    }

    public void SkillOne_Resources()
    {
        SkillOneCooldown_Current = SkillOneCooldown_Total;
        AccountEntity.CurrentCharka -= SkillOne_Entity.Chakra;
        LoadPlayerChakraUI();


    }

    public void SkillTwo_Resources()
    {
        SkillTwoCooldown_Current = SkillTwoCooldown_Total;
        AccountEntity.CurrentCharka -= SkillTwo_Entity.Chakra;
        LoadPlayerChakraUI();


    }

    public void SkillThree_Resources()
    {
        SkillThreeCooldown_Current = SkillThreeCooldown_Total;
        AccountEntity.CurrentCharka -= SkillThree_Entity.Chakra;
        LoadPlayerChakraUI();
    }

    public void LoadAccountSkill()
    {
        SkillOne_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, SkillOneName);
        SkillTwo_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, SkillTwoName);
        SkillThree_Entity = AccountSkill_DAO.GetAccountSkillByID(AccountEntity.ID, SkillThreeName);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(MoveDirection);
            stream.SendNext(PlayerHealthChakraUI.GetComponent<RectTransform>().localScale);


            stream.SendNext(AccountEntity.CurrentHealth);
            stream.SendNext(AccountEntity.CurrentCharka);
            stream.SendNext(AccountEntity.Health);
            stream.SendNext(AccountEntity.Charka);

        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            MoveDirection = (Vector2)stream.ReceiveNext();
            PlayerHealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();

            AccountEntity.CurrentHealth = (int)stream.ReceiveNext();
            AccountEntity.CurrentCharka = (int)stream.ReceiveNext();
            AccountEntity.Health = (int)stream.ReceiveNext();
            AccountEntity.Charka = (int)stream.ReceiveNext();

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


}
