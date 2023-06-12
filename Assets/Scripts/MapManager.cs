using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int mazeX;
    public int mazeY;
    public float interval;

    public int[,] maze;

    public GameObject cube;

    Transform cubeHolder;

    // Start is called before the first frame update
    void Awake()
    {
        cubeHolder = new GameObject("Walls").transform;
        //스폰 장소 변경
        GameObject.Find("Player").transform.position = new Vector3(interval, 1, interval);
        cube.transform.localScale = new Vector3(interval, interval, interval);
        //미로 벽 생성, 1은 벽 0은 빈칸
        maze = new int[mazeY, mazeX];
        for (int y = 0; y < mazeY; y++)
        {
            for (int x = 0; x < mazeX; x++)
            {
                if (x % 2 == 0 || y % 2 == 0)
                {
                    maze[y, x] = 1;
                }
                else
                {
                    maze[y, x] = 0;
                }
            }
        }
        Generate();

        //벽 오브젝트 생성
        for (int y = 0; y < mazeY; y++)
        {
            for (int x = 0; x < mazeX; x++)
            {
                if (maze[y, x] == 1)
                {
                    GameObject temp = Instantiate(cube, new Vector3(x* interval, 0.5f * interval, y* interval), Quaternion.identity);
                    temp.transform.SetParent(cubeHolder);
                }
            }
        }

    }

    void Generate()
    {
        for (int y = 0; y < mazeY; y++)
        {
            for (int x = 0; x < mazeX; x++)
            {
                //최외각 둘러싸는 블록
                if (x % 2 == 0 || y % 2 == 0)
                    continue;

                //탈출구
                if (x == mazeX - 2 && y == mazeY - 2)
                {
                    maze[y + 1, x] = 0;
                    continue;
                }
                
                //최상단 및 최우측 빈칸 만들기
                if (x == mazeX-2)
                {
                    maze[y + 1, x] = 0;
                    continue;
                }
                if (y == mazeY-2)
                {
                    maze[y, x + 1] = 0;
                    continue;
                }
                
                else
                {
                    int dir = Random.Range(0, 2); // 0일때 위, 1일때 오른쪽

                    if (dir == 0)
                    {
                        maze[y + 1, x] = 0;
                    }
                    else
                    {

                        maze[y, x + 1] = 0;
                    }
                }

            }
        }
    }
}
