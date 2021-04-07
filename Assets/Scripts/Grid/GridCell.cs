using System.Collections;
using System.Collections.Generic;
using FruitBowl;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    // Game Manager
    private GameManager gameManager;

    // Cell information
    public int cellValue = 0;
    public Vector2Int gridPosition = Vector2Int.zero;
    public bool cellActive = false;

    // Text elements
    Text value;
    Text position;

    // Fruit reference
    public Fruit fruitReference;

    void Awake() 
    {
        // Store game manager reference
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        value = transform.Find("Label").GetComponentInChildren<Text>();
        position = transform.Find("Position").GetComponentInChildren<Text>();
    }

    void Start()
    {
    }

    void Update()
    {    
        UpdateText();
    }

    void UpdateText()
    {
        string targetValue = gameManager.debug ? cellValue.ToString() : "";
        string targetPosition = gameManager.debug ? gridPosition.ToString() : "";
        
        if(value.text != targetValue) { value.text = targetValue; }
        if(position.text != targetPosition) { position.text = targetPosition; }
    }

    public void UpdateCell(int value)
    {
        cellActive = value > 0;
        cellValue += value;
    }

    public void DisableCell()
    {
        cellActive = false;
        cellValue = 0;
    }

    public Vector2 GetAnchoredPosition()
    {
        return GetComponent<RectTransform>().anchoredPosition;
    }

    public Vector2 GetAnchoredSize()
    {
        return GetComponent<RectTransform>().sizeDelta;
    }

    public Vector2 NextCell(Direction direction)
    {
        float nextX = gridPosition.x;
        float nextY = gridPosition.y;
        if(direction == Direction.Up || direction == Direction.Down)
        {
            nextY += direction == Direction.Up ? -1 : 1;
        }
        if(direction == Direction.Left || direction == Direction.Right)
        {
            nextX += direction == Direction.Left ? -1 : 1;
        }
        return new Vector2(nextX, nextY);
    }

    public bool DontTouch(GridCell incomingCell)
    {
        return cellActive && incomingCell.cellValue != cellValue;
    }
}