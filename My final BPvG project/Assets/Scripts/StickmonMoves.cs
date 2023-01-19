using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmonMove
{
    private string _moveName;    
    private int _damage;
    
    public StickmonMove(string moveName, int damage)
    {
        _moveName = moveName;        
        _damage = damage;
    }

    public string GetMoveName()
    {
        return _moveName;
    }

    public int GetAmountOfDamage()
    {
        return _damage;
    }
}
