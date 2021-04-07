using FruitBowl;
using UnityEngine;
using UnityEngine.Events;

public class InputControl : MonoBehaviour
{
    // Events
    [HideInInspector]
    public UnityEvent<Direction> swipeEvent;

    // Touch information
    private Vector2 touchStart;
    private float swipeThreshold = 300.0f;
    
    // Update is called once per frame
    void Update()
    {
        DetectSwipeEvents();
        DetectKeyboardEvents();
    }

    // Detects swipe events (single finger only)
    void DetectSwipeEvents()
    {
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                Debug.Log("Touch Started");
            }
            if(touch.phase == TouchPhase.Ended)
            {
                // Check direction
                if((touch.position.x - touchStart.x) >= swipeThreshold)
                {
                    DoSwipeEvent(Direction.Right);
                }
                if((touch.position.x - touchStart.x) <= -swipeThreshold)
                {
                    DoSwipeEvent(Direction.Left);
                }   
                if((touch.position.y - touchStart.y) >= swipeThreshold)
                {
                    DoSwipeEvent(Direction.Up);
                }
                if((touch.position.y - touchStart.y) <= -swipeThreshold)
                {
                    DoSwipeEvent(Direction.Down);
                }
            }

        }
    }

    void DetectKeyboardEvents()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        { 
            DoSwipeEvent(Direction.Up); 
        }
        if (Input.GetKeyDown(KeyCode.A))
        { 
            DoSwipeEvent(Direction.Left); 
        }
        if (Input.GetKeyDown(KeyCode.S)) 
        { 
            DoSwipeEvent(Direction.Down); 
        }
        if (Input.GetKeyDown(KeyCode.D)) 
        { 
            DoSwipeEvent(Direction.Right); 
        }
    }

    void DoSwipeEvent(Direction direction)
    {                    
        swipeEvent.Invoke(direction);
        Debug.Log("Swiped " + direction.ToString());
    }
}