using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmonMove
{
    private string moveName;
    //private string type;
    private int damage;

    //public StickmonMove(string moveName, string type, int damage)
    public StickmonMove(string moveName, int damage)
    {
        this.moveName = moveName;
        //this.type = type;
        this.damage = damage;
    }
}
