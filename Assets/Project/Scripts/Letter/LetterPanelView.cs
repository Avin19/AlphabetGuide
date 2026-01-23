using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LetterPanelView : MonoBehaviour
{
    [SerializeField] private Image letterTrace;
    [SerializeField] private Image letterTop;

    void OnEnable()
    {
        letterTrace.sprite = SelectedLetterHolder.current.strokeData.letterSprite;
        letterTop.sprite = SelectedLetterHolder.current.strokeData.letterSprite;

    }



}
