using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float vAxis;
    float hAxis;
    bool sDown;

    Vector3 moveVector;

    public float speed;
    public float jumpSpeed;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyBoardInput();
        PlayerMove();
        playerJump();
    }

    void KeyBoardInput()
    {
        vAxis = Input.GetAxisRaw("Vertical");
        hAxis = Input.GetAxisRaw("Horizontal");
        sDown = Input.GetButtonDown("Jump");
    }

    void PlayerMove()
    {
        moveVector = hAxis * transform.right + vAxis * transform.forward;

        moveVector = new Vector3(moveVector.x, 0, moveVector.z).normalized;

        transform.position += moveVector * speed * Time.deltaTime;
    }

    void playerJump()
    {
        if (sDown)
        {
            rigid.AddForce(transform.up * jumpSpeed, ForceMode.Impulse);
        }
    }

}
