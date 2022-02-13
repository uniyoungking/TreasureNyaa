using LSY;
using UnityEngine;

public enum PlatformSize { Small, Midium, Large }

public struct PlatformInfo
{
    public PlatformGenerator generator;
    public ObjectPooler pooler;
    public string objName;
    public int listKey;
}

public abstract class PlatformBase : MonoBehaviour
{
    [field: SerializeField]
    public PlatformSize myPlatformSize { get; private set; }

    protected ObjectPooler pooler;
    protected PlatformGenerator generator;

    [SerializeField]
    protected int myListDicKey;

    protected string objectName;

    protected SpriteRenderer spriteRenderer;
    protected SpriteRenderer SpriteRenderer
    {
        get
        {
            if (!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();

            return spriteRenderer;
        }
    }

    public abstract void HitPlatform(PlayerController player);


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            ReturnPlatform();
        }
    }


    public void Initialize(PlatformInfo info)
    {
        generator = info.generator;
        pooler = info.pooler;
        objectName = info.objName;
        myListDicKey = info.listKey;
    }


    public float GetSpriteSizeX()
    {
        return SpriteRenderer.bounds.size.x;
    }

    public float GetLeftPosX()
    {
        return transform.position.x - (GetSpriteSizeX() * 0.5f);
    }

    public float GetRightPosX()
    {
        return transform.position.x + (GetSpriteSizeX() * 0.5f);
    }

    // 블록 삭제
    public void ReturnPlatform()
    {
        // 삭제할 때 해당 줄의 리스트에서도 삭제하기

        if (pooler)
        {
            generator.RemovePlatformInList(myListDicKey, this);
            pooler.EnqueueObject(objectName, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
