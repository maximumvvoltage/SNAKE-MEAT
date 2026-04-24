using UnityEngine;

/// <summary>
/// Central coordinator. Attach to a persistent GameObject in your scene.
/// Wire up all controllers in the Inspector.
/// </summary>
public class CutsceneController : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private ScreenController screenController;
    [SerializeField] private CursorController cursorController;
    

    private bool cursorSwapped = false;

    public void OnDrawingStageComplete(int stageIndex)
    {
        switch (stageIndex)
        {
            case 0:
                TriggerCursorSwap();
                break;

            case 1:
                Debug.Log("Everything Complete");
                break;
        }
    }
    
    public void TriggerCursorSwap()
    {
        if (cursorSwapped) return;
        cursorSwapped = true;
        cursorController.SwapToAltCursor();
    }

    public void ShowSubtitle(string text)
    {
        screenController.ShowSubtitle(text);
    }

    public void ClearSubtitle()
    {
        screenController.ClearSubtitle();
    }

    public void FadeOutOverlay(float duration = 1f)
    {
        screenController.FadeOutOverlay(duration);
    }
}