using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmonManager
{
    private List<StickmonMove> myStickmonMoves;
    private List<Stickmon> myStickmon;

    public StickmonManager()
    {
        myStickmonMoves = new List<StickmonMove>();
        myStickmon = new List<Stickmon>();
    }

    public void AddStickmon(Stickmon newStickmon)
    {
        myStickmon.Add(newStickmon);
    }

    public Stickmon GetRandomStickmon()
    {
        Stickmon currentStickmon =  myStickmon[Random.Range(0, myStickmon.Count)];
        return currentStickmon;
    }

    #region StickmonMoves

    public void AddStickmonMove(StickmonMove newStickmonMove)
    {
        myStickmonMoves.Add(newStickmonMove);
    }

    public List<StickmonMove> GetAllStickmonMoves()
    {
        return myStickmonMoves;
    }

    #endregion
}
