using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string name;
    private List<StickmonMove> allMoves;
    public Texture2D stickmonSprite;
    //private int level;
    //private int experiencePoints;
    //private StickmonMoves[] currentStickmonMoves = new StickmonMoves[1];

    //public Stickmon(string name, List<StickmonMove>allmoves, Texture2D stickmonSprite)
    public Stickmon(string name, List<StickmonMove> allmoves)
    {
        this.name = name;
        this.allMoves = allmoves;
        //this.stickmonSprite = stickmonSprite;
    }

    public string GetStickmonName()
    {
        return name;
    }
}
