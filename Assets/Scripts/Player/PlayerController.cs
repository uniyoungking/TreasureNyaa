using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Speed")]
    [Range(1, 50)]
    [SerializeField] private int select_Speed = 30;

    [Header("Weight")]
    [Range(1, 50)]
    [SerializeField] private int select_Weight = 20;

    [Header("Jump Power")]
    [Range(1, 50)]
    [SerializeField] private int select_JumpPower = 30;

    [Header("Endurance")]
    [Range(1, 50)]
    [SerializeField] private int select_Endurance = 30;

    [Header("Lucky")]
    [Range(1, 50)]
    [SerializeField] private int select_Lucky = 20;

    [SerializeField]
    private PlatformGenerator platformGenerator;

    private SpriteRenderer playerSprite = null;
    private PlayerMovement movement;

    [HideInInspector] public float speed;
    [HideInInspector] public float weight;
    [HideInInspector] public float jumpPower;
    [HideInInspector] public float endurance;
    [HideInInspector] public float lucky;

    [HideInInspector] public float fixedSpeed;

    private bool isJumping = false;

    [HideInInspector] public bool isDead = false;

    

    private bool isTouching_FireFairy = false;

    



    public event Action<float> onChangedPlayerHp;

    private float playerHp = 200f;
    public float PlayerHp
    {
        get => playerHp;
        set
        {
            playerHp = value;
            onChangedPlayerHp?.Invoke(playerHp);
        }
    }


    private void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }


    private void Start()
    {
        SetPlayerStat();
    }

    private void Update()
    {
        if (playerHp > 200)
            playerHp = 200;

        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(speed);
            Debug.Log(weight);
            Debug.Log(jumpPower);
            Debug.Log(endurance);
            Debug.Log(lucky);
        }

        if (speed < fixedSpeed)
        {
            speed += 0.005f;
        }

        if (speed > fixedSpeed)
        {
            speed -= 0.005f;
        }
    }

    private void SetPlayerStat()
    {
        speed = (float)select_Speed / 10;
        weight = (float)select_Weight / 10;
        jumpPower = (float)select_JumpPower / 10;
        endurance = (float)select_Endurance / 10;
        lucky = (float)select_Lucky / 10;

        fixedSpeed = speed;
    }

    public void GameOver()
    {
        isDead = true;
        GameManager.instance.GameOver();
    }

    public void SpeedControl(float speedForce)
    {
        speed += speedForce;
    }

    public PlayerMovement GetPlayerMovement()
    {
        return movement;
    }

    public SpriteRenderer GetPlayerSprite()
    {
        return playerSprite;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            GameOver();
        }

        if (collision.CompareTag("Item"))
        {
            if (collision.GetComponent<Item>().MyItemType.Equals(ItemType.FireFairy))
            {
                if (!isTouching_FireFairy)
                {
                    StartCoroutine(Contact_FireFairy());
                }
            }
            else if (collision.GetComponent<Item>().MyItemType.Equals(ItemType.TreeFairy) ||
                collision.GetComponent<Item>().MyItemType.Equals(ItemType.TreeFairy2))
            {
                collision.GetComponent<Item>().EnqueuePooler();
                movement.Jump(3.7f);
                PlayerHp += 5f;
            }
        }
    }


    private IEnumerator Contact_FireFairy()
    {
        isTouching_FireFairy = true;

        playerSprite.color = new Color(1, 1, 1, 0.5f);
        PlayerHp -= 5f;

        movement.AddForceRandom_X(3f);
        movement.DisableCanMove();

        yield return new WaitForSeconds(1f);

        movement.EnableCanMove();

        yield return new WaitForSeconds(1f);

        playerSprite.color = new Color(1, 1, 1, 1);

        isTouching_FireFairy = false;
    }

    public void UseItem(int itemCode)
    {
        switch (itemCode)
        {
            // 네잎클로버
            case 0:
                lucky += 0.5f;
                PlayerHp += (float)UnityEngine.Random.Range(3, 34);
                break;

            // 덤벨
            case 1:
                weight += (float)UnityEngine.Random.Range(0.1f, 0.3f);
                break;

            //깃털
            case 2:
                weight -= 0.1f;

                if (UnityEngine.Random.Range(0, 5) == 0)
                    endurance += 0.1f;
                break;

            //번개
            case 3:
                fixedSpeed += 0.2f;
                speed += 0.2f;

                if (UnityEngine.Random.Range(0, 10) == 0)
                    endurance -= 0.1f;
                break;

            //근육하트
            case 4:
                endurance += 0.1f;
                break;

            //날개
            case 5:
                jumpPower += 0.2f;
                break;

            //시계
            case 6:
                float recovery = PlayerHp / 10;
                PlayerHp += recovery;
                break;
        }
    }

    private void RemoveBottomPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, Mathf.Infinity, 1 << LayerMask.NameToLayer("Platform"));

        PlatformBase platform = hit.transform.GetComponent<PlatformBase>();
        platform.ReturnPlatform();
        RemoveMonster(platform);
    }


    private void RemoveNearestMonster()
    {

    }


    private void RemoveAllBottomPlatform()
    {
        // 활성화된애들중에 y축 값 아래
        List<GameObject> platforms = platformGenerator.GetActivatedObjects();

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i].transform.position.y < transform.position.y)
            {
                PlatformBase platform = platforms[i].GetComponent<PlatformBase>();

                RemoveMonster(platform);
                platform.ReturnPlatform();
            }
        }
    }

    // 발판에 가까운 몬스터 삭제
    private void RemoveMonster(PlatformBase platform)
    {
        Vector2 point = new Vector2(platform.transform.position.x, platform.transform.position.y);

        Collider2D coll = Physics2D.OverlapCircle(point, 1f, 1 << LayerMask.NameToLayer("Item"));

        if (coll)
            coll.GetComponent<Item>().EnqueuePooler();
    }
}
