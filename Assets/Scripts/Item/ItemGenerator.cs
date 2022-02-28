using System;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    private LSY.ObjectPooler pooler;

    private string[] itemNameArray;

    private void Awake()
    {
        pooler = GetComponent<LSY.ObjectPooler>();

        SetItemNames();
    }

    private void SetItemNames()
    {
        itemNameArray = Enum.GetNames(typeof(ItemType));
    }


    // 발판에서 아이템을 생성함
    // 개별 확률 적용이 필요함
    public void GenerateItem(Vector3 position, string platformName, int level)
    {
        int ran = UnityEngine.Random.Range(0, itemNameArray.Length);

        string itemName = itemNameArray[ran];

        GameObject itemObj = pooler.DequeueObject(itemName);

        if (itemObj.name.Equals("ItemGenerator"))
            return;

        Item item = itemObj.GetComponent<Item>();

        // 불정령은 일반 발판에서만 생성, 일반 발판이라도 가장 작은 A발판인 경우에는 생성X
        if (item.MyItemType == ItemType.FireFairy)
        {
            if (platformName.Equals("NormalBlockA"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("MucusBlockA"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("MucusBlockB"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("MucusBlockC"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("Honey"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("FlowerA"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }
            else if (platformName.Equals("FlowerB"))
            {
                pooler.EnqueueObject(itemName, gameObject);
                return;
            }

            int ran_Fire = UnityEngine.Random.Range(0, 10);
            if (level == 1)
            {
                if (ran_Fire >= 3)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
            else
            {
                if (ran_Fire >= 6)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
        }

        // 테스트용.. 이름에 따라선 구분
        if (item.MyItemType == ItemType.Coin)
        {
            position.y += 0.45f;

            int ran_Coin = UnityEngine.Random.Range(0, 10);
            if (level == 1)
            {
                if (ran_Coin >= 7)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
        }

        else if(item.MyItemType == ItemType.Gift)
        {
            position.y += 0.5f;

            int ran_Gift = UnityEngine.Random.Range(0, 10);
            if (level == 1)
            {
                if (ran_Gift >= 10)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
            else
            {
                if (ran_Gift >= 10)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
        }

        else if (item.MyItemType == ItemType.FireFairy)
        {
            position.y += 0.8f;
        }

        else if (item.MyItemType == ItemType.TreeFairy)
        {
            position.x += 0.12f;
            position.y += 0.6f;

            int ran_Tree = UnityEngine.Random.Range(0, 10);
            if (level == 1)
            {
                if (ran_Tree >= 5)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
            else
            {
                if (ran_Tree >= 2)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
        }

        else if (item.MyItemType == ItemType.TreeFairy2) // 영학 : 일단 발판 간격이 2f라서 가능한 코드. 발판 간격이 좀더 유연하게 수정될 시 수정해야할 부분.
        {
            float ranX = UnityEngine.Random.Range(-6f, 6f);

            position.x += ranX;
            position.y += 1;

            int ran_Tree2 = UnityEngine.Random.Range(0, 10);
            if (level == 1)
            {
                if (ran_Tree2 >= 7)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
            else
            {
                if (ran_Tree2 >= 5)
                {
                    pooler.EnqueueObject(itemName, gameObject);
                    return;
                }
            }
        }

        item.Initialize(pooler, itemName);

        itemObj.transform.position = position;
        itemObj.gameObject.SetActive(true);
    }
}
