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
    private Sprite _stickmonSprite;
    private int[] _allLevelBarriers;

    public CurrentStickmon(string name, int level, float experiencePoints, float maxHealthPoints, List<StickmonMove> allCurrentMoves, Sprite stickmonSprite, int[] allLevelBarriers)
    {
        _name = name;
        _level = level;
        _experiencePoints = experiencePoints;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
        _allCurrentMoves = allCurrentMoves;
        _stickmonSprite = stickmonSprite;
        _allLevelBarriers = allLevelBarriers;
    }

    public CurrentStickmon(Encounter newEncounter, int[] allLevelBarriers, float experiencePoints)
    {
        _name = newEncounter.GetEncounterName();
        _level = newEncounter.GetEncounterLevel();       
        _experiencePoints = experiencePoints;
        _maxHealthPoints = newEncounter.GetEncounterMaxHealth();
        _currentHealthPoints = 0;
        _allCurrentMoves = newEncounter.GetEnounterMoves();
        _stickmonSprite = newEncounter.GetEncounterImage();
        _allLevelBarriers = allLevelBarriers;
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

    public Sprite GetStickmonSprite()
    {
        return _stickmonSprite;
    }

    public StickmonMove GetStickmonMove(int index)
    {
        return _allCurrentMoves[index];
    }

    public List<StickmonMove> GetAllStickmonMoves()
    {
        return _allCurrentMoves;
    }

    #endregion

    #region DuringBattle

    public bool DecreaseHealth(float amountOfDamage)
    {
        if (_currentHealthPoints - amountOfDamage > 0)
        {
            _currentHealthPoints -= amountOfDamage;
            return false;
        }
        else
        {
            _currentHealthPoints = 0;
            return true;
        }
    }

    #endregion

    #region AfterBattle

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

    #endregion

    #region Healing

    public void HealStickmon()
    {
        _currentHealthPoints = _maxHealthPoints;
    }

    #endregion
    
}
