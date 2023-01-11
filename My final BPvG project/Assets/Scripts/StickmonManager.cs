using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StickmonManager
{
    private List<StickmonMove> myStickmonMoves;
    private List<Stickmon> myStickmon;
    private List<CurrentStickmon> myAlliedStickmon;

    public StickmonManager()
    {
        myStickmonMoves = new List<StickmonMove>();
        myStickmon = new List<Stickmon>();
        myAlliedStickmon = new List<CurrentStickmon>();
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
        Stickmon currentStickmon =  myStickmon[Random.Range(0, myStickmon.Count)];
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

    

    public List<CurrentStickmon> GetAllAlliedStickmon()
    {
        return myAlliedStickmon;
    }

    #endregion
}
