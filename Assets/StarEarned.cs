using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarEarned : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] earnsprite;
    private void Start()
    {
        for (int i = 0; i <= 2; i++)
        {
            earnsprite[i].SetActive(false);
        }
    }
    public void ActivteStar(int earnStar)
    {
        for (int i = 0; i <= earnStar; i++)
        {
            earnsprite[i].SetActive(true);
        }
    }
}
