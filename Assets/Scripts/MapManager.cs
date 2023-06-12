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
        //���� ��� ����
        GameObject.Find("Player").transform.position = new Vector3(interval, 1, interval);
        cube.transform.localScale = new Vector3(interval, interval, interval);
        //�̷� �� ����, 1�� �� 0�� ��ĭ
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

        //�� ������Ʈ ����
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
                //�ֿܰ� �ѷ��δ� ���
                if (x % 2 == 0 || y % 2 == 0)
                    continue;

                //Ż�ⱸ
                if (x == mazeX - 2 && y == mazeY - 2)
                {
                    maze[y + 1, x] = 0;
                    continue;
                }
                
                //�ֻ�� �� �ֿ��� ��ĭ �����
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
                    int dir = Random.Range(0, 2); // 0�϶� ��, 1�϶� ������

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
