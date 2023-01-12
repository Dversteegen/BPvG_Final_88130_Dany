using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string _name;
    private float _baseHealthPoints;
    //private List<StickmonMove> allPossibleMoves;
    //private Texture2D _stickmonSprite;
    
    public Stickmon(string name, float baseHealthPoints)
    {
        _name = name;
        _baseHealthPoints = baseHealthPoints;
    }

    public string GetStickmonName()
    {
        return _name;
    }

    public float GetBaseHealthPoints()
    {
        return _baseHealthPoints;
    }    
}
