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

    #region AddAlliedStickmon

    public void AddFirstStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    public void AddAlliedStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    #endregion

    #region GetFunctions

    public CurrentStickmon GetFirstAlliedStickmon()
    {
        return myAlliedStickmon[0];
    }

    /// <summary>
    /// Returns the first healthy Stickmon
    /// </summary>
    /// <returns></returns>
    public CurrentStickmon GetFirstHealthyAllliedStickmon()
    {
        foreach (CurrentStickmon alliedStickmon in myAlliedStickmon)
        {
            if (alliedStickmon.GetCurrentHealthPoints() > 0)
            {
                return alliedStickmon;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns all allied Stickmon
    /// </summary>
    /// <returns></returns>
    public List<CurrentStickmon> GetAllAlliedStickmon()
    {
        return myAlliedStickmon;
    }

    /// <summary>
    /// Returns the amount of healthy Stickmon
    /// </summary>
    /// <returns></returns>
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

    #endregion

    #region Methods

    public void HealAllAlliedStickmon()
    {
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            currentStickmon.HealStickmon();
        }        
    }

    public void SwapStickmonPositions(CurrentStickmon currentFirstStickmon, string position)
    {
        if (position == "second")
        {
            myAlliedStickmon[0] = myAlliedStickmon[1];
            myAlliedStickmon[1] = currentFirstStickmon;
        }
        else
        {
            myAlliedStickmon[0] = myAlliedStickmon[2];
            myAlliedStickmon[2] = currentFirstStickmon;
        }        
    }

    #endregion

    #endregion
}
