using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int stagenumber;
    public Stage stage;
    public Gem[,] allGems;

    public GameObject bgTilePrefab;
    public Gem gemtouse;
    public float adjScale;
    public float moveSpeed;

    [HideInInspector]
    public int width, height, size;

    private List<int> suffledList = new List<int>();
    [HideInInspector]
    public int[] suffleGems;

    public CameraMng cameramng;
    private float adjvar;

    [HideInInspector]
    public int[] copyarray;
    [HideInInspector]
    public int[,] finalarray;

    private void Start()
    {
        scaleAdj(adjScale);
        SuffledGems();
        allGems = new Gem[width, height];
        Setup();
        //CopyGemArray();
    }

    private void SuffledGems()
    {
        width = stage.stats[stagenumber].width;
        height = stage.stats[stagenumber].height;
        size = width * height;
        
        int usedsuffle;
        int gemNumberCount = stage.stats[stagenumber].UsingGems.Length;

        suffleGems = new int[size];

        for(int inUsingGem = 0; inUsingGem < gemNumberCount; inUsingGem++)
        {
            for(int inGemSize = 0; inGemSize < stage.stats[stagenumber].UsingGemNumbers[inUsingGem]; inGemSize++)
            {
                suffledList.Add(inUsingGem);
            }
        }

        for(int inSize = 0; inSize < size; inSize++)
        {
            usedsuffle = UnityEngine.Random.Range(0, suffledList.Count);
            suffleGems[inSize] = suffledList[usedsuffle];
            suffledList.RemoveAt(usedsuffle);
        }
    }

    private void Setup()
    {
        var index = 0;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);

                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.localScale = new Vector3(adjvar*(float)0.4, adjvar*(float)0.4, 1);
                bgTile.transform.parent = transform;
                bgTile.name = "BG Tile - " + x + ", " + y;

                int gemToUse = suffleGems[index];

                SpawnGem(new Vector2Int(x, y), stage.stats[stagenumber].UsingGems[gemToUse]);
                
                index += 1;
            }
        }
    }

    private void SpawnGem(Vector2Int pos, int usingGems)
    {
        var x = pos.x;
        var y = pos.y;

        Gem gem = Instantiate(gemtouse, new Vector3(x, y, 0f), Quaternion.identity);
        
        SpriteRenderer spriteR = gem.GetComponent<SpriteRenderer>();
        BoxCollider m_Collider = gem.GetComponent<BoxCollider>();
        spriteR.sprite = gem.sprites[usingGems];
        gem.transform.localScale = new Vector3(adjvar, adjvar, 1);
        m_Collider.size = new Vector3(adjvar*1000, adjvar*1000, 1);
        gem.type = (Gem.GemType)usingGems;

        gem.transform.parent = transform;
        gem.name = "Gem - " + x + ", " + y;
        
        allGems[x, y] = gem;
        
        gem.SetupGem(pos, this);
    }

    private float scaleAdj(float scale)
    {
        adjvar = cameramng.adjvar ;
        adjvar = scale * adjvar;

        return adjvar;
    }

    public void CopyGemArray()
    {
        copyarray = (int[])suffleGems.Clone();
        finalarray = new int[width, height];

        int index = 0;

        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                finalarray[i, k] = copyarray[index];
                Debug.Log(height);
                Debug.Log("i = " + i + " k = " + k + " 내부의값은 : " + finalarray[i, k]);
                ++index;
            }
        }
    }
}
