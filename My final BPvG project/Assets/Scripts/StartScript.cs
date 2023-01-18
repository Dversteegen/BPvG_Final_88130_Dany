using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class StartScript : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] Sprite larrySprite;
    [SerializeField] Sprite paulSprite;
    [SerializeField] Sprite griffinSprite;
    [SerializeField] Sprite julianSprite;

    private void Start()
    {

    }

    #region SetupFunctions

    public void SetupData()
    {
        AddAllStickmonMoves();
        AddAllStickmons();
        AddFirstStickmon();
        SceneManager.LoadScene("StartScene");
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        SetupData();
    }

    public void ContinueGame()
    {
        SetupData();
        //Maybe a function to get the data from playerprefs
    }

    #region AddFunctions

    private void AddAllStickmonMoves()
    {
        StickmonMove newStickmonMove = new StickmonMove("Punch", 4);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Kick", 5);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Push", 3);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Beat up", 6);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Break", 8);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Squash", 7);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Hit brick", 4);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);
    }

    private void AddAllStickmons()
    {
        Stickmon newStickmon = new Stickmon("Larry", 12, larrySprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Julian", 9, julianSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Griffin", 10, griffinSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Paul", 8, paulSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);
    }

    private void AddFirstStickmon()
    {
        Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();
        float maxHealthPoints = firstStickmon.GetBaseHealthPoints();

        for (int count = 0; count < 2; count++)
        {
            int randomNumber = Random.Range(0, 2);

            if (randomNumber == 0)
            {
                maxHealthPoints++;
            }
            else if (randomNumber == 2)
            {
                maxHealthPoints += 2;
            }
        }

        StickmonMove randomStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStandardStickmonMove();
        List<StickmonMove> allCurrentMoves = new List<StickmonMove>();
        allCurrentMoves.Add(randomStickmonMove);
        randomStickmonMove = GameManagerScript.myGameManagerScript.GetRandomNewStickmonMove(randomStickmonMove);
        allCurrentMoves.Add(randomStickmonMove);

        int[] allLevelBarriers = new int[] { 5, 11, 18, 26, 35, 47, 61, 78, 98, 123, 153 };
        int level = 3;        

        for(int count = 0; count < level - 1; count++)
        {
            allLevelBarriers = allLevelBarriers.Where(levelBarrier => levelBarrier != allLevelBarriers[0]).ToArray();
        }

        CurrentStickmon newAlliedStickmon = new CurrentStickmon(firstStickmon.GetStickmonName(), 3, 11, maxHealthPoints, allCurrentMoves, julianSprite, allLevelBarriers);
        //CurrentStickmon newAlliedStickmon = new CurrentStickmon(firstStickmon.GetStickmonName(), 3, 11, 1, allCurrentMoves, julianSprite, allLevelBarriers);
        GameManagerScript.myGameManagerScript.AddFirstStickmon(newAlliedStickmon);
    }

    #endregion

    #endregion

}
