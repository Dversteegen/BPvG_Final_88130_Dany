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
    private Sprite _stickmonBackSprite;

    private int[] _allLevelBarriers;

    /// <summary>
    /// This is used only once and that's when a new game has started and the first allied Stickmon needs to be defined still
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="experiencePoints"></param>
    /// <param name="maxHealthPoints"></param>
    /// <param name="allCurrentMoves"></param>
    /// <param name="stickmonSprite"></param>
    /// <param name="stickmonBackSprite"></param>
    /// <param name="allLevelBarriers"></param>
    public CurrentStickmon(string name, int level, float experiencePoints, float maxHealthPoints, List<StickmonMove> allCurrentMoves, Sprite stickmonSprite, Sprite stickmonBackSprite, int[] allLevelBarriers)
    {
        _name = name;
        _level = level;
        _experiencePoints = experiencePoints;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = maxHealthPoints;
        _allCurrentMoves = allCurrentMoves;
        _stickmonSprite = stickmonSprite;
        _stickmonBackSprite= stickmonBackSprite;
        _allLevelBarriers = allLevelBarriers;
    }

    /// <summary>
    /// This constructor is used when defining the CurrentStickmon when there have been values stored in the PlayerPrefs.
    /// This constructor has the extra parameter currentHealth
    /// </summary>
    /// <param name="name"></param>
    /// <param name="level"></param>
    /// <param name="experiencePoints"></param>
    /// <param name="currentHealthPoints"></param>
    /// <param name="maxHealthPoints"></param>
    /// <param name="allCurrentMoves"></param>
    /// <param name="stickmonSprite"></param>
    /// <param name="stickmonBackSprite"></param>
    /// <param name="allLevelBarriers"></param>
    public CurrentStickmon(string name, int level, float experiencePoints, float currentHealthPoints, float maxHealthPoints, List<StickmonMove> allCurrentMoves, Sprite stickmonSprite, Sprite stickmonBackSprite, int[] allLevelBarriers)
    {
        _name = name;
        _level = level;
        _experiencePoints = experiencePoints;
        _maxHealthPoints = maxHealthPoints;
        _currentHealthPoints = currentHealthPoints;
        _allCurrentMoves = allCurrentMoves;
        _stickmonSprite = stickmonSprite;
        _stickmonBackSprite = stickmonBackSprite;
        _allLevelBarriers = allLevelBarriers;
    }

    /// <summary>
    /// This constructor is being used when the player recruited an ecnounter
    /// </summary>
    /// <param name="newEncounter"></param>
    /// <param name="allLevelBarriers"></param>
    /// <param name="experiencePoints"></param>
    public CurrentStickmon(Encounter newEncounter, int[] allLevelBarriers, float experiencePoints)
    {
        _name = newEncounter.GetEncounterName();
        _level = newEncounter.GetEncounterLevel();       
        _experiencePoints = experiencePoints;
        _maxHealthPoints = newEncounter.GetEncounterMaxHealth();
        _currentHealthPoints = 0;
        _allCurrentMoves = newEncounter.GetEnounterMoves();
        _stickmonSprite = newEncounter.GetEncounterNormalImage();
        _stickmonBackSprite = newEncounter.GetEncounterBackImage();
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

    public Sprite GetNormalStickmonImage()
    {
        return _stickmonSprite;
    }

    public Sprite GetBackStickmonImage()
    {
        return _stickmonBackSprite;
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

    /// <summary>
    /// Decreased the heakth of the Stickmon and returns true if the Stickmon has zero health points
    /// </summary>
    /// <param name="amountOfDamage"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Adds the gained experience points to the current amount and returns true if the Stickmon has gained a level
    /// When that happens, the first level barrier will be removed
    /// </summary>
    /// <param name="amountOfNewExperiencePoints"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Increases the max and current healthpoints when the Stickmon had inceased a level
    /// </summary>
    /// <param name="amountOfHealth"></param>
    /// <returns></returns>
    public float IncreaseMaxHealth(float amountOfHealth)
    {
        _currentHealthPoints += amountOfHealth;
        _maxHealthPoints += amountOfHealth;
        return _maxHealthPoints;
    }

    #endregion

    #region Healing

    /// <summary>
    /// Heals the current Stickmon
    /// </summary>
    public void HealStickmon()
    {
        _currentHealthPoints = _maxHealthPoints;
    }

    #endregion
    
}
