using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CurrentStickmon
{
    private string _name;

    private int _level;
    private float _experiencePoints;
    private float _maxHealthPoints;
    private float _currentHealthPoints;

    private List<StickmonMove> _allCurrentMoves;
    private int[] _allLevelBarriers;

    public CurrentStickmon(string name, int level, float experiencePoints, float maxHealthPoints, List<StickmonMove> allCurrentMoves)
    {
        _name = name;
        _level = level;
        _experiencePoints = experiencePoints;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
        _allCurrentMoves = allCurrentMoves;
        _allLevelBarriers = new int[] { 18, 26, 35, 47, 61, 78, 92, 109, 133 };
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

    #region DuringBattle

    public bool DecreaseHealth(float amountOfDamage)
    {
        if (_currentHealthPoints - amountOfDamage >= 0)
        {
            _currentHealthPoints -= amountOfDamage;
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion

    public bool AddExperiencePoints(float amountOfNewExperiencePoints)
    {
        _experiencePoints += amountOfNewExperiencePoints;

        if (_experiencePoints > _allLevelBarriers[0])
        {            
            _allLevelBarriers = _allLevelBarriers.Where(level => level != _allLevelBarriers[0]).ToArray();            
            _level++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public float IncreaseMaxHealth(float amountOfHealth)
    {
        _currentHealthPoints += amountOfHealth;
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

    public List<StickmonMove> GetAllStickmonMoves()
    {
        return _allCurrentMoves;
    }

}
