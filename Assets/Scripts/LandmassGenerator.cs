using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LandmassGenerator : MonoBehaviour
{
    public Tile[] tiles;
    public Tilemap tilemap;
    public float[] levels;
    public Vector2 sizeMap;
    Vector2 vSeed;
    public float seed, scale;
    public float compresionAmount;
    
    void Start()
    {
        StartCoroutine(GenTerrain());
    }

    IEnumerator GenTerrain()
    {
        Debug.Log("Starting generation...");
        for(int y = 0; y < sizeMap.y; y++)
        {
            vSeed.y = seed + y;
            yield return null;
            for(int x = 0; x < sizeMap.x; x++)
            {
                vSeed.x = seed + x;

                //Finding the distance from the edge using the Eucledian distance function
                float distFromEdgeX = x / sizeMap.x - 0.5F;
                float distFromEdgeY = y / sizeMap.y - 0.5F;
                float distFromEdge = Mathf.Sqrt(distFromEdgeX * distFromEdgeX + distFromEdgeY * distFromEdgeY) / Mathf.Sqrt(0.5F);
                float perlinNoiseValue = Mathf.PerlinNoise(((float)x + vSeed.x) / scale, ((float)y + vSeed.y) / scale);

                distFromEdge = Mathf.Pow(distFromEdge, compresionAmount);

                perlinNoiseValue = (1 + perlinNoiseValue - distFromEdge) / 2;

                tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(perlinNoiseValue));
            }
        }
        Debug.Log("Generation finished.");
    }

    Tile SelectTile(float p)
    {
        for(int i = 0; i < levels.Length; i++)
        {
            Debug.Log("Generating level: " + i);
            if (p < levels[i])
            {
                return tiles[i];
            }
        }
        return null;
    }
}
