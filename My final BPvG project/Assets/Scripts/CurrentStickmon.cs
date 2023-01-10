using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStickmon
{
    private string name;
    private int level;
    private int experiencePoints;

    private List<StickmonMove> allCurrentMoves;


    public CurrentStickmon(string name, int level, int experiencePoints, List<StickmonMove> allCurrentMoves)
    {
        this.name = name;
        this.level = level;
        this.experiencePoints = experiencePoints;
        this.allCurrentMoves = allCurrentMoves;
    }

    public string GetStickmonName()
    {
        return name;
    }

    public int GetStickmonLevel()
    {
        return level;
    }

    public int GetExperiencePoints()
    {
        return experiencePoints;
    }

    public bool AddExperiencePoints(int amountOfNewExperiencePoints)
    {
        this.experiencePoints += amountOfNewExperiencePoints;

        switch (this.experiencePoints)
        {
            case 100:
                //Check if there's a new level
                return true;
        }
        return false;
    }

    public int GetAmountOfStickmonMoves()
    {
        return allCurrentMoves.Count;
    }

    public StickmonMove GetStickmonMove(int index)
    {
        return allCurrentMoves[index];
    }

}
