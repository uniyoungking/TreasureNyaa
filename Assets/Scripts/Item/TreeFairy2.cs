using UnityEngine;
using System.Collections;

public class TreeFairy2 : MonoBehaviour
{
    private float speed = 1f;
    private bool movementFlag = false;
    private bool isChanging = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isChanging)
            StartCoroutine(Change());
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (!movementFlag)
        {
            moveVelocity = Vector3.left;
        }
        else if (movementFlag)
        {
            moveVelocity = Vector3.right;
        }

        transform.position += moveVelocity * speed * Time.deltaTime;
    }

    private IEnumerator Change()
    {
        isChanging = true;

        yield return new WaitForSeconds(2f);

        if (!movementFlag)
            movementFlag = true;
        else
            movementFlag = false;

        isChanging = false;
    }
}
