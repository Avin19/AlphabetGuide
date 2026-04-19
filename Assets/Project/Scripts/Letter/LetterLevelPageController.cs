using System;
using UnityEngine;
using UnityEngine.UI;
using HutongGames.PlayMaker;

public class LetterLevelPageController : MonoBehaviour
{
    [SerializeField] private LetterProgressDatabase database;
    [SerializeField] private PlayMakerFSM levelFSM;

    void OnEnable()
    {
        LetterProgressSaveSystem.Load(database);
        BuildUI();

    }

    public void BuildUI()
    {
        levelFSM.FsmVariables.GetFsmInt("numberOfLetter").Value = database.letters.Count;
        //Debug.Log(database.letters.Count);


    }
    public void LoadLetterData(int index)
    {
        levelFSM.FsmVariables.GetFsmObject("letters").Value = database.letters[index].strokeData;
    }

    public void LetterData(int index)
    {
        levelFSM.FsmVariables.GetFsmGameObject("createObject").Value.GetComponent<Image>().sprite = database.letters[index].strokeData.letterSprite;
        // levelFSM.FsmVariables.GetFsmGameObject("createObject").Value.GetComponent<StarEarned>().ActivteStar(database.letters[index].starsEarned);
        levelFSM.FsmVariables.GetFsmInt("currectLetterStar").Value = database.letters[index].starsEarned;
    }




}
