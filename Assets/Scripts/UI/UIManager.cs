using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private PlayerController player;

    private int currentWindow = 0; // 0:Home, 1:Pulse, 2:Inventoy, 3:Pulse2, 4:Inventory2

    private float pulse = 0f;
    private int coin = 0;

    private float pulse_Count = 0f;
    private int item_Count = 0;
    private bool enableGetItem = true;

    [SerializeField] private GameObject window_Home;
    [SerializeField] private GameObject window_Pulse;
    [SerializeField] private GameObject window_Inventory;
    [SerializeField] private GameObject window_Pulse2;
    [SerializeField] private GameObject window_Inventory2;
    [SerializeField] private GameObject back;

    private GameObject window_Current;

    [SerializeField] private TextMeshProUGUI stats_Speed;
    [SerializeField] private TextMeshProUGUI stats_Weight;
    [SerializeField] private TextMeshProUGUI stats_JumpPower;
    [SerializeField] private TextMeshProUGUI stats_Endurance;
    [SerializeField] private TextMeshProUGUI stats_Lucky;

    [SerializeField] private GameObject[] button_Levels;
    [SerializeField] private GameObject button_Restart;

    public Sprite[] sprites_Item;
    [SerializeField] private Sprite sprite_Pulse;
    [SerializeField] private Sprite sprite_Inventory;
    [SerializeField] private Sprite sprite_Pulse2;
    [SerializeField] private Sprite sprite_Inventory2;
    [SerializeField] private Sprite sprite_Back;
    [SerializeField] private Sprite sprite_Back2;
    [SerializeField] private Image image_Pulse;
    [SerializeField] private Image image_Inventory;
    [SerializeField] private Image image_Back;

    private bool active_Minigame = false; // state to enable start minigame
    private bool ing_Minigame = false; // state to playing minigame
    private float cooltime_Next_Minigame = 0;
    private float count_Until_MinigameStart = 0;
    private int minigameCode; // 0:Pulse, 1:Inventory

    // Pulse
    [SerializeField] private TextMeshProUGUI text_Hp;
    [SerializeField] private TextMeshProUGUI text_Stack;
    [SerializeField] private Image image_Charge;
    [SerializeField] private Sprite[] sprites_Charge;
    [SerializeField] private GameObject[] stacks;
    [SerializeField] private Animator animator;

    // Minigame_Pulse
    [SerializeField] private Image image_Minigame_Pulse;
    [SerializeField] private Sprite sprite_Minigame_Pulse_Green;
    [SerializeField] private Sprite sprite_Minigame_Pulse_Red;
    [SerializeField] private Image image_Minigame_Pulse_Red;
    [SerializeField] private Sprite[] sprites_Minigame_Pulse_Red_Count;
    private bool isTouch_Minigame_Pulse = false;
    private bool isSuccess_Minigame_Pulse = true;

    // Minigame_Inventory
    [SerializeField] private GameObject button_Hacking;
    [SerializeField] private Image image_Minigame_Inventory_Hacking;
    [SerializeField] private Sprite[] sprites_Minigame_Inventory_Count;
    private List<int> exist_button;
    private bool isSuccess_Minigame_Inventory = true;
    [HideInInspector] public bool isActive_Minigame_Inventory = false;
    private bool isTouch_Minigame_Inventory = false;
    private int randomTouch = 0;
    private int countTouch = 0;
    private bool isLimit = false;
    private RectTransform rectTransform_Hacking;

    // Minigame_Inventory2
    [SerializeField] private Image image_Answer;
    [SerializeField] private Image[] images_Example;
    [SerializeField] private Sprite[] sprites_Minigame_Inventory2;
    private int[] exampleCodes = new int[3];
    private int answerCode;
    private bool isSuccess_Minigame_Inventory2 = true;

    private bool switch_Blink = false;

    // above : play ui field

    [SerializeField] private GameObject go_UI_Play;
    [SerializeField] private GameObject go_UI_Main;

    // below : main ui field

    [SerializeField] private GameObject go_screenA;
    [SerializeField] private GameObject go_screenB;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        window_Current = window_Home;
        exist_button = new List<int>();
        rectTransform_Hacking = button_Hacking.GetComponent<RectTransform>();

        if (CameraSet.moreHeightLong)
            gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
    }

    void Update()
    {
        if (item_Count >= 4)
            enableGetItem = false;

        switch (currentWindow)
        {
            case 0:
                break;

            case 1:
                Pulse();
                break;

            case 2:        
                break;

            default:
                break;
        }

        stats_Speed.text = "S: " + player.speed.ToString("F1");
        stats_Weight.text = "W: " + player.weight.ToString("F1");
        stats_JumpPower.text = "J: " + player.jumpPower.ToString("F1");
        stats_Endurance.text = "E: " + player.endurance.ToString("F1");
        stats_Lucky.text = "L: " + player.lucky.ToString("F1");

        if (player.isDead)
            button_Restart.SetActive(true);

        // -------Mini game-------
        if ((!active_Minigame) && (!ing_Minigame))
            cooltime_Next_Minigame += Time.deltaTime;
        else if (active_Minigame && (!ing_Minigame))
            count_Until_MinigameStart += Time.deltaTime;

        if (cooltime_Next_Minigame > 10)
        {
            active_Minigame = true;
            minigameCode = Enable_Minigame();
            cooltime_Next_Minigame = 0;
        }

        if (count_Until_MinigameStart > 7)
        {
            active_Minigame = false;
            Fail_Minigame(minigameCode);
            count_Until_MinigameStart = 0;
        }

        if (randomTouch > 0 && (!isLimit))
        {
            if (isTouch_Minigame_Inventory)
            {
                isTouch_Minigame_Inventory = false;
                countTouch++;
            }

            if (randomTouch == countTouch)
                isSuccess_Minigame_Inventory = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
            Debug.Log(cooltime_Next_Minigame);
    }

    private void Pulse()
    {
        text_Hp.text = "" + (int)player.PlayerHp;

        pulse_Count += Time.deltaTime;

        if (pulse_Count > 0.5)
        {
            pulse += 1f;
            text_Stack.text = "" + pulse;
            pulse_Count = 0;
        }

        if (pulse == 1)
            stacks[0].SetActive(true);
        if (pulse == 2)
            stacks[1].SetActive(true);
        if (pulse == 3)
            stacks[2].SetActive(true);
        if (pulse == 4)
            stacks[3].SetActive(true);
        if (pulse == 5)
            stacks[4].SetActive(true);

        if (pulse == 6)
        {
            player.PlayerHp += 1f;
            pulse = 0f;

            for (int i = 0; i < 5; i++)
                stacks[i].SetActive(false);

            animator.SetTrigger("Charge");

            text_Stack.text = "" + pulse;
        }
    }

    public void Click_Pulse()
    {
        pulse = 0f;
        text_Stack.text = "" + pulse;

        window_Home.SetActive(false);

        if (active_Minigame && minigameCode == 0)
        {
            image_Minigame_Pulse.sprite = sprite_Minigame_Pulse_Red;
            image_Minigame_Pulse_Red.gameObject.SetActive(true);
            window_Pulse2.SetActive(true);
            window_Current = window_Pulse2;
            currentWindow = 3;

            active_Minigame = false;
            ing_Minigame = true;
            cooltime_Next_Minigame = 0;
            count_Until_MinigameStart = 0;
            StartCoroutine(Minigame_Pulse());
        }
        else
        {  
            window_Pulse.SetActive(true);
            window_Current = window_Pulse;
            currentWindow = 1;
        }

        back.SetActive(true);
    }

    public void Click_Inventory()
    {
        window_Home.SetActive(false);
        button_Hacking.SetActive(false);

        if (active_Minigame && minigameCode == 1)
        {
            active_Minigame = false;
            ing_Minigame = true;
            cooltime_Next_Minigame = 0;
            count_Until_MinigameStart = 0;

            int random = Random.Range(1, 2);

            if (random == 0)
            {
                StartCoroutine(Minigame_Inventory());
                window_Inventory.SetActive(true);
                window_Current = window_Inventory;
                currentWindow = 2;
            }
            else
            {
                Minigame_Inventory2();
                window_Inventory2.SetActive(true);
                window_Current = window_Inventory2;
                currentWindow = 4;
            }
        }
        else
        {
            window_Inventory.SetActive(true);
            window_Current = window_Inventory;
            currentWindow = 2;
        }

        back.SetActive(true);
    }

    public void Click_Back()
    {
        image_Back.sprite = sprite_Back;
        switch_Blink = false;

        window_Current.SetActive(false);
        window_Home.SetActive(true);
        back.SetActive(false);
        currentWindow = 0;

        for (int i = 0; i < 5; i++)
            stacks[i].SetActive(false);

        if (!isSuccess_Minigame_Pulse)
        {
            Debug.Log("Fail minigame pulse");
            Fail_Minigame(0);
        }

        if (!isSuccess_Minigame_Inventory)
        {
            Debug.Log("Fail minigame inventory");
            Fail_Minigame(1);
        }

        if (!isSuccess_Minigame_Inventory2)
        {
            Debug.Log("Fail minigame inventory2");
            Fail_Minigame(2);
        }
    }

    public void Click_RestartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Click_Level(int level)
    {
        GameManager.instance.GameStart(level);

        for (int i = 0; i < 3; i++)
            button_Levels[i].SetActive(false);
    }

    public void Click_Setting()
    {
        bool isPause = GameManager.instance.IsPause;

        if (!isPause)
            TimePause();
        else
            TimeResume();
    }

    public void TimePause()
    {
        Time.timeScale = 0f;
        GameManager.instance.IsPause = true;
    }

    public void TimeResume()
    {
        Time.timeScale = 1f;
        GameManager.instance.IsPause = false;
    }

    public void Click_InventoryButton(int buttonCode)
    {
        int itemCode = InventoryManager.instance.inventory[buttonCode];

        if (itemCode != -1)
        {
            if (InventoryManager.instance.isWrap[buttonCode])
            {
                InventoryManager.instance.isWrap[buttonCode] = false;
                InventoryManager.instance.inventoryButtons[buttonCode].GetComponent<Image>().sprite = sprites_Item[itemCode];
            }
            else
            {
                InventoryManager.instance.isWrap[buttonCode] = true;
                Debug.Log("Item Code : " + itemCode);
                player.UseItem(itemCode);
                InventoryManager.instance.RemoveItem(buttonCode);
            }
        }
    }

    public void Click_Minigame_Pulse_Touch()
    {
        isTouch_Minigame_Pulse = true;
    }
    public void Click_Minigame_Inventory_Touch()
    {
        isTouch_Minigame_Inventory = true;
    }

    public void Click_Minigame_Inventory2_Touch(int buttonCode)
    {
        if (answerCode == exampleCodes[buttonCode])
        {
            isSuccess_Minigame_Inventory2 = true;
            ing_Minigame = false;
            isActive_Minigame_Inventory = false;
            Click_Back();
            Debug.Log("Success");
        }
        else
        {
            ing_Minigame = false;
            isActive_Minigame_Inventory = false;
            Click_Back();
            Debug.Log("Fail");
        }
    }

    private int Enable_Minigame()
    {
        if (currentWindow != 0)
        {
            image_Back.sprite = sprite_Back2;
            switch_Blink = true;
            StartCoroutine(Blink_Button_Back());
        }

        int random = Random.Range(1, 2); // if you want to certain minigame test, change here(minigame code)

        if (random == 0)
        {
            Enable_Minigame_Pulse();
            Debug.Log("Enable minigame pulse");
        }
        else
        {
            if (InventoryManager.instance.inventory[0] == -1 && InventoryManager.instance.inventory[1] == -1 && InventoryManager.instance.inventory[2] == -1
            && InventoryManager.instance.inventory[3] == -1)
            {
                Enable_Minigame_Pulse();
                Debug.Log("Enable minigame pulse");
                random = 0;
            }
            else
            {
                Enable_Minigame_Inventory();
                Debug.Log("Enable minigame inventory");
            }
        }

        return random;
    }

    private void Fail_Minigame(int minigameCode)
    {
        player.PlayerHp -= 20f;
        image_Back.sprite = sprite_Back;
        switch_Blink = false;

        if (minigameCode == 0)
        {
            image_Pulse.sprite = sprite_Pulse;
            player.speed = 3f;
            player.weight = 2f;
            player.jumpPower = 3f;
            player.endurance = 3f;
            player.lucky = 2f;
            player.fixedSpeed = 3f;

            isSuccess_Minigame_Pulse = true;
        }
        else if (minigameCode == 1)
        {;
            InventoryManager.instance.RemoveItem(3);
            InventoryManager.instance.RemoveItem(2);
            InventoryManager.instance.RemoveItem(1);
            InventoryManager.instance.RemoveItem(0);

            isLimit = false;
            isSuccess_Minigame_Inventory = true;
        }
        else if (minigameCode == 2)
        {
            InventoryManager.instance.RemoveItem(3);
            InventoryManager.instance.RemoveItem(2);
            InventoryManager.instance.RemoveItem(1);
            InventoryManager.instance.RemoveItem(0);

            isSuccess_Minigame_Inventory2 = true;
        }
    }

    private void Enable_Minigame_Pulse()
    {
        image_Pulse.sprite = sprite_Pulse2;
    }

    private void Enable_Minigame_Inventory()
    {
        isActive_Minigame_Inventory = true;
        image_Inventory.sprite = sprite_Inventory2;
    }

    private IEnumerator Minigame_Pulse()
    {
        image_Pulse.sprite = sprite_Pulse;
        isSuccess_Minigame_Pulse = false;

        int random_Count = Random.Range(5, 8);
        float random_Sec = Random.Range(0.6f, 1.3f);

        image_Minigame_Pulse_Red.sprite = sprites_Minigame_Pulse_Red_Count[random_Count];

        while (random_Count > 0)
        {
            if (isTouch_Minigame_Pulse) // Fail be caused by wrong touch
            {
                isTouch_Minigame_Pulse = false;
                ing_Minigame = false;
                Click_Back();
            }

            yield return new WaitForSeconds(random_Sec);

            if (currentWindow == 0) // if click back button
            {
                ing_Minigame = false;
                yield break;
            }

            random_Count--;

            image_Minigame_Pulse_Red.sprite = sprites_Minigame_Pulse_Red_Count[random_Count];
        }

        image_Minigame_Pulse_Red.gameObject.SetActive(false);
        image_Minigame_Pulse.sprite = sprite_Minigame_Pulse_Green;

        yield return new WaitForSeconds(random_Sec);

        if (currentWindow == 0) // if click back button
        {
            ing_Minigame = false;
            yield break;
        }

        if (isTouch_Minigame_Pulse) // success minigame
        {
            isTouch_Minigame_Pulse = false;
            ing_Minigame = false;
            Debug.Log("Success minigame pulse");

            window_Home.SetActive(true);
            window_Pulse2.SetActive(false);
            currentWindow = 0;
            yield break;
        }

        ing_Minigame = false; // Fail be caused by no any touch
        Click_Back();
    }

    private IEnumerator Minigame_Inventory()
    {
        image_Inventory.sprite = sprite_Inventory;
        isSuccess_Minigame_Inventory = false;

        exist_button.Clear();

        for (int i = 0; i < 4; i++)
        {
            if (InventoryManager.instance.inventory[i] != -1)
                exist_button.Add(i);
        }

        int random = Random.Range(0, exist_button.Count);

        switch (exist_button[random])
        {
            case 0:
                rectTransform_Hacking.anchoredPosition = new Vector2(-159f, 11f);
                break;

            case 1:
                rectTransform_Hacking.anchoredPosition = new Vector2(-34f, 11f);
                break;

            case 2:
                rectTransform_Hacking.anchoredPosition = new Vector2(91f, 11f);
                break;

            case 3:
                rectTransform_Hacking.anchoredPosition = new Vector2(216f, 11f);
                break;

            default:
                break;
        }

        button_Hacking.SetActive(true);

        randomTouch = Random.Range(5, 8);
        countTouch = 0;

        image_Minigame_Inventory_Hacking.sprite = sprites_Minigame_Inventory_Count[randomTouch];

        yield return new WaitForSeconds(randomTouch);

        if (!isSuccess_Minigame_Inventory)
        {
            isLimit = true;
            Debug.Log("Fail");
        }
        else
        {
            if (countTouch > randomTouch)
            {
                isLimit = true;
                isSuccess_Minigame_Inventory = false;
                Debug.Log("Fail");
            }
            else
                Debug.Log("Success");
        }

        //if (currentWindow == 0) // if click back button
        //{
        //    ing_Minigame = false;
        //    exist_button.Clear();
        //    yield break;
        //}

        randomTouch = 0;
        countTouch = 0;
        ing_Minigame = false;
        isActive_Minigame_Inventory = false;
        Click_Back();
    }

    private void Minigame_Inventory2()
    {
        image_Inventory.sprite = sprite_Inventory;
        isSuccess_Minigame_Inventory2 = false;

        int random = Random.Range(0, 9); // Examples
        images_Example[0].sprite = sprites_Minigame_Inventory2[random];
        exampleCodes[0] = random;

        random = Random.Range(0, 9);
        while (random == exampleCodes[0])
            random = Random.Range(0, 9);
        images_Example[1].sprite = sprites_Minigame_Inventory2[random];
        exampleCodes[1] = random;

        random = Random.Range(0, 9);
        while (random == exampleCodes[0] || random == exampleCodes[1])
            random = Random.Range(0, 9);
        images_Example[2].sprite = sprites_Minigame_Inventory2[random];
        exampleCodes[2] = random;

        random = Random.Range(0, 3); // Answer
        if (random == 0)
        {
            image_Answer.sprite = images_Example[0].sprite;
            answerCode = exampleCodes[0];
        }
        else if (random == 1)
        {
            image_Answer.sprite = images_Example[1].sprite;
            answerCode = exampleCodes[1];
        }
        else if (random == 2)
        {
            image_Answer.sprite = images_Example[2].sprite;
            answerCode = exampleCodes[2];
        } 
    }

    private IEnumerator Blink_Button_Back()
    {
        while (switch_Blink)
        {
            image_Back.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.5f);
            image_Back.color = new Color32(255, 255, 255, 0);
            yield return new WaitForSeconds(0.5f);
        }

        if (!switch_Blink)
            image_Back.color = new Color32(255, 255, 255, 255);
    }

    // above : play ui method
    // below : main ui method

    public void Click_ScreenA_GameStart()
    {
        go_screenA.SetActive(false);
        go_screenB.SetActive(true);
    }

    public void Click_ScreenB_GameStart()
    {
        go_UI_Main.SetActive(false);
        go_UI_Play.SetActive(true);
    }
}
