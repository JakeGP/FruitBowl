using System.Linq;
using System.Collections.Generic;
using FruitBowl;
using UnityEngine;
using UnityEngine.UI;

public class FruitManager : MonoBehaviour
{
    // Game Manager
    private GameManager gameManager;
 
    // Fruit prefabs
    public GameObject debugPrefab;
    public GameObject applePrefab;
    public GameObject orangePrefab;
    public GameObject lemonPrefab;

    // Attached elements
    private List<GameObject> fruitElements = new List<GameObject>();

    // Attributes
    private GameFruitData fruitSettings;

    void Awake()
    {
        // Store game manager reference
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fruitSettings = gameManager.gameSettings.fruitSettings;
    }

    public void ClearFruit()
    {
        foreach(GameObject fruit in fruitElements)
        {
            DestroyImmediate(fruit);
        }
    }

    public void CreateFruit(GridCell cellReference)
    {
        // Add new fruit element
        GameObject newFruit = Instantiate(GetNewFruit(), transform) as GameObject;
        fruitElements.Add(newFruit);
        
        // Set position and add relationship references
        Fruit newFruitComponent = newFruit.GetComponent<Fruit>();
        newFruitComponent.UpdateFruit(true, cellReference);
    }

    public void MoveFruit(Fruit fruitReference, GridCell newCellReference)
    {
        // Update fruit position and data
        fruitReference.UpdateFruit(false, newCellReference);
    }

    private GameObject GetNewFruit()
    {   
        switch(fruitSettings.allowedFruits[UnityEngine.Random.Range(0, fruitSettings.allowedFruits.Count - 1)])
        {
            case FruitType.Lemon: return lemonPrefab;
            case FruitType.Apple: return applePrefab;
            case FruitType.Orange: return orangePrefab;
        }
        return debugPrefab;
    }
}
