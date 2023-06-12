using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    class Pos
    {
        //위, 오른쪽, 아래, 왼쪽 좌표 y,x
        public int[] dirPosX = new int[4] {0, 1, 0, -1};
        public int[] dirPosY = new int[4] {1, 0, -1, 0};

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

    int curPosX;
    int curPosY;

    int nextPosX;
    int nextPosY;

    Pos pos;

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        maze = GameObject.Find("GameManager").GetComponent<MapManager>();

        pos = new Pos();

        curPosX = (int)(transform.position.x / maze.interval);
        curPosY = (int)(transform.position.y / maze.interval);

        RightHand();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.red);

        if (transform.position.x * maze.interval == nextPosX && transform.position.y * maze.interval == nextPosY)
        {
            curPosX = nextPosX;
            curPosY = nextPosY;
            RightHand();
        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(nextPosX * maze.interval, transform.position.y, nextPosY * maze.interval), 3f * Time.deltaTime);
    }

    
    void RightHand()
    {
        //비어있는 곳. 가도됨
        if (maze.maze[curPosY + pos.dirPosY[pos.dir], curPosX + pos.dirPosX[pos.dir]] == 0)
        {
            nextPosX = curPosX + pos.dirPosX[pos.dir];
            nextPosY = curPosY + pos.dirPosY[pos.dir];
            return;
        }
        //막혀있는 곳. 돌아야함
        else
        {
            //재귀 돌려서 빈 곳 찾기
            pos.changeDir();
            RightHand();
        }
    }
}
