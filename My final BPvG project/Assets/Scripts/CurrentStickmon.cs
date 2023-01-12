using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentStickmon
{
    private string _name;

    private int _level;
    private float _experiencePoints;
    private float _maxHealthPoints;
    private float _currentHealthPoints;

    private List<StickmonMove> _allCurrentMoves;

    public CurrentStickmon(string name, int level, float experiencePoints, float maxHealthPoints, List<StickmonMove> allCurrentMoves)
    {
        _name = name;
        _level = level;
        _experiencePoints = experiencePoints;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
        _allCurrentMoves = allCurrentMoves;
    }

    #region GetFunctions

    public string GetStickmonName()
    {
        return _name;
    }

    public int GetStickmonLevel()
    {
        return _level;
    }

    public float GetExperiencePoints()
    {
        return _experiencePoints;
    }

    public float GetMaxHealthPoints()
    {
        return _maxHealthPoints;
    }

    public float GetCurrentHealthPoints()
    {
        return _currentHealthPoints;
    }

    #endregion

    public bool AddExperiencePoints(float amountOfNewExperiencePoints)
    {
        _experiencePoints += amountOfNewExperiencePoints;

        switch (_experiencePoints)
        {
            case >5:
                //Check if there's a new level
                _level++;
                return true;
        }
        return false;
    }

    public float IncreaseMaxHealth(float amountOfHealth)
    {
        _maxHealthPoints += amountOfHealth;
        return _maxHealthPoints;
    }

    public int GetAmountOfStickmonMoves()
    {
        return _allCurrentMoves.Count;
    }

    public StickmonMove GetStickmonMove(int index)
    {
        return _allCurrentMoves[index];
    }

    public List<StickmonMove>GetAllStickmonMoves()
    {
        return _allCurrentMoves;
    }

}
