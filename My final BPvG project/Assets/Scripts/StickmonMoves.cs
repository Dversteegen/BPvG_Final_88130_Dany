using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmonMove
{
    private string moveName;
    //private string type;
    private int damage;

    //public StickmonMove(string moveName, string type, int damage)
    public StickmonMove(string c_moveName, int c_damage)
    {
        this.moveName = c_moveName;
        //this.type = type;
        this.damage = c_damage;
    }

    public string GetMoveName()
    {
        return this.moveName;
    }

    public int GetAmountOfDamage()
    {
        return this.damage;
    }
}
