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

public class PlayerBase : MonoBehaviour, IPunObservable
{

    [Header("Player Entity")]
    public Account_Entity AccountEntity = new Account_Entity();

    public int CurrentHealth, CurrentChakra;

    [Header("Player Instance")]
    [SerializeField] GameObject PlayerControlPrefabs;
    [SerializeField] GameObject PlayerCameraPrefabs;
    [SerializeField] GameObject PlayerAllUIPrefabs;

    [SerializeField] TMP_Text PlayerNickName;
    [SerializeField] GameObject PlayerHealthChakraUI;


    GameObject PlayerControlInstance;
    GameObject PlayerCameraInstance;
    GameObject PlayerAllUIInstance;

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
    public PhotonView PV;
    public Rigidbody2D rigidbody2d;
    public SpriteRenderer spriteRenderer;
    public SortingGroup sortingGroup;
    public PlayerInput playerInput;

    //Script Component
    public Player_Pool playerPool;

    //Player Input
    [SerializeField] Vector2 MoveDirection;
    Vector3 Movement;
    bool FacingRight = true;

    //Health UI
    [SerializeField] Image CurrentHealth_UI;
    [SerializeField] Image CurrentChakra_UI;


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
        PV = GetComponent<PhotonView>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortingGroup = GetComponent<SortingGroup>();
        playerInput = GetComponent<PlayerInput>();


        playerPool = GetComponent<Player_Pool>();
    }

    public void Start()
    {
        AccountEntity = Account_DAO.GetAccountByID(References.UserID);
        

        if (AccountEntity != null)
        {
            SetUpComponent();        

            if (PV.IsMine)
            {
                PlayerControlInstance = Instantiate(PlayerControlPrefabs);
                PlayerCameraInstance = Instantiate(PlayerCameraPrefabs);
                PlayerAllUIInstance = Instantiate(PlayerAllUIPrefabs);


                PlayerCameraInstance.GetComponent<CinemachineVirtualCamera>().m_Follow = gameObject.transform;
                PlayerControlInstance.GetComponent<Player_ButtonManagement>().SetUpPlayer(this.gameObject);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpExperienceUI(AccountEntity.Level, AccountEntity.Exp, AccountEntity.Level * 100);
                PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().SetUpNameUI(PV.Owner.NickName);

                sortingGroup.sortingLayerName = "Me";
                PlayerHealthChakraUI.SetActive(false);
                PlayerHealthChakraUI.GetComponent<Canvas>().sortingLayerName = "Me";
            }
            else
            {
                sortingGroup.sortingLayerName = "Other";
                PlayerHealthChakraUI.GetComponent<Canvas>().sortingLayerName = "Other";
            }

            CurrentHealth = AccountEntity.Health;
            CurrentChakra = AccountEntity.Charka;

            InvokeRepeating(nameof(RegenHealth), 1f, 2f);
            InvokeRepeating(nameof(RegenChakra), 1f, 2f);

            PlayerNickName.text = PV.Owner.NickName;

            LoadPlayerHealthNChakraUI();

        }
    }

    public void RegenHealth()
    {
        if (CurrentHealth < AccountEntity.Health)
        {
            CurrentHealth += 1;
            LoadPlayerHealthNChakraUI();
        }
    }

    public void RegenChakra()
    {
        if (CurrentChakra < AccountEntity.Charka)
        {
            CurrentChakra += 1;
            LoadPlayerHealthNChakraUI();
        }
    }

    public void LoadPlayerHealthNChakraUI()
    {
        CurrentChakra_UI.fillAmount = (float)CurrentChakra / (float)AccountEntity.Charka;
        CurrentHealth_UI.fillAmount = (float)CurrentHealth / (float)AccountEntity.Health;
        PlayerAllUIInstance.GetComponent<Player_AllUIManagement>().
        SetUpHealthNChakraUI((float)CurrentHealth / (float)AccountEntity.Health, (float)CurrentChakra / (float)AccountEntity.Charka);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }

    public void Update()
    {
        animator.SetFloat("Horizontal", MoveDirection.x);
        animator.SetFloat("Vertical", MoveDirection.y);
        animator.SetFloat("Speed", MoveDirection.sqrMagnitude);
    }

    public void FixedUpdate()
    {
        if (PV.IsMine)
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
        if (Hurting) { return; }
        CurrentHealth -= Damage;
        StartCoroutine(DamageAnimation());
        PlayerCameraInstance.GetComponent<Player_Camera>().StartShakeScreen(3, 3, 1);

        if (CurrentHealth <= 0)
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
        PV.RPC(nameof(TriggerAnimator), RpcTarget.AllBuffered, TriggerName);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(MoveDirection);
            stream.SendNext(PlayerHealthChakraUI.GetComponent<RectTransform>().localScale);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            MoveDirection = (Vector2)stream.ReceiveNext();
            PlayerHealthChakraUI.GetComponent<RectTransform>().localScale = (Vector3)stream.ReceiveNext();

            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }

}
