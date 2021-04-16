using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 class WindowManager : MonoBehaviour
{
    Window currentWindow = null;
    int direction = -1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoSizeWindows());
    }

    IEnumerator CoSizeWindows()
    {
        yield return new WaitForEndOfFrame();

        // Store primary window
        currentWindow = GetPrimaryWindow();
        Rect currentRect = currentWindow.rect.rect;

        // Loop through each UI window and position it
        foreach(Window window in GetAllWindows(true))
        {
            window.rect.offsetMin = new Vector2(currentRect.width * window.x, currentRect.height * window.y);
            window.rect.offsetMax = new Vector2(currentRect.width * window.x, currentRect.height * window.y);
        } 
    }

    public void ScrollToWindow(int target)
    {
        // Get components
        Window targetWindow = GetWindow(target);
        RectTransform targetRect = targetWindow.rect;
        RectTransform currentRect = currentWindow.rect;

        // Set direction
        SetDirection(targetWindow);

        // Begin tween
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", currentRect.offsetMin,
            "to", targetRect.offsetMin,
            "time", 0.2f,
            "ease", iTween.EaseType.easeInOutCirc,
            "onstart", "EnableDisableButtons(false)",
            "onupdate", "UpdateScrollPosition",
            "oncomplete", "EnableDisableButtons(true)"
        ));

        // Store new current window
        currentWindow = targetWindow;  
    }

    void EnableDisableButtons(bool enable)
    {
        foreach(Button button in FindObjectsOfType<Button>())
        {
            button.enabled = enable;
        }
    }

    void UpdateScrollPosition(Vector2 target)
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMin = target * direction;
        rect.offsetMax = target * direction;
    } 

    void SetDirection(Window target)
    {
        return;
    } 

    Window GetPrimaryWindow()
    {
        return GetAllWindows(false).Where(a => a.primary == true).First();
    }  

    Window GetWindow(int target) 
    {
        return GetAllWindows(false).Where(a => a.index == target).First();
    }

    List<Window> GetAllWindows(bool notPrimary)
    {
        if(notPrimary)
        {
            return GameObject.FindGameObjectsWithTag("Window").Select(a => a.GetComponent<Window>()).Where(a => a.primary == false).ToList();
        }
        return GameObject.FindGameObjectsWithTag("Window").Select(a => a.GetComponent<Window>()).ToList();
    }
}
