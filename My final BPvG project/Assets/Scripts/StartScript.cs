using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{    
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

        newStickmonMove = new StickmonMove("Pray", 0);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Sleep", 0);
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
        Stickmon newStickmon = new Stickmon("Larry", 12);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Julian", 9);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Griffin", 10);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Paul", 8);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);
    }

    private void AddFirstStickmon()
    {
        Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();
        float maxHealthPoints = firstStickmon.GetBaseHealthPoints();

        StickmonMove randomStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStandardStickmonMove();
        List<StickmonMove> allCurrentMoves = new List<StickmonMove>();
        allCurrentMoves.Add(randomStickmonMove);

        CurrentStickmon newAlliedStickmon = new CurrentStickmon(firstStickmon.GetStickmonName(), 1, 0, maxHealthPoints, allCurrentMoves);
        GameManagerScript.myGameManagerScript.AddFirstStickmon(newAlliedStickmon);
    }

    #endregion

    #endregion

}
