using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSY;

public enum ItemType { Coin, Gift, FireFairy, TreeFairy, TreeFairy2 }

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemType MyItemType { get; private set; }

    private ObjectPooler pooler;

    private string objectName;


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            if (pooler)
                pooler.EnqueueObject(objectName, gameObject);
            else
                Destroy(gameObject);
        }
    }

    public void Initialize(ObjectPooler pooler, string name)
    {
        this.pooler = pooler;
        objectName = name;
    }


    public void EnqueuePooler()
    {
        // 큐에 다시 넣기
        gameObject.SetActive(false);
        pooler.EnqueueObject(objectName, gameObject);
    }
}
