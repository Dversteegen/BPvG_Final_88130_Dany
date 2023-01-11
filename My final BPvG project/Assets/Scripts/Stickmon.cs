using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string name;
    private float baseHealthPoints;
    //private List<StickmonMove> allPossibleMoves;
    private Texture2D stickmonSprite;
    
    public Stickmon(string c_name, float c_baseHealthPoints)
    {
        this.name = c_name;
        this.baseHealthPoints = c_baseHealthPoints;
    }

    public string GetStickmonName()
    {
        return name;
    }

    public float GetBaseHealthPoints()
    {
        return baseHealthPoints;
    }    
}
