using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;


public class MapLocation
{
    public int x;
    public int z;

    public MapLocation(int _x, int _z)
    {
        x= _x;
        z= _z;
    }
}
public class MazeLogic : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public int scale = 6;
    public List<GameObject> Cube;
    public GameObject Character;
    public GameObject Enemy;
    public List<GameObject> Items;
    public byte[,] map;
    bool item = false;
    // Start is called before the first frame update
    void Start()
    {
        InitialiseMap();
        GenerateMap();
        DrawMaps();
        PlaceCharacter();
        PlaceEnemy();
        PlaceItems();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitialiseMap()
    {
        map = new byte[width, depth];
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                map[x, z] = 1;
            }
    }

    public virtual void GenerateMap()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (Random.Range(0, 100) < 50)
                    map[x, z] = 0;
            }
    }

    void DrawMaps()
    {
        for (int z = 0; z < depth; z++)
            for (int x = 0; x < width; x++)
            {
                if (map[x, z] == 1)
                {
                    Vector3 pos = new Vector3(x * scale, 0, z*scale);
                    GameObject wall = Instantiate(Cube[Random.Range(0, Cube.Count)], pos, Quaternion.identity);
                    wall.transform.localScale = new Vector3(scale, scale, scale);
                    wall.transform.position = pos;
                }
            }
    }

    public int CountSquareNeighbours(int x, int z)
    {
        int count = 0;
        if (x <= 0 || x >= width - 1 || z <= 0 || z >= depth - 1) return 5;
        if (map[x - 1, z] == 0) count++;
        if (map[x + 1, z] == 0) count++;
        if (map[x, z + 1] == 0) count++;
        if (map[x, z - 1] == 0) count++;

        return count;
    }

    public virtual void PlaceCharacter()
    {
        bool PlayerSet = false;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x, z] == 0 && !PlayerSet)
                {
                    Debug.Log("Placing Character");
                    PlayerSet = true;
                    Character.transform.position = new Vector3(x * scale, -2.442f, z * scale);
                }
                else if (PlayerSet)
                {
                    Debug.Log("Already Placing Character");
                    return;
                }
            }
        }
    }

    public virtual void PlaceEnemy()
    {
        bool EnemySet = false;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x, z] == 0 && !EnemySet)
                {
                    Debug.Log("Placing Enemy");
                    EnemySet = true;
                    Enemy.transform.position = new Vector3(x * scale, -2.442f, z * scale);
                }
                else if (EnemySet)
                {
                    Debug.Log("Already Placing Enemy");
                    return;
                }
            }
        }
    }
    public virtual void PlaceItems()
    {
        for (int a=0; a<Items.Count; a++)
        { 
            for (int i = 0; i < depth; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int x = Random.Range(0, width);
                    int z = Random.Range(0, depth);
                    if (map[x, z] == 0 && !item)
                    {
                        Debug.Log("Placing Item");
                        Items[a].transform.position = new Vector3(x * scale, -2.442f, z * scale);
                    }
                    else if (item)
                    {
                        Debug.Log("Already Placing Enemy");
                        return;
                    }
                }
            }
        }
        item = true;

    }

}

