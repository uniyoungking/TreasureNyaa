using UnityEngine;
using TMPro;

public class CollisionChecker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    private int coinCount = 0;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            ItemType type = item.MyItemType;

            switch (type)
            {
                case ItemType.Coin:
                    GetCoin(item);
                    break;

                case ItemType.Gift:
                    GetGift(item);
                    break;

                case ItemType.FireFairy:
                    HitFireFairy(item);
                    break;

                case ItemType.TreeFairy:
                    HitTreeFairy(item);
                    break;
                case ItemType.TreeFairy2:
                    HitTreeFairy(item);
                    break;
            }
        }
    }


    private void GetCoin(Item item)
    {
        coinCount++;
        coinText.text = string.Format("{0:0000}", coinCount);

        item.EnqueuePooler();
    }


    private void GetGift(Item item)
    {
        item.EnqueuePooler();

        int random = Random.Range(0, 7);

        InventoryManager.instance.AddItem(random);
    }

    private void HitFireFairy(Item item)
    {
        //Debug.LogWarning("불정령에 닿음");
    }

    private void HitTreeFairy(Item item)
    {
        //Debug.LogWarning("나무정령에 닿음");
    }

}
