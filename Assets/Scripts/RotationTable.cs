using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChangeStateType
{
    Zero_Right , Right_Half , Half_Left , Left_Zero
}

public class RotationTable
{
    public static Tests GetOffsetsCommonShapes(ChangeStateType type)
    {
        switch (type)
        {
            case ChangeStateType.Zero_Right:
                return new Tests(Vector2Int.zero , new Vector2Int(-1 , 0) , new Vector2Int(-1 , 1) , 
                    new Vector2Int(0 , -2) , new Vector2Int(-1 , -2));

            case ChangeStateType.Right_Half:
                return new Tests(Vector2Int.zero, new Vector2Int(1, 0), new Vector2Int(1, -1), 
                    new Vector2Int(0, 2), new Vector2Int(1, 2));

            case ChangeStateType.Half_Left:
                return new Tests(Vector2Int.zero, new Vector2Int(1, 0), new Vector2Int(1, 1),
                    new Vector2Int(0, -2), new Vector2Int(1, -2));
                
            case ChangeStateType.Left_Zero:
                return new Tests(Vector2Int.zero, new Vector2Int(-1, 0), new Vector2Int(-1, -1), 
                    new Vector2Int(0, 2), new Vector2Int(-1, 2));

            default:
                return default;
        }
    }

    public static Tests GetOffsets_I_Shape(ChangeStateType changeType)
    {
        switch (changeType)
        {
            case ChangeStateType.Zero_Right:
                return new Tests(Vector2Int.zero, new Vector2Int(-2, 0), new Vector2Int(1, 0),
                    new Vector2Int(-2, -1), new Vector2Int(1, 2));

            case ChangeStateType.Right_Half:
                return new Tests(Vector2Int.zero, new Vector2Int(-1, 0), new Vector2Int(2, 0),
                    new Vector2Int(-1, 2), new Vector2Int(2, -1));

            case ChangeStateType.Half_Left:
                return new Tests(Vector2Int.zero, new Vector2Int(2, 0), new Vector2Int(-1, 0),
                    new Vector2Int(2, 1), new Vector2Int(-1, -2));

            case ChangeStateType.Left_Zero:
                return new Tests(Vector2Int.zero, new Vector2Int(1, 0), new Vector2Int(-2, 0),
                    new Vector2Int(1, -2), new Vector2Int(-2, 1));

            default:
                return default;
        }
    }
}

public struct Tests
{
    public List<Vector2Int> tests; 
    public Tests(params Vector2Int[] testParam)
    {
        tests = new List<Vector2Int>();
        for(int i=0; i< testParam.Length; i++)       
            tests.Add(testParam[i]);        
    }
}
