using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStickmon
{
    private string name;

    private int level;
    private int experiencePoints;
    private float maxHealthPoints;
    private float currentHealthPoints;

    private List<StickmonMove> allCurrentMoves;

    public CurrentStickmon(string c_name, int c_level, int c_experiencePoints, float c_maxHealthPoints, List<StickmonMove> c_allCurrentMoves)
    {
        this.name = c_name;
        this.level = c_level;
        this.experiencePoints = c_experiencePoints;
        this.maxHealthPoints = c_maxHealthPoints;
        this.currentHealthPoints = c_maxHealthPoints;
        this.allCurrentMoves = c_allCurrentMoves;
    }

    #region GetFunctions

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

    public float GetMaxHealthPoints()
    {
        return maxHealthPoints;
    }

    public float GetCurrentHealthPoints()
    {
        return currentHealthPoints;
    }

    #endregion

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

    public List<StickmonMove>GetAllStickmonMoves()
    {
        return allCurrentMoves;
    }

}
