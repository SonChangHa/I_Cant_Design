using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public float speed;
    int dig;
    class Pos
    {
        enum Dir
        {
            Up = 0, Right = 1, Down = 2, Left = 3
        }

        public int dir = (int)Dir.Up;

        public void changeDir()
        {
            dir = dir + 1 % 4;
        }
    }
    MapManager maze;

    Pos pos;

    RaycastHit hit;

    Vector3 moveVector;
    // Start is called before the first frame update
    void Start()
    {
        maze = GameObject.Find("GameManager").GetComponent<MapManager>();

        pos = new Pos();

        RightHand();
        dig = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red);

        if (transform.position == moveVector)
        {
            RightHand();
        }
        transform.position = Vector3.MoveTowards(transform.position, moveVector, speed * Time.deltaTime);
    }
    void RightHand()
    {
        if (!Physics.Raycast(transform.position, transform.right, out hit, 3))
        {
            transform.Rotate(new Vector3(0, 90f, 0));
            moveVector = transform.position + transform.forward * 3;
        } 
        else if (!Physics.Raycast(transform.position, transform.forward, out hit, 3))
        {
            moveVector = transform.position + transform.forward * 3;
        }
        else if(!Physics.Raycast(transform.position, -transform.right, out hit, 3))
        {
            transform.Rotate(new Vector3(0, -90f, 0));
            moveVector = transform.position + transform.forward * 3;
        }
        else 
        {
            transform.Rotate(new Vector3(0, -180f, 0));
            moveVector = transform.position + transform.forward * 3;
        }
    }

}
