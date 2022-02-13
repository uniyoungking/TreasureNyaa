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

    private float pulse_Count = 0.5f;
    private int item_Count = 0;
    private bool enableGetItem = true;

    [SerializeField] private GameObject window_Home;
    [SerializeField] private GameObject window_Pulse;
    [SerializeField] private GameObject window_Inventory;
    [SerializeField] private GameObject window_Pulse2;
    [SerializeField] private GameObject back;

    private GameObject window_Current;

    [SerializeField] private TextMeshProUGUI text_Life;
    [SerializeField] private TextMeshProUGUI text_Stack;

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

    private bool active_Minigame = false;
    private bool ing_Minigame = false;
    private float count_Minigame = 0;
    private float count_MinigameLimit = 0;
    private int minigameCode; // 0:Pulse, 1:Inventory

    // Minigame_Pulse
    [SerializeField] private TextMeshProUGUI text_Minigame_Pulse;
    [SerializeField] private Image image_Minigame_Pulse;
    private bool isTouch_Minigame_Pulse = false;
    private bool isSuccess_Minigame_Pulse = true;

    // Minigame_Inventory
    [SerializeField] private GameObject button_Hacking;
    [SerializeField] private TextMeshProUGUI text_Hacking;
    private List<int> exist_button;
    private bool isSuccess_Minigame_Inventory = true;
    [HideInInspector] public bool isActive_Minigame_Inventory = false;
    private bool isTouch_Minigame_Inventory = false;
    private int randomTouch = 0;
    private int countTouch = 0;
    private bool isLimit = false;
    private float button_Hacking_Position_X;

    private bool switch_Blink = false;

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
        button_Hacking_Position_X = button_Hacking.transform.position.x;
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
            count_Minigame += Time.deltaTime;
        else if (active_Minigame && (!ing_Minigame))
            count_MinigameLimit += Time.deltaTime;

        if (count_Minigame > 10)
        {
            active_Minigame = true;
            minigameCode = Enable_Minigame();
            count_Minigame = 0;
        }

        if (count_MinigameLimit > 7)
        {
            active_Minigame = false;
            Fail_Minigame(minigameCode);
            count_MinigameLimit = 0;
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
            Debug.Log(count_Minigame);
    }

    private void Pulse()
    {
        text_Life.text = (int)player.PlayerHp + " / 200";

        pulse_Count += Time.deltaTime;

        if (pulse_Count > 0.5)
        {
            pulse += 1f;
            text_Stack.text = "" + pulse;
            pulse_Count = 0;
        }

        if (pulse == 6)
        {
            player.PlayerHp += 1f;
            pulse = 0f;
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
            window_Pulse2.SetActive(true);
            window_Current = window_Pulse2;
            currentWindow = 3;

            active_Minigame = false;
            ing_Minigame = true;
            count_Minigame = 0;
            count_MinigameLimit = 0;
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
            count_Minigame = 0;
            count_MinigameLimit = 0;
            StartCoroutine(Minigame_Inventory());
        }

        window_Inventory.SetActive(true);
        window_Current = window_Inventory;
        currentWindow = 2;

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

        if (!isSuccess_Minigame_Pulse)
            Fail_Minigame(0);

        if (!isSuccess_Minigame_Inventory)
            Fail_Minigame(1);
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
            Debug.Log("Item Code : " + itemCode);
            player.UseItem(itemCode);
        }

        InventoryManager.instance.RemoveItem(buttonCode);
    }

    public void Click_Minigame_Pulse_Touch()
    {
        isTouch_Minigame_Pulse = true;
    }
    public void Click_Minigame_Inventory_Touch()
    {
        isTouch_Minigame_Inventory = true;
    }

    private int Enable_Minigame()
    {
        if (currentWindow != 0)
        {
            image_Back.sprite = sprite_Back2;
            switch_Blink = true;
            StartCoroutine(Blink_Button_Back());
        }

        int random = Random.Range(0, 2);

        if (random == 0)
            Enable_Minigame_Pulse();
        else
        {
            if (InventoryManager.instance.inventory[0] == -1 && InventoryManager.instance.inventory[1] == -1 && InventoryManager.instance.inventory[2] == -1
            && InventoryManager.instance.inventory[3] == -1)
            {
                Enable_Minigame_Pulse();
                random = 0;
            }
            else
                Enable_Minigame_Inventory();
        }

        return random;
    }

    private void Fail_Minigame(int minigameCode)
    {
        player.PlayerHp -= 20f;
        image_Back.sprite = sprite_Back;

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
        {
            image_Inventory.sprite = sprite_Inventory;
            InventoryManager.instance.RemoveItem(3);
            InventoryManager.instance.RemoveItem(2);
            InventoryManager.instance.RemoveItem(1);
            InventoryManager.instance.RemoveItem(0);

            isLimit = false;
            isSuccess_Minigame_Inventory = true;
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

        image_Minigame_Pulse.color = new Color32(255, 71, 71, 255);

        while (random_Count > 0)
        {
            text_Minigame_Pulse.text = "" + random_Count;

            yield return new WaitForSeconds(random_Sec);

            if (currentWindow == 0) // if click back button
            {
                ing_Minigame = false;
                yield break;
            }

            random_Count -= 1;
        }

        text_Minigame_Pulse.text = "touch";
        image_Minigame_Pulse.color = new Color32(77, 219, 100, 255);

        yield return new WaitForSeconds(random_Sec);

        if (currentWindow == 0) // if click back button
        {
            ing_Minigame = false;
            yield break;
        }

        if (isTouch_Minigame_Pulse)
        {
            isTouch_Minigame_Pulse = false;
            ing_Minigame = false;
            yield break;
        }

        ing_Minigame = false;
        Click_Back();
    }

    private IEnumerator Minigame_Inventory()
    {
        image_Inventory.sprite = sprite_Inventory;
        isSuccess_Minigame_Inventory = false;

        button_Hacking.transform.position = new Vector3(button_Hacking_Position_X, button_Hacking.transform.position.y,
            button_Hacking.transform.position.z);

        for (int i = 0; i < 4; i++)
        {
            if (InventoryManager.instance.inventory[i] != -1)
                exist_button.Add(i);
        }

        int random = Random.Range(0, exist_button.Count);

        switch (exist_button[random])
        {
            case 0:
                button_Hacking.transform.position = new Vector3(button_Hacking.transform.position.x - 185f, button_Hacking.transform.position.y,
            button_Hacking.transform.position.z);
                break;

            case 1:
                button_Hacking.transform.position = new Vector3(button_Hacking.transform.position.x - 60f, button_Hacking.transform.position.y,
            button_Hacking.transform.position.z);
                break;

            case 2:
                button_Hacking.transform.position = new Vector3(button_Hacking.transform.position.x + 65f, button_Hacking.transform.position.y,
            button_Hacking.transform.position.z);
                break;

            case 3:
                button_Hacking.transform.position = new Vector3(button_Hacking.transform.position.x + 190f, button_Hacking.transform.position.y,
            button_Hacking.transform.position.z);
                break;

            default:
                break;
        }

        button_Hacking.SetActive(true);

        randomTouch = Random.Range(5, 8);
        countTouch = 0;

        text_Hacking.text = "" + randomTouch;

        yield return new WaitForSeconds((float)randomTouch);

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
}
