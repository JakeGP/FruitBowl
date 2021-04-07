using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FruitBowl;

public class GameManager : MonoBehaviour
{
    // Gameplay managers
    public GameObject grid;
    private GridManager gridManager;
    private InputControl inputControl;

    // Settings
    public GameSettings gameSettings;
    public bool debug = false;
    private bool inputEnabled = true;

    void Awake()
    {
        // Bind grid manager
        gridManager = grid.GetComponent<GridManager>();

        // Bind touch input
        inputControl = GetComponent<InputControl>();
        inputControl.swipeEvent.AddListener(MoveGrid);
    }

    void Start()
    {
        InitialiseGame();
    }

    public void InitialiseGame()
    {
        // Create the grid and place the start cells
        gridManager.DrawGrid();
        gridManager.EnableRandomCells(gameSettings.startCells, false);
    }

    public void ResetGame()
    {
        // Completely clear the grid
        gridManager.ClearGrid();
        InitialiseGame();
    }

    public void MoveGrid(Direction direction)
    {
        // Move the grid elements if input is enabled
        if(inputEnabled)
        {
            // Disable input and run coroutine to spawn new cells
            inputEnabled = false;
            gridManager.MoveGrid(direction);  
            StartCoroutine(CoEnableNewCells());
        }
    }

    public IEnumerator CoEnableNewCells()
    {
        // Spawn new cells and re-enable input
        yield return new WaitForSeconds(0.2f);
        gridManager.EnableRandomCells(gameSettings.cellsAddedPerMove, true);
        inputEnabled = true;
    }
}
