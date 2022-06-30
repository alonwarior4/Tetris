using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotState
{
    Zero , Right , Half , Left
}

public class Shape 
{
    public string name;
    public int[,] shape;
    public Vector2Int gridPos;
    public RotState rotState;

    //TODO : Use this for sure
    public Color shapeColor;    

    public Shape(string name , int[,] shape , Color shapeColor = default)
    {
        this.name = name;
        this.shape = shape;
        rotState = RotState.Zero;
        this.shapeColor = shapeColor;
    }

    public void ReConfigShape(string name , int[,] shape , Color shapeColor = default)
    {
        this.name = name;
        this.shape = shape;
        rotState = RotState.Zero;
        this.shapeColor = shapeColor;
    }

    public void ShapeRotated()
    {
        switch (rotState)
        {
            case RotState.Zero:
                rotState = RotState.Right;
                break;
            case RotState.Right:
                rotState = RotState.Half;
                break;
            case RotState.Half:
                rotState = RotState.Left;
                break;
            case RotState.Left:
                rotState = RotState.Zero;
                break;            
        }
    }
}
