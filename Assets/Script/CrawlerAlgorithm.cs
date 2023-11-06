using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrawlerAlgorithm : MazeLogic
{
    public override void GenerateMap()
    {
        bool done = false;
        int x = width / 2;
        int z = depth / 2;

        while (!done)
        {
            map[x, z] = 0;
            if (Random.Range(0,100) < 50)
                x += Random.Range(-1, 2);
            else
                z += Random.Range(-1, 2);
            

            if(x < 0 || x > width || z < 0 || z >= depth)
                done = true;
            else
                done = false;
        }
    }
}
