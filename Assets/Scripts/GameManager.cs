using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FruitBowl;

public class GameManager : MonoBehaviour
{
    // Grid element
    private GameObject grid;

    // Gameplay managers
    private GridManager gridManager;
    private InputControl inputControl;
    private ScoreManager scoreManager;

    // Settings
    public GameSettings gameSettings;
    public bool debug = false;
    private bool inputEnabled = true;

    public IEnumerator BeginGame()
    {
        // Create the grid and place the start cells
        yield return new WaitForEndOfFrame();

        // Bind grid manager
        grid = GameObject.FindGameObjectWithTag("Grid");
        gridManager = grid.GetComponent<GridManager>();

        // Bind touch input
        inputControl = GetComponent<InputControl>();
        inputControl.swipeEvent.AddListener(MoveGrid);

        // Bind score manager
        scoreManager = GetComponent<ScoreManager>();
        scoreManager.InitialiseScore();

        // Create game
        StartCoroutine(InitialiseGame());
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetGame()
    {
        // Completely clear the grid
        gridManager.ClearGrid();
        scoreManager.ResetTime();
        StartCoroutine(InitialiseGame());
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

    IEnumerator InitialiseGame()
    {
        yield return new WaitForEndOfFrame(); 
        gridManager.DrawGrid();
        gridManager.EnableRandomCells(gameSettings.startCells, false);
        scoreManager.UpdateScore(gridManager);
    }

    IEnumerator CoEnableNewCells()
    {
        // Spawn new cells and re-enable input
        yield return new WaitForSeconds(0.2f);
        gridManager.EnableRandomCells(gameSettings.cellsAddedPerMove, true);
        scoreManager.UpdateScore(gridManager);
        inputEnabled = true;
    }
}
