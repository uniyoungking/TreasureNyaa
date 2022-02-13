using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Transform camera_Transform = null;

    private void Awake()
    {
        camera_Transform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector2(0, camera_Transform.position.y - 4);
    }
}
