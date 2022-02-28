using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [HideInInspector] public int[] inventory;
    public GameObject[] inventoryButtons;
    [HideInInspector] public bool[] isWrap;
    [SerializeField] private Sprite sprite_Empty;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        inventory = new int[4];
        isWrap = new bool[4];
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
            inventory[i] = -1;

        for (int i = 0; i < 4; i++)
            isWrap[i] = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < 4; i++)
                Debug.Log(inventory[i]);
        }
    }

    public void AddItem(int itemCode) // itemType
    {
        if (!UIManager.instance.isActive_Minigame_Inventory)
        {
            bool isFull = true;

            for (int i = 0; i < 4; i++)
            {
                if (inventory[i] < 0)
                {
                    inventory[i] = itemCode;
                    inventoryButtons[i].GetComponent<Image>().sprite = UIManager.instance.sprites_Item[7];
                    isFull = false;
                    break;
                }
            }

            if (isFull)
                Debug.Log("Full Inventory");
        }
    }

    public void RemoveItem(int buttonCode)
    {
        inventory[buttonCode] = -1;
        inventoryButtons[buttonCode].GetComponent<Image>().sprite = sprite_Empty;
    }
}
