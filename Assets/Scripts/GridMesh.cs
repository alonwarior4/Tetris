using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMesh : MonoBehaviour
{
    Mesh gridMesh;
    GridMap gridMap;
    [SerializeField] float quadSizeRatio = 0.75f;

    //Mesh Settings
    int quadCount;
    float quadSize;
    Vector3[] vertices;
    Color[] colors;
    int[] triangles;

    private void Start()
    {
        gridMap = GetComponent<GridMap>();
        quadCount = gridMap.Size;
        quadSize = gridMap.cellSize * quadSizeRatio;

        GenerateGrid();
    }

    public void GenerateGrid()
    {
        vertices = new Vector3[quadCount * 4];
        colors = new Color[quadCount * 4];
        triangles = new int[quadCount * 6];

        for(int i=0; i< quadCount; i++)
        {
            int y = i / gridMap.x;
            int x = i - (y * gridMap.x);

            vertices[i * 4 + 0] = (Vector3)gridMap.GetMidPos(x, y) - new Vector3(1, 1, 0) * 0.5f * quadSize - transform.position;
            vertices[i * 4 + 1] = vertices[i * 4 + 0] + Vector3.up * quadSize;
            vertices[i * 4 + 2] = vertices[i * 4 + 0] + new Vector3(1, 1, 0) * quadSize;
            vertices[i * 4 + 3] = vertices[i * 4 + 0] + Vector3.right * quadSize;

            triangles[i * 6 + 0] = i * 4 + 0;
            triangles[i * 6 + 1] = i * 4 + 1;
            triangles[i * 6 + 2] = i * 4 + 2;
            triangles[i * 6 + 3] = i * 4 + 0;
            triangles[i * 6 + 4] = i * 4 + 2;
            triangles[i * 6 + 5] = i * 4 + 3;
        }

        gridMesh = new Mesh();
        gridMesh.vertices = vertices;
        gridMesh.triangles = triangles;

        GetComponent<MeshFilter>().mesh = gridMesh;

        //Refresh();
    }

    public void ReColor()
    {
        for(int i = 0; i< quadCount; i++)
        {
            int y = i / gridMap.x;
            int x = i - (y * gridMap.x);
            Color cellColor = gridMap.Cells[x, y].cellColor;

            colors[i * 4 + 0] = cellColor;
            colors[i * 4 + 1] = cellColor;
            colors[i * 4 + 2] = cellColor;
            colors[i * 4 + 3] = cellColor;
            //if(x == 0 || y == 0 || x == gridMap.x - 1 || y == gridMap.y - 1)
            //{
            //    colors[i * 4 + 0] = Color.black;
            //    colors[i * 4 + 1] = Color.black;
            //    colors[i * 4 + 2] = Color.black;
            //    colors[i * 4 + 3] = Color.black;
            //}
            //else
            //{
            //    colors[i * 4 + 0] = Color.white;
            //    colors[i * 4 + 1] = Color.white;
            //    colors[i * 4 + 2] = Color.white;
            //    colors[i * 4 + 3] = Color.white;
            //}
        }

        gridMesh.colors = colors;
    }

    //public void ColorQuads(Shape shape)
    //{
    //    for(int j = shape.gridPos.y ; j < shape.shape.GetLength(1); j--)
    //    {
    //        for(int i = shape.gridPos.x ; i < shape.shape.GetLength(0); i++)
    //        {
    //            int quadIndex = i + j * gridMap.x;
    //            colors[quadIndex * 4 + 0] = shape.shapeColor;
    //            colors[quadIndex * 4 + 1] = shape.shapeColor;
    //            colors[quadIndex * 4 + 2] = shape.shapeColor;
    //            colors[quadIndex * 4 + 3] = shape.shapeColor;
    //        }
    //    }

    //    gridMesh.colors = colors;
    //}    
}
