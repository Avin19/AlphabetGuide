using UnityEngine;
using UnityEngine.UI;

public class BgManager : MonoBehaviour
{
    [SerializeField] private Sprite[] bgImages;
    [SerializeField] private Image imageHolder;


    void Start()
    {
        imageHolder.sprite = bgImages[Random.Range(0, bgImages.Length - 1)];
    }
}
