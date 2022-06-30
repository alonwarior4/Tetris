using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shape Config")]
public class ShapeConfig : ScriptableObject
{
    //shape Cashes
    int[,] map3x3 = new int[3, 3];
    int[,] map4x4 = new int[4, 4];
    int[,] map4x3 = new int[4, 3];

    [Header("Colors")]
    [SerializeField] Color T_Color;
    [SerializeField] Color O_Color;
    [SerializeField] Color Z_Color;
    [SerializeField] Color S_Color;
    [SerializeField] Color I_Color;
    [SerializeField] Color L_Color;
    [SerializeField] Color J_Color;

    [Header("Sprites")]
    [SerializeField] Sprite T_Sprite;
    [SerializeField] Sprite O_Sprite;
    [SerializeField] Sprite Z_Sprite;
    [SerializeField] Sprite S_Sprite;
    [SerializeField] Sprite I_Sprite;
    [SerializeField] Sprite L_Sprite;
    [SerializeField] Sprite J_Sprite;


    #region Shape

    public int[,] GetShapeMatrix(string shapeName)
    {
        //TODO : change name with enum for better performance
        switch (shapeName)
        {
            case "T":
                return T_ShapeMatrix();
            case "O":
                return O_ShapeMatrix();
            case "Z":
                return Z_ShapeMatrix();
            case "S":
                return S_ShapeMatrix();
            case "I":
                return I_ShapeMatrix();
            case "L":
                return L_ShapeMatrix();
            case "J":
                return J_ShapeMatrix();

            default:
                return null;
        }
    }

    int[,] T_ShapeMatrix()
    {
        //int[,] map = new int[3, 3];

        map3x3[0, 0] = 0;
        map3x3[1, 0] = 1;
        map3x3[2, 0] = 0;
        map3x3[0, 1] = 1;
        map3x3[1, 1] = 1;
        map3x3[2, 1] = 1;
        map3x3[0, 2] = 0;
        map3x3[1, 2] = 0;
        map3x3[2, 2] = 0;

        return map3x3;
    }

    int[,] O_ShapeMatrix()
    {
        //int[,] map = new int[4, 3];
        map4x3[0, 0] = 0;
        map4x3[1, 0] = 1;
        map4x3[2, 0] = 1;
        map4x3[3, 0] = 0;
        map4x3[0, 1] = 0;
        map4x3[1, 1] = 1;
        map4x3[2, 1] = 1;
        map4x3[3, 1] = 0;
        map4x3[0, 2] = 0;
        map4x3[1, 2] = 0;
        map4x3[2, 2] = 0;
        map4x3[3, 2] = 0;

        return map4x3;
    }

    int[,] Z_ShapeMatrix()
    {
        //int[,] map = new int[3, 3];
        map3x3[0, 0] = 1;
        map3x3[1, 0] = 1;
        map3x3[2, 0] = 0;
        map3x3[0, 1] = 0;
        map3x3[1, 1] = 1;
        map3x3[2, 1] = 1;
        map3x3[0, 2] = 0;
        map3x3[1, 2] = 0;
        map3x3[2, 2] = 0;

        return map3x3;
    }

    int[,] S_ShapeMatrix()
    {
        //int[,] map = new int[3, 3];
        map3x3[0, 0] = 0;
        map3x3[1, 0] = 1;
        map3x3[2, 0] = 1;
        map3x3[0, 1] = 1;
        map3x3[1, 1] = 1;
        map3x3[2, 1] = 0;
        map3x3[0, 2] = 0;
        map3x3[1, 2] = 0;
        map3x3[2, 2] = 0;

        return map3x3;
    }

    int[,] I_ShapeMatrix()
    {
        //int[,] map = new int[4, 4];
        map4x4[0, 0] = 0;
        map4x4[1, 0] = 0;
        map4x4[2, 0] = 0;
        map4x4[3, 0] = 0;
        map4x4[0, 1] = 1;
        map4x4[1, 1] = 1;
        map4x4[2, 1] = 1;
        map4x4[3, 1] = 1;
        map4x4[0, 2] = 0;
        map4x4[1, 2] = 0;
        map4x4[2, 2] = 0;
        map4x4[3, 2] = 0;
        map4x4[0, 3] = 0;
        map4x4[1, 3] = 0;
        map4x4[2, 3] = 0;
        map4x4[3, 3] = 0;

        return map4x4;
    }

    int[,] L_ShapeMatrix()
    {
        //int[,] map = new int[3, 3];
        map3x3[0, 0] = 1;
        map3x3[1, 0] = 0;
        map3x3[2, 0] = 0;
        map3x3[0, 1] = 1;
        map3x3[1, 1] = 1;
        map3x3[2, 1] = 1;
        map3x3[0, 2] = 0;
        map3x3[1, 2] = 0;
        map3x3[2, 2] = 0;

        return map3x3;
    }

    int[,] J_ShapeMatrix()
    {
        //int[,] map = new int[3, 3];
        map3x3[0, 0] = 0;
        map3x3[1, 0] = 0;
        map3x3[2, 0] = 1;
        map3x3[0, 1] = 1;
        map3x3[1, 1] = 1;
        map3x3[2, 1] = 1;
        map3x3[0, 2] = 0;
        map3x3[1, 2] = 0;
        map3x3[2, 2] = 0;

        return map3x3;
    }

    #endregion

    #region Color

    public Color GetShapeColor(string shapeName)
    {
        switch (shapeName)
        {
            case "T":
                return T_Color;
            case "O":
                return O_Color;
            case "Z":
                return Z_Color;
            case "S":
                return S_Color;
            case "I":
                return I_Color;
            case "L":
                return L_Color;
            case "J":
                return J_Color;

            default:
                return default;
        }
    }

    #endregion

    #region Sprite

    public Sprite GetShapeSprite(string shapeName)
    {
        switch (shapeName)
        {
            case "T":
                return T_Sprite;
            case "O":
                return O_Sprite;
            case "Z":
                return Z_Sprite;
            case "S":
                return S_Sprite;
            case "I":
                return I_Sprite;
            case "L":
                return L_Sprite;
            case "J":
                return J_Sprite;

            default:
                return null;
        }
    }

    #endregion
}
