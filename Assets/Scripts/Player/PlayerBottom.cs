using UnityEngine;

public class PlayerBottom : MonoBehaviour
{
    private PlayerController player;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        rigid2D = GetComponentInParent<Rigidbody2D>();
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlatformBase platform = collision.GetComponent<PlatformBase>();

        if (!platform)
            return;

        if (rigid2D.velocity.y <= 0f)
        {
            platform.HitPlatform(player);
        }
    }
}
