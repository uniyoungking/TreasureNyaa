using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.5f;

    private GameObject player;

    private const float rangeX = 4.4f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }


    private void LateUpdate()
    {
        MoveUp();
        //MoveTest();
    }


    // 자동으로 화면을 위로 윔직임
    private void MoveUp()
    {
        float posX = player.transform.position.x;
        posX = Mathf.Clamp(posX, -rangeX, rangeX);

        float posY = transform.position.y;
        posY += moveSpeed * Time.deltaTime;

        if (player.transform.position.y > posY)
            posY = player.transform.position.y;

        transform.position = new Vector3(posX, posY, transform.position.z);
    }



    // 테스트용.. 플레이어 없어도 실행 가능
    private void MoveTest()
    {
        float posX = transform.position.x;

        posX += Input.GetAxis("Horizontal");
        posX = Mathf.Clamp(posX, -rangeX, rangeX);

        float posY = transform.position.y;
        posY += moveSpeed * Time.deltaTime;

        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
