using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmonMove
{
    private string _moveName;
    //private string type;
    private int _damage;

    //public StickmonMove(string moveName, string type, int damage)
    public StickmonMove(string moveName, int damage)
    {
        _moveName = moveName;
        //this.type = type;
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
