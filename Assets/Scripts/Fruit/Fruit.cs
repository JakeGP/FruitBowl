using System.Collections;
using FruitBowl;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{
    // References
    public GridCell curCellReference;

    // Fruit data
    public FruitType fruitType;
    public FruitSize fruitSize;

    // Fruit assets
    public Sprite quarter;
    public Sprite half;
    public Sprite slice;
    public Sprite whole;
    public Sprite bunch;

    // Positioning
    private RectTransform rect;
    private Image image;
    private bool isUpgrade = false;
    private Vector3 targetScale;

    void Awake()
    {
        // Get component references
        rect = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
    }

    public void UpdateFruit(bool newFruit, GridCell newCellReference)
    { 
        // If new fruit then hide
        if(newFruit)
        {
            targetScale = rect.localScale;
            rect.localScale = Vector3.zero;
        }
        
        // Clear current fruit reference
        if(curCellReference != null) { curCellReference.fruitReference = null; }
        if(newCellReference.fruitReference != null) 
        { 
            isUpgrade = true;
            StartCoroutine(CoDestroyFruit(newCellReference.fruitReference)); 
        }

        // Update relationship between us and new cell
        curCellReference = newCellReference;
        curCellReference.fruitReference = this;

        // Animate fruit from coroutine (because of grid fuckery)
        StartCoroutine(CoAnimateFruit(newFruit));
    }

    public IEnumerator CoDestroyFruit(Fruit replacingFruit)
    { 
        yield return new WaitForSeconds(0.1f);
        if(replacingFruit != null)
        {
            Destroy(replacingFruit.gameObject);
        }
    }

    public IEnumerator CoAnimateFruit(bool newFruit)
    {
        yield return new WaitForEndOfFrame();

        // Match size of target cell
        rect.sizeDelta = curCellReference.GetAnchoredSize() * 2;

        if(this != null && curCellReference != null) 
        {
            // Check how to animate
            if(newFruit)
            {
                // Set attributes
                UpdateSprite();
                UpdatePosition(curCellReference.GetAnchoredPosition());
                
                // Animate new fruit in
                iTween.ValueTo(gameObject, iTween.Hash(
                    "from", Vector3.zero,
                    "to", targetScale,
                    "time", 0.1f,
                    "ease", iTween.EaseType.easeOutBounce,
                    "onupdate", "UpdateScale"
                ));
            }
            else
            {
                // Animate existing fruit to position
                iTween.ValueTo(gameObject, iTween.Hash(
                    "from", rect.anchoredPosition, 
                    "to", curCellReference.GetAnchoredPosition(), 
                    "time", 0.1f, 
                    "ease", iTween.EaseType.easeOutCirc, 
                    "onupdate", "UpdatePosition",
                    "oncomplete", "AnimateUpgrade"
                ));
            }
        }
    }

    void AnimateUpgrade()
    {
        if(isUpgrade)
        {
            isUpgrade = false; 

            // Animate popping in of new fruit
            iTween.PunchScale(gameObject, iTween.Hash(
                "amount", new Vector3(0.4f, 0.4f, 0.0f), 
                "time", 1f,
                "onstart", "UpdateSprite"));
        }
    }

    void UpdateSprite()
    {
        // Apply new sprite 
        if((int)fruitSize != curCellReference.cellValue)
        {
            fruitSize = (FruitSize)curCellReference.cellValue;
            image.sprite = GetSprite();
        }
    }

    void UpdatePosition(Vector2 newPosition)
    {
        rect.anchoredPosition = newPosition;
    }

    void UpdateScale(Vector3 newScale)
    {
        rect.localScale = newScale;
    }

    private Sprite GetSprite()
    {
        switch(fruitSize)
        {
            case FruitSize.Quarter: return quarter;
            case FruitSize.Half:    return half;
            case FruitSize.Slice:   return slice;
            case FruitSize.Whole:   return whole;
            case FruitSize.Bunch:   return bunch;
        }
        return slice;
    }
}
