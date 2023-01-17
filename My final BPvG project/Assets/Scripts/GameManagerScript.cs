using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript myGameManagerScript;

    private List<StickmonMove> myStickmonMoves;
    private List<Stickmon> myStickmon;
    private List<CurrentStickmon> myAlliedStickmon;    

    public GameManagerScript()
    {
        myStickmonMoves = new List<StickmonMove>();
        myStickmon = new List<Stickmon>();
        myAlliedStickmon = new List<CurrentStickmon>();
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        MakeThisTheOnlyGameManager();
    }    

    void MakeThisTheOnlyGameManager()
    {
        if (myGameManagerScript == null)
        {
            DontDestroyOnLoad(gameObject);
            myGameManagerScript = this;
        }
        else
        {
            if (myGameManagerScript != this){
                Destroy(gameObject);
            }
        }
    }    

    #region Stickmon

    public void AddStickmon(Stickmon newStickmon)
    {
        myStickmon.Add(newStickmon);
    }

    public Stickmon GetFirstStickmon()
    {
        foreach (Stickmon stickmon in myStickmon)
        {
            if (stickmon.GetStickmonName() == "Julian")
            {
                return stickmon;
            }
        }
        return null;
    }

    public Stickmon GetRandomStickmon()
    {
        List<Stickmon> allOpponentStickmon = myStickmon.Where(stickmon => stickmon.GetStickmonName() != "Julian").ToList();

        Stickmon currentStickmon = allOpponentStickmon[Random.Range(0, allOpponentStickmon.Count)];
        return currentStickmon;
    }

    #endregion

    #region StickmonMoves

    public void AddStickmonMove(StickmonMove newStickmonMove)
    {
        myStickmonMoves.Add(newStickmonMove);
    }

    public StickmonMove GetRandomStandardStickmonMove()
    {
        List<StickmonMove> allPossibleMoves = myStickmonMoves.Where(move => move.GetAmountOfDamage() > 0 && move.GetAmountOfDamage() < 6).Select(move => move).ToList();

        return allPossibleMoves[Random.Range(0, allPossibleMoves.Count)];
    }

    public StickmonMove GetRandomNewStickmonMove(StickmonMove firstStickmonMove)
    {
        List<StickmonMove> allPossibleMoves = myStickmonMoves.Where(move => move != firstStickmonMove && move.GetAmountOfDamage() >= 6).Select(move => move).ToList();

        return allPossibleMoves[Random.Range(0, allPossibleMoves.Count)];
    }

    public List<StickmonMove> GetAllStickmonMoves()
    {
        return myStickmonMoves;
    }

    #endregion

    #region AlliedStickmon

    public void AddFirstStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    public void AddAlliedStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    public CurrentStickmon GetFirstAlliedStickmon()
    {
        return myAlliedStickmon[0];
    }

    public List<CurrentStickmon> GetAllAlliedStickmon()
    {
        return myAlliedStickmon;
    }

    public int GetAmountOfHealthyStickmon()
    {
        int amountOfHealthyStickmon = 0;
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            if (currentStickmon.GetCurrentHealthPoints() > 0)
            {
                amountOfHealthyStickmon++;
            }
        }
        return amountOfHealthyStickmon;
    }

    public List<CurrentStickmon> GetAllHealthyStickmon()
    {
        List<CurrentStickmon> allHealthyStickmon = new List<CurrentStickmon>();
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            if (currentStickmon.GetCurrentHealthPoints() > 0)
            {
                allHealthyStickmon.Add(currentStickmon);
            }
        }
        return allHealthyStickmon;
    }

    public void HealAllAlliedStickmon()
    {
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            currentStickmon.HealStickmon();
        }        
    }

    #endregion

    //#region Encounter

    //public void DefineCurrentEncounter(Encounter currentEncounter)
    //{
    //    _currentEncounter = currentEncounter;
    //}

    ////public bool SubtractDamage(float amountOfDamage)
    ////{

    ////} 

    //#endregion
}
