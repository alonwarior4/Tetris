using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;
using System;

using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float tileMoveDelay;
    [SerializeField] ShapeConfig shapeConfig;

    [Header("Refrences")]
    [SerializeField] GridMap gridMap;    
    Shape currentShape;
    string nextShapeName;

    //Shape nextShape;
    [SerializeField] SpriteRenderer nextShapeSprite;

    CancellationToken token;
    List<int> blockedInexes = new List<int>();

    [Header("shape Sprites")]
    [SerializeField] Sprite I_Sprite;
    [SerializeField] Sprite L_Sprite;
    [SerializeField] Sprite J_Sprite;
    [SerializeField] Sprite O_Sprite;
    [SerializeField] Sprite S_Sprite;
    [SerializeField] Sprite T_Sprite;
    [SerializeField] Sprite Z_Sprite;

    [Header("UI Refrence")]
    int totalScore = 0;
    [SerializeField] int LineScore;
    [SerializeField] TextMeshProUGUI scoreText;

    string[] shapeNames = { "I", "L", "J", "O", "S", "T", "Z" };

    GridMesh gridMesh;
    RandomSelector randomSelector;

    private void Awake()
    {
        token = this.GetCancellationTokenOnDestroy();
        gridMesh = gridMap.GetComponent<GridMesh>();
    }

    private async void Start()
    {
        randomSelector = new SingleBagRandom();

        gridMap.GenerateGridMap();
        scoreText.text = totalScore.ToString();

        SetNewRandomShape();
        //nextShapeName = shapeNames[Random.Range(0, shapeNames.Length)];
        nextShapeName = randomSelector.GetNextTetromino();
        nextShapeSprite.sprite = shapeConfig.GetShapeSprite(nextShapeName);

        try
        {
            await MoveCurrentShape();
        }
        catch (Exception ex) { return; }


        while (true)
        {
            try
            {
                await SelectAndMoveShape();
            }
            catch (Exception ex) { return; }
        }
    }

    async UniTask SelectAndMoveShape()
    {
        ConfigCurrentShapeByName(nextShapeName , true);

        //nextShapeName = shapeNames[Random.Range(0, shapeNames.Length)];
        nextShapeName = randomSelector.GetNextTetromino();
        nextShapeSprite.sprite = shapeConfig.GetShapeSprite(nextShapeName);

        await MoveCurrentShape();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int testXPos = currentShape.gridPos.x - 1;            
            if (!isValidPos(currentShape.shape, new Vector2Int(testXPos, currentShape.gridPos.y))) return;

            currentShape.gridPos.x--;

            ShowShape();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            int testXPos = currentShape.gridPos.x + 1;            
            if (!isValidPos(currentShape.shape, new Vector2Int(testXPos, currentShape.gridPos.y))) return;

            currentShape.gridPos.x++;

            ShowShape();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            TryRotateCurrentShape();
    }

    void TryRotateCurrentShape()
    {
        if (currentShape.name == "O") return;

        int[,] a = new int[currentShape.shape.GetLength(0), currentShape.shape.GetLength(1)];
        
        for(int j = 0; j< currentShape.shape.GetLength(1); j++)
        {
            for(int i=0; i< currentShape.shape.GetLength(0); i++)
            {
                a[i, j] = currentShape.shape[i, j];
            }           
        }

        int size = a.GetLength(0);

        for (int i = 0; i < size / 2; i++)
        {
            for (int j = i; j < size - i - 1; j++)
            {
                int temp = a[i, j];
                a[i, j] = a[j, size - 1 - i];
                a[j , size - 1 - i] = a[size - 1 - i, size - 1 - j];
                a[size - 1 - i, size - 1 - j] = a[size - 1 - j ,i];
                a[size - 1 - j, i] = temp;
            }
        }

        if (isValidPos(a , currentShape.gridPos))
        {
            currentShape.ShapeRotated();
            currentShape.shape = a;
            ShowShape();
            return;
        }

        ChangeStateType nextRotChange = ChangeStateType.Zero_Right;        

        switch (currentShape.rotState)
        {
            case RotState.Zero:
                nextRotChange = ChangeStateType.Zero_Right;
                break;

            case RotState.Right:
                nextRotChange = ChangeStateType.Right_Half;
                break;

            case RotState.Half:
                nextRotChange = ChangeStateType.Half_Left;
                break;

            case RotState.Left:
                nextRotChange = ChangeStateType.Left_Zero;
                break;
        }

        Tests offsets = default;

        if (currentShape.name != "I")
            offsets = RotationTable.GetOffsetsCommonShapes(nextRotChange);
        else
            offsets = RotationTable.GetOffsets_I_Shape(nextRotChange);
        
        for(int i=0; i< offsets.tests.Count; i++)
        {
            Vector2Int testPos = currentShape.gridPos + offsets.tests[i];
            if (isValidPos(a , testPos))
            {
                currentShape.ShapeRotated();
                currentShape.gridPos += offsets.tests[i];
                currentShape.shape = a;
                ShowShape();
                break;
            }
        }
    }

    async UniTask MoveCurrentShape()
    {
        currentShape.gridPos.y = gridMap.y - 2;
        currentShape.gridPos.x = gridMap.x / 2 - currentShape.shape.GetLength(0) / 2;

        if (isValidPos(currentShape.shape, currentShape.gridPos))
            ShowShape();
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        while (true)
        {
            await UniTask.Delay((int)(tileMoveDelay * 1000), cancellationToken: token);
            int testYPos = currentShape.gridPos.y - 1;

            if (!isValidPos(currentShape.shape, new Vector2Int(currentShape.gridPos.x, testYPos)))
            {
                AddToBlockList();
                CheckLines();
                return;
            }

            currentShape.gridPos.y--;
            ShowShape();
        }
    }

    bool isValidPos(int[,] matrix , Vector2Int pos)
    {
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, j] == 1)
                {
                    int x = pos.x + i;
                    int y = pos.y - j;

                    if (!gridMap.IsValidIndex(x, y)) return false;
                    if (gridMap.Cells[x, y].isBlocked) return false;
                }
            }
        }

        return true;
    }

    void AddToBlockList()
    {
        for(int j = 0; j< currentShape.shape.GetLength(1); j++)
        {
            for(int i=0; i < currentShape.shape.GetLength(0); i++)
            {
                if (currentShape.shape[i, j] == 1)
                {
                    int xPos = currentShape.gridPos.x + i;
                    int yPos = currentShape.gridPos.y - j;
                    blockedInexes.Add(xPos + (yPos * gridMap.x));
                    gridMap.Cells[xPos, yPos].isBlocked = true;
                }
            }
        }
    }

    void CheckLines()
    {
        int gridYSize = gridMap.y;
        int gridXSize = gridMap.x;
        int CompleteLineCount = 0;

        for(int j = gridYSize - 2; j>=1; j--)
        {
            //Check Completed Lines from top to bot
            bool isCompleteLine = true;
            for(int i = 1; i<= gridXSize - 2; i++)
            {
                if (!gridMap.Cells[i, j].isBlocked)
                {
                    isCompleteLine = false;
                    break;
                }                    
            }

            if (!isCompleteLine) continue;
            CompleteLineCount++;

            //Remove Completed Line
            for (int i = 1; i <= gridXSize - 2; i++)
            {
                int index = i + j * gridMap.x;
                blockedInexes.Remove(index);
            }

            //Shift Above Completed Line One Row Down
            CellConfig[,] gridPart = new CellConfig[gridXSize - 2, gridYSize - 1 - j];
            for (int k = j + 1; k < gridYSize - 1; k++)
            {
                for (int i = 1; i < gridXSize - 1; i++)
                {
                    Cell gridCell = gridMap.Cells[i, k];
                    CellConfig cc = new CellConfig(gridCell.isBlocked, gridCell.cellColor);
                    gridPart[i - 1, k - j - 1] = cc;

                    if (gridCell.isBlocked)
                    {
                        int index = i + (k * gridMap.x);
                        blockedInexes.Remove(index);
                    }                    
                }
            }

            for (int k = j; k < gridYSize - 1; k++)
            {
                for (int i = 1; i < gridXSize - 1; i++)
                {
                    gridMap.Cells[i, k].isBlocked = gridPart[i - 1, k - j].isBlock;
                    gridMap.Cells[i, k].cellColor = gridPart[i - 1, k - j].cellColor;

                    if (gridMap.Cells[i, k].isBlocked)
                    {
                        int index = i + (k * gridMap.x);
                        blockedInexes.Add(index);
                    }
                }
            }
        }

        totalScore += CompleteLineCount * LineScore;
        scoreText.text = totalScore.ToString();
    }

    void ShowShape()
    {
        int xSize = currentShape.shape.GetLength(0);
        int ySize = currentShape.shape.GetLength(1);

        gridMap.Refresh();

        //Set Block
        for (int i = 0; i < blockedInexes.Count; i++)
        {
            int y = blockedInexes[i] / gridMap.x;
            int x = blockedInexes[i] - y * gridMap.x;            
            gridMap.Cells[x, y].isBlocked = true;            
        }

        //Set Shape
        for (int j = 0; j < ySize; j++)
        {
            for (int i = 0; i < xSize; i++)
            {
                int xPos = currentShape.gridPos.x + i;
                int yPos = currentShape.gridPos.y - j;
                if (gridMap.IsValidIndex(xPos, yPos) && !gridMap.Cells[xPos, yPos].isBlocked)
                {
                    gridMap.Cells[xPos, yPos].isShape = currentShape.shape[i, j] == 1 ? true : false;
                    gridMap.Cells[xPos, yPos].cellColor = currentShape.shape[i , j] == 1 ? currentShape.shapeColor : Color.white;
                }
            }
        }

        gridMesh.ReColor();
    }

    void SetNewRandomShape()
    {
        //string shapeName = shapeNames[Random.Range(0, shapeNames.Length)];
        string shapeName = randomSelector.GetNextTetromino();
        ConfigCurrentShapeByName(shapeName, false);
    }    

    void ConfigCurrentShapeByName(string shapeName , bool isReconfig)
    {
        int[,] shapeMap = shapeConfig.GetShapeMatrix(shapeName);
        Color shapeColor = shapeConfig.GetShapeColor(shapeName);

        if (isReconfig)
            currentShape.ReConfigShape(shapeName, shapeMap, shapeColor);
        else
            currentShape = new Shape(shapeName, shapeMap, shapeColor);
    }

}

public struct CellConfig
{
    public bool isBlock;
    public Color cellColor;

    public CellConfig(bool isBlock , Color cellColor)
    {
        this.isBlock = isBlock;
        this.cellColor = cellColor;
    }
}
