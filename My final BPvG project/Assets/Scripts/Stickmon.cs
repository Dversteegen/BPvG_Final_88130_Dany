using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string _name;
    private float _baseHealthPoints;
    private Sprite _stickmonImage;
    //private List<StickmonMove> allPossibleMoves;
    //private Texture2D _stickmonSprite;
    
    public Stickmon(string name, float baseHealthPoints, Sprite stickmonImage)
    {
        _name = name;
        _baseHealthPoints = baseHealthPoints;
        _stickmonImage = stickmonImage;
    }

    public string GetStickmonName()
    {
        return _name;
    }

    public float GetBaseHealthPoints()
    {
        return _baseHealthPoints;
    }

    public Sprite GetStickmonImage()
    {
        return _stickmonImage;
    }
}
