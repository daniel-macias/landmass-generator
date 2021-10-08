using System;
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
    public float edgeCompressionAmount;
    public float decreasingMultiplier;
    public float increasingMultiplier;
    public float decreasingOffset;
    public float increasingOffset;

    
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

                distFromEdge = Mathf.Pow(distFromEdge, edgeCompressionAmount);

                perlinNoiseValue = (1 + perlinNoiseValue - distFromEdge) / 2;

                //Adding a second iteration of perlin-noise randomness to create sub-shapes in the terrain
                perlinNoiseValue = decresePNV(distFromEdge,x,y) + perlinNoiseValue * (incresePNV(distFromEdge, x, y) - decresePNV(distFromEdge, x, y));

                tilemap.SetTile(new Vector3Int(x, y, 0), SelectTile(perlinNoiseValue));
            }
        }
        Debug.Log("Generation finished.");
    }

    //Increses the PNV (Perlin Noise Value) by using the same Perlin Noise generated
    //scaled to create bigger or smaller shapes on the terrain
    private float incresePNV(float distFromEdge,int x, int y)
    {
        float perlinNoiseValue = Mathf.PerlinNoise(((float)x + vSeed.x) / scale* increasingMultiplier, ((float)y + vSeed.y) / scale * increasingMultiplier) + increasingOffset;
        return perlinNoiseValue;
    }

    //Decreses the PNV (Perlin Noise Value) by using the same Perlin Noise generated
    //scaled to create bigger or smaller shapes on the terrain
    private float decresePNV(float distFromEdge, int x, int y)
    {
        float perlinNoiseValue = Mathf.PerlinNoise(((float)x + vSeed.x) / scale * decreasingMultiplier, ((float)y + vSeed.y) / scale * decreasingMultiplier) - decreasingOffset;
        return perlinNoiseValue;
    }

    //Assigns a tile acording the the PNV
    Tile SelectTile(float p)
    {
        for(int i = 0; i < levels.Length; i++)
        {
            if (p < levels[i])
            {
                return tiles[i];
            }
        }
        return null;
    }
}


