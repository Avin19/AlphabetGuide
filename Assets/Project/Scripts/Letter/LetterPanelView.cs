using UnityEngine;
using UnityEngine.UI;

public class LetterPanelView : MonoBehaviour
{
    public Image letterImage;

    public void ShowLetter(LetterTraceData data)
    {
        if (data == null || letterImage == null)
            return;

        letterImage.sprite = data.letterSprite;

    }
}
