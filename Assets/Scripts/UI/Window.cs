using UnityEngine;

public class Window : MonoBehaviour 
{
    public bool primary = false;
    public int index = 0;
    public int x = 0;
    public int y = 0;

    [HideInInspector]
    public RectTransform rect;

    public void Awake()
    {
        this.rect = GetComponent<RectTransform>();
    }   
}