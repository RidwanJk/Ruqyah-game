using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
    int playerX;
    int playerY;
    public NavMeshAgent enemyagent;
    public NavigationBaker baker;
    public PlayerLogic PL;
    CharacterController characterController;
    public GameManager gameManager;

    [Header("Do Not Edit List")]
    public GameObject waypoints;
    public int waypointscount = 20;
    public List<Transform> WaypointsList = new List<Transform>();


   [Header("Random Audio")]
    public AudioSource RandomSfx;
    public int RandomSfxcount = 5;    
    public List<Transform> RandomSfxList = new List<Transform>();
    public List<AudioClip> audioClips = new List<AudioClip>();      
    bool playingalready = false;

    // Start is called before the first frame update
    void Start()
    {        
        InitialiseMap();
        GenerateMap();
        DrawMaps();
        PlaceCharacter();
        PlaceEnemy();
        PlaceItems();
        PlaceWaypoints();
        RandomAudio();
        
        characterController= Character.GetComponent<CharacterController>();
        characterController.enabled=true;

        //PlaceCharacter();
        if (map[playerX,playerY] == 1) 
        {
            Debug.Log("player ditembok, calling place character lagi");
            PlaceCharacter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckAndPlayAudioInRange();
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

    public void PlaceCharacter()
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
                    Debug.Log("masang karakter di koordinat x: "+x+" z: "+z + " Value : " + map[x,z]);
                    PlayerSet = true;
                    Character.transform.position = new Vector3(x * scale, -2.442f, z * scale);
                    playerX = x;
                    playerY = z;
                }
                else if (PlayerSet)
                {
                    PL.enabled = true;
                    gameManager.ResumeGame();
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
                    EnemySet = true;
                    Enemy.transform.position = new Vector3(x * scale, -2.442f, z * scale);
                }
                else if (EnemySet)
                {
                    baker.bake();
                    enemyagent.enabled = true;
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
                        Items[a].transform.position = new Vector3(x * scale, -2.442f, z * scale);
                    }
                    else if (item)
                    {                       
                        return;
                    }
                }
            }
        }
        item = true;

    }
    public virtual void PlaceWaypoints()
    {
        int waypointset = 0;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x, z] == 0 && waypointset != waypointscount)
                {                    
                    waypointset++;
                    Transform waypointTransform = Instantiate(waypoints, new Vector3(x * scale, 0, z * scale), Quaternion.identity).transform;
                    WaypointsList.Add(waypointTransform);
                }
                else if (waypointset == waypointscount)
                {                    
                    return;
                }
            }
        }
    }

    public virtual void RandomAudio()
    {
        int RandomSfxSet = 0;
        for (int i = 0; i < depth; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int x = Random.Range(0, width);
                int z = Random.Range(0, depth);
                if (map[x, z] == 0 && RandomSfxSet != RandomSfxcount)
                {                    
                    RandomSfxSet++;

                    
                    Transform SFXTransform = Instantiate(RandomSfx, new Vector3(x * scale, 0, z * scale), Quaternion.identity).transform;
                    RandomSfxList.Add(SFXTransform);

                    
                    AudioSource audioSource = SFXTransform.GetComponent<AudioSource>();
                    if (audioSource != null && audioClips.Count > 0)
                    {
                        audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
                    }
                    else
                    {
                        Debug.LogError("AudioSource or audioClips list not set up properly.");
                    }
                }
                else if (RandomSfxSet == RandomSfxcount)
                {                    
                    return;
                }
            }
        }
    }
    void CheckAndPlayAudioInRange()
    {        
        Vector3 playerPosition = Character.transform.position;

        foreach (Transform audioTransform in RandomSfxList)
        {            
            float distance = Vector3.Distance(playerPosition, audioTransform.position);            
            float playRange = 5.0f;

            if (distance < playRange)
            {
                AudioSource audioSource = audioTransform.GetComponent<AudioSource>();
                if (audioSource != null && !audioSource.isPlaying)
                {                    
                    audioSource.Play();
                }
            }
        }
    }

}





