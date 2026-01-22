using UnityEngine;
using UnityEngine.UI;

public class LetterPanelView : MonoBehaviour
{
    [SerializeField] private Image letterTrace;
    [SerializeField] private Image letterTop;

    public void ShowLetter(LetterTraceData data)
    {
        if (data == null || letterTrace == null)
            return;

        letterTrace.sprite = data.letterSprite;
        letterTop.sprite = data.letterSprite;

    }
}
