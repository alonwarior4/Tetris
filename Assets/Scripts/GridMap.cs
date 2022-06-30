using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public int x;
    public int y;
    public float cellSize;
    Cell[,] cells;
    public Cell[,] Cells => cells;
    public int Size => x * y;

    GridMesh gridMesh;

    private void Start()
    {
        gridMesh = GetComponent<GridMesh>();
    }

    public void Refresh()
    {
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                Cell cell = cells[i, j];

                if (!cell.isBlocked)
                {
                    //cell.isBlocked = false;
                    cell.isShape = false;
                    cell.cellColor = Color.white;
                }

                //if (i == 0 || i == x - 1 || j == 0 || j == y - 1)
                //{
                //    cell.isBlocked = true;
                //    cell.cellColor = Color.black;
                //}
                //else
                //{
                //    cell.isBlocked = false;
                //    cell.isShape = false;
                //    cell.cellColor = Color.white;
                //}
            }
        }

        //gridMesh.Refresh();
    }

    public void GenerateGridMap()
    {
        cells = new Cell[x, y];
        for(int j = 0; j < y; j++)
        {
            for(int i=0; i< x; i++)
            {
                Cell cell = new Cell();
                cell.isShape = false;
                
                if (i == 0 || i == x - 1 || j == 0 || j == y - 1)
                {
                    cell.isBlocked = true;
                    cell.cellColor = Color.black;
                }
                else
                {
                    cell.isBlocked = false;
                    cell.cellColor = Color.white;
                }

                cells[i, j] = cell;
            }
        }

        gridMesh.ReColor();
    }

    public bool IsValidIndex(int x, int y) => x >= 0 && y >= 0 && x < this.x && y < this.y;

    public bool IsValidIndex(Vector2Int pos) => IsValidIndex(pos.x, pos.y);

    public Vector2 GetGridPos(int x , int y) => (Vector2)transform.position + new Vector2(x, y) * cellSize;

    public Vector2 GetMidPos(int x, int y) => GetGridPos(x, y) + Vector2.one * cellSize * 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                Gizmos.DrawLine(GetGridPos(i, j), GetGridPos(i + 1, j));
                Gizmos.DrawLine(GetGridPos(i, j), GetGridPos(i, j + 1));
            }
        }

        Gizmos.DrawLine(GetGridPos(x, 0), GetGridPos(x, y));
        Gizmos.DrawLine(GetGridPos(0, y), GetGridPos(x, y));

        if (cells == null) return;
        for (int j = 0; j < y; j++)
        {
            for (int i = 0; i < x; i++)
            {
                if (cells[i, j].isBlocked)
                    Gizmos.color = Color.black;
                else
                    Gizmos.color = Color.white;

                if (cells[i, j].isShape)
                    Gizmos.color = Color.blue;

                Gizmos.DrawCube(GetMidPos(i, j), Vector3.one * cellSize * 0.75f);
            }
        }
    }
}

public class Cell
{
    public bool isBlocked;
    public bool isShape;
    public Color cellColor;

    public Cell() { }

    public Cell(bool isBlocked , bool isShape , Color cellColor = default)
    {
        this.isBlocked = isBlocked;
        this.isShape = isShape;
        this.cellColor = cellColor;
    }
}
