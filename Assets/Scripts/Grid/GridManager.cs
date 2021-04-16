using System.Linq;
using System.Collections.Generic;
using FruitBowl;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    // Game Manager
    private GameManager gameManager;

    // Fruit Manager
    public GameObject fruitLayer;
    private FruitManager fruitManager;
    
    // Cell prefab
    public GameObject cellPrefab;

    // Attached elements
    private List<GameObject> cellElements = new List<GameObject>();

    // Attributes
    private int movementCount = -1;
    private bool moveMade = false;
    private GridSettings gridSettings;

    void Awake()
    {
        // Store game manager reference
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fruitManager = fruitLayer.GetComponent<FruitManager>();
    }

    public void ClearGrid()
    {
        // Remove every attached grid cell
        cellElements = new List<GameObject>();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        // Clear all fruit
        fruitManager.ClearFruit();
    }

    public void DrawGrid()
    {
        // Clear first
        ClearGrid();

        // Get components and store settings
        RectTransform rect = GetComponent<RectTransform>();
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        gridSettings = gameManager.gameSettings.gridSettings;
        
        // Calculate cell size
        float rectWidth = rect.rect.width;
        float totalSpacing = (gridSettings.cellSpacing * gridSettings.rowsAndCols);  
        float cellSize = (rectWidth - gridSettings.gridPadding - totalSpacing) / gridSettings.rowsAndCols;
        grid.spacing = new Vector2(gridSettings.cellSpacing, gridSettings.cellSpacing);
        grid.cellSize = new Vector2(cellSize, cellSize);

        // Add grid cells to gridLayer
        for(int x = 0; x < gridSettings.rowsAndCols; x++)
        {
            for(int y = 0; y < gridSettings.rowsAndCols; y++)
            {
                GameObject newCell = Instantiate(cellPrefab, transform) as GameObject;
                newCell.GetComponent<GridCell>().gridPosition = new Vector2Int(x, y);
                cellElements.Add(newCell);
            }
        }
    }

    public void EnableRandomCells(int count, bool onlyIfMovePlayed)
    {
        // Check to only add cells if valid move was made previously
        if(onlyIfMovePlayed && !moveMade) { return; }

        // Check all inactive cells
        moveMade = false;
        for(int i = 0; i < count; i++)
        {
            // Update grid cell and create fruit over it
            GridCell randomCell = RandomCell(false).GetComponent<GridCell>();
            randomCell.UpdateCell(2);
            fruitManager.CreateFruit(randomCell);
        }
    }

    public void DisableAllCells()
    {
        // Disable every attached cell element  
        cellElements.ForEach(cell => cell.GetComponent<GridCell>().DisableCell());
    }

    public void MoveGrid(Direction direction)
    {
        // Reset counter
        movementCount = -1;

        // Move all cells in grid accordingly
        if(direction == Direction.Up || direction == Direction.Left)
        {
            for(int x = 0; x < gridSettings.rowsAndCols; x++)
            {
                for(int y = 0; y < gridSettings.rowsAndCols; y++)
                { 
                    StartCoroutine(CoMoveCell(new Vector2(x, y), direction));
                }
            }
        }
        else if(direction == Direction.Down || direction == Direction.Right)
        {
            for(int x = gridSettings.rowsAndCols - 1; x >= 0; x--)
            {
                for(int y = gridSettings.rowsAndCols - 1; y >= 0; y--)
                {
                    StartCoroutine(CoMoveCell(new Vector2(x, y), direction));
                }
            }
        }
    }

    public System.Collections.IEnumerator CoMoveCell(Vector2 cellPosition, Direction direction)
    {
        movementCount++;
        yield return new WaitForSeconds(movementCount * 0.01f);

        // Get current cell
        GameObject curCell = GetCell(cellPosition); 

        // Sanity check the cell exists and is active
        if(curCell != null)
        {
            // Enter loop to move cell as far as possible
            bool keepChecking = true;
            while(keepChecking)
            {
                // Force check to false for now
                keepChecking = false;

                // Get current grid cell component
                GridCell curGridCell = curCell.GetComponent<GridCell>();
                GameObject nextCell = GetCell(curGridCell.NextCell(direction));

                //  If we have an active cell and a cell to try move to
                if(curGridCell.cellActive && nextCell != null) 
                {
                    // Get next grid cell component
                    GridCell nextGridCell = nextCell.GetComponent<GridCell>();

                    // Check that moving to the next cell is an eligible option
                    if(!nextGridCell.DontTouch(curGridCell)) 
                    { 
                        // Move cell data into next and disable current
                        nextGridCell.UpdateCell(curGridCell.cellValue);
                        curGridCell.DisableCell();

                        // Update fruit layer
                        fruitManager.MoveFruit(curGridCell.fruitReference, nextGridCell);

                        // Override current cell with one moved into and keep checking
                        curCell = nextCell;
                        keepChecking = true;
                        moveMade = true;
                    }
                }
            }
        }
    }

    GameObject GetCell(Vector2 gridPosition)
    {
        // Returns cell at position
        if(gridPosition.x >= gridSettings.rowsAndCols || gridPosition.y >= gridSettings.rowsAndCols) { return null; }
        if(gridPosition.x < 0 || gridPosition.y < 0) { return null; }
        return cellElements.Where(cell => cell.GetComponent<GridCell>().gridPosition == gridPosition).First();
    }

    GameObject RandomCell(bool? active)
    {
        // Gets a random cell from criteria
        List<GameObject> randomCells = active.HasValue ? GetCellsByActive(active.Value) : cellElements;
        return randomCells[UnityEngine.Random.Range(0, randomCells.Count - 1)];
    }

    List<GameObject> GetCellsByActive(bool active)
    {
        // Gets all active/inactive cells
        return cellElements.Where(cell => cell.GetComponent<GridCell>().cellActive == active).ToList();
    }

    public List<GridCell> GetAllCellComponents()
    {
        // Gets all GridCell components
        return cellElements.Select(cell => cell.GetComponent<GridCell>()).ToList();
    }
}
