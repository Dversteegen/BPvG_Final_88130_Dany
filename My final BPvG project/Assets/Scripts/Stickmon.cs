using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stickmon
{
    private string _name;
    private float _baseHealthPoints;
    private Sprite _stickmonImage;
    private Sprite _stickmonBackImage;    

    public Stickmon(string name, float baseHealthPoints, Sprite stickmonImage, Sprite stickmonBackImage)
    {
        _name = name;
        _baseHealthPoints = baseHealthPoints;
        _stickmonImage = stickmonImage;
        _stickmonBackImage = stickmonBackImage;
    }

    public string GetStickmonName()
    {
        return _name;
    }

    public float GetBaseHealthPoints()
    {
        return _baseHealthPoints;
    }

    /// <summary>
    /// Returns either the normal sprite or the back sprite depending on the string
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite GetStickmonImage(string type)
    {
        if (type == "normal")
        {
            return _stickmonImage;
        }
        else
        {
            return _stickmonBackImage;
        }        
    }
}
