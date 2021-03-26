using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UGrid : MonoBehaviour
{
    private bool[,] Grid;
    private static int columns, rows;
    private Sprite GridSprite;
    private List<GameObject> NomList;
    private GameObject Agent;
    private int posRow, posCol;
    private Sprite MonsterSprite;
    

    private void Update()
    {
        
        
        
    }
    
    
    public void createUGrid(bool[,] grid)
    {
        Grid = grid;
        
        NomList = new List<GameObject>();

        rows = grid.GetLength(0);
        columns = grid.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (Grid[i, j])
                {
                    SpawnNom(i, j);
                }
            }
        }
        
        Agent = new GameObject("Cookie Monster");
        SpawnAgent(columns, rows);
    }
    
    
    public void SpawnAgent(int hor, int ver)
    {
        posCol = Random.Range(0, hor);
        posRow = Random.Range(0, ver);
        transform.position = new Vector3(0.5f - hor + posCol, 0.5f - ver + posRow, -2);
        var spt = Agent.AddComponent<SpriteRenderer>();
        spt.sprite = Resources.Load<Sprite>("monster");
    }

    private void SpawnNom(int row, int col)
    {
        GameObject nom = new GameObject("[" + row + ", " + col + "]");
        NomList.Add(nom);
        nom.transform.parent = this.transform;
        nom.transform.position = new Vector3(col - (columns/2) + 0.5f, row - (rows/2) + 0.5f,  -1);
        var spt = nom.AddComponent<SpriteRenderer>();
        spt.sprite = Resources.Load<Sprite>("nom");
    }

    public void UpdateGrid()
    {
        
    }
    
    public void ClearNomList()
    {
        foreach (GameObject nom in NomList)
        {
            Destroy(nom);
        }
        NomList.Clear();
    }
    
    public void turnOffNomSprite(int row, int col)
    {
        GameObject nom = GameObject.Find("[" + row + ", " + col + "]");

        if (nom)
        {
            nom.GetComponent<SpriteRenderer>().enabled = false;   
        }
    }
    
    public void ClearGrid()
    {
        ClearNomList();
        
        Destroy(GameObject.Find("NomGrid"));
        
        Agent = null;
        Destroy(GameObject.Find("Monster"));
    }
}
