using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string name;    
    private List<StickmonMove> allPossibleMoves;
    private Texture2D stickmonSprite;    

    //public Stickmon(string name, List<StickmonMove>allmoves, Texture2D stickmonSprite)
    public Stickmon(string name, List<StickmonMove> allmoves)
    {
        this.name = name;
        this.allPossibleMoves = allmoves;
        //this.stickmonSprite = stickmonSprite;
    }

    public string GetStickmonName()
    {
        return name;
    }

    //public List<StickmonMove> GetAllPossibleStickmonMoves()
    //{

    //}
}
