using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter
{
    private string _name;
    private int _level;

    private float _maxHealth;
    private float _currentHealth;

    private Sprite _stickmonImage;

    private List<StickmonMove> _allmoves;

    public Encounter(string encounterName, int encounterLevel, float maxHealth, float currentHealth, List<StickmonMove> allEncounterMoves, Sprite stickmonImage)
    {
        _name = encounterName;
        _level = encounterLevel;
        _maxHealth = maxHealth;
        _currentHealth = currentHealth;
        _allmoves = allEncounterMoves;
        _stickmonImage = stickmonImage;
    }

    #region GetFunctions

    public string GetEncounterName()
    {
        return _name;
    }

    public int GetEncounterLevel()
    {
        return _level;
    }

    public float GetEncounterMaxHealth()
    {
        return _maxHealth;
    }

    public float GetEncouterCurrentHealth()
    {
        return _currentHealth;
    }

    public List<StickmonMove>GetEnounterMoves()
    {
        return _allmoves;
    }

    public Sprite GetEncounterImage()
    {
        return _stickmonImage;
    }

    #endregion

    #region BattleFunctions

    public bool DealDamage(float amountOfDamage)
    {
        if (_currentHealth - amountOfDamage <= 0)
        {
            _currentHealth = 0;
            return true;
        }
        else
        {
            _currentHealth -= amountOfDamage;
            return false;
        }        
    }

    #endregion
}
