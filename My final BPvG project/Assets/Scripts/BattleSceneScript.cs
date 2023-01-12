using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleSceneScript : MonoBehaviour
{
    #region UIElements

    [Header("Images")]
    public Image opponentImage;
    public Image playerImage;

    [Header("TextMesh")]
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI opponentName;

    [SerializeField] TextMeshProUGUI firstMove;
    [SerializeField] TextMeshProUGUI secondMove;
    [SerializeField] TextMeshProUGUI thirdMove;

    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] TextMeshProUGUI opponentLevel;

    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI opponentHealth;

    [Header("Buttons")]
    [SerializeField] Button firstMoveButton;
    [SerializeField] Button secondMoveButton;
    [SerializeField] Button thirdMoveButton;

    #endregion

    #region Sprites

    [Header("Sprites")]
    public Sprite larrySprite;
    public Sprite paulSprite;
    public Sprite griffinSprite;
    public Sprite julianSprite;

    #endregion

    private Encounter _currentEncounter;

    private bool playerTurn = false;

    //private void Start()
    void Start()
    {
        //This needs to be above the GetComponent lines for the text, god knows why
        SetUpPlayer();
        _currentEncounter = GetEncounter();
        SetUpOpponent();
        DefineFirstAttacker();

        playerName = GetComponent<TextMeshProUGUI>();
        opponentName = GetComponent<TextMeshProUGUI>();

        firstMove = GetComponent<TextMeshProUGUI>();
        secondMove = GetComponent<TextMeshProUGUI>();
        thirdMove = GetComponent<TextMeshProUGUI>();

        firstMoveButton = GetComponent<Button>();
        secondMoveButton = GetComponent<Button>();
        thirdMoveButton = GetComponent<Button>();
    }

    #region SetUpBattleScene

    private void SetUpPlayer()
    {
        //Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
        //SetUpOpponent(encounter);

        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
        SetUpPlayerStickmon(firstStickmon);

    }

    #region SetUpPlayers

    private void SetUpPlayerStickmon(CurrentStickmon firstStickmon)
    {
        switch (firstStickmon.GetStickmonName())
        {
            case "Julian":
                playerImage.sprite = julianSprite;
                break;
        }
        playerName.text = firstStickmon.GetStickmonName();

        List<StickmonMove> allStickmonMoves = firstStickmon.GetAllStickmonMoves();

        SetUpPlayerMoves(allStickmonMoves);
        SetUpPlayerData(firstStickmon);
    }

    private void SetUpPlayerMoves(List<StickmonMove> allStickmonMoves)
    {
        if (allStickmonMoves.Count > 0)
        {
            firstMove.text = allStickmonMoves[0].GetMoveName();
        }

        if (allStickmonMoves.Count > 1)
        {
            secondMove.text = allStickmonMoves[1].GetMoveName();
        }
        else
        {
            secondMove.text = "";
            secondMoveButton.enabled = false;
        }

        if (allStickmonMoves.Count > 2)
        {
            thirdMove.text = allStickmonMoves[2].GetMoveName();
        }
        else
        {
            thirdMove.text = "";
            thirdMoveButton.enabled = false;
        }
    }

    private void SetUpPlayerData(CurrentStickmon firstStickmon)
    {
        float maxHealth = firstStickmon.GetMaxHealthPoints();
        float currentHealth = firstStickmon.GetCurrentHealthPoints();
        float level = firstStickmon.GetStickmonLevel();

        playerLevel.text = $"Level: {level}";
        playerHealth.text = $"Health: {currentHealth}/{maxHealth}";
    }

    #endregion

    #region SetUpOpponent    

    private Encounter GetEncounter()
    {
        Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
        CurrentStickmon firstStickmon = GetFirstStickmon();

        int encounterLevel = GetEncounterLevel(firstStickmon.GetStickmonLevel());
        int levelDifference = firstStickmon.GetStickmonLevel() - encounterLevel;

        float baseHealth = encounter.GetBaseHealthPoints();
        float maxHealth = GetEncounterHealthPoints(levelDifference, baseHealth);

        List<StickmonMove> allEncounterStickmonMoves = new List<StickmonMove>();
        StickmonMove currentStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStandardStickmonMove();
        allEncounterStickmonMoves.Add(currentStickmonMove);

        Encounter currentEncounter = new Encounter(encounter.GetStickmonName(), encounterLevel, maxHealth, maxHealth, allEncounterStickmonMoves);
        return currentEncounter;
    }

    private int GetEncounterLevel(int firstStickmonLevel)
    {
        int encounterLevel = firstStickmonLevel;
        int randomNumber = Random.Range(0, 4);
        switch (randomNumber)
        {
            case 0:
                encounterLevel++;
                break;

            case 1:
                encounterLevel += 2;
                break;

            case 2:
                break;

            case 3:
                encounterLevel -= 2;
                break;

            case 4:
                encounterLevel--;
                break;
        }
        return encounterLevel;
    }

    private float GetEncounterHealthPoints(int levelDifference, float baseHealth)
    {
        float maxHealth = baseHealth;

        for (int count = 0; count < levelDifference; count++)
        {
            int randomNumber = Random.Range(0, 2);

            if (randomNumber == 0)
            {
                maxHealth++;
            }
            else if (randomNumber == 2)
            {
                maxHealth += 2;
            }
        }

        return maxHealth;
    }

    private void SetUpOpponent()
    {
        string encounterName = _currentEncounter.GetEncounterName();
        Debug.Log(encounterName);

        switch (encounterName)
        {
            case "Larry":
                opponentImage.sprite = larrySprite;
                break;

            case "Paul":
                opponentImage.sprite = paulSprite;
                break;

            case "Griffin":
                opponentImage.sprite = griffinSprite;
                break;
        }
        opponentName.text = encounterName;
        opponentLevel.text = $"Level: {_currentEncounter.GetEncounterLevel()}";

        opponentHealth.text = $"Health: {_currentEncounter.GetEncouterCurrentHealth()}/{_currentEncounter.GetEncouterCurrentHealth()}";

    }

    #endregion

    #region DecideFirstAttacker

    private void DefineFirstAttacker()
    {
        int randomNumber = Random.Range(0, 1);
        if (randomNumber == 0)
        {
            playerTurn = true;
        }
    }

    #endregion

    #endregion

    #region UpdateBattleScene    

    #region BattleFunctions

    #region BattleEncounter

    #region PlayerTurn

    #region DealDamage

    public void GetAmountOfDamage(TextMeshProUGUI move)
    {
        Debug.Log("GetAmountOfDamage");
        string moveName = move.text;

        int amountOfDamage = 0;

        List<StickmonMove> allStickmonMoves = GameManagerScript.myGameManagerScript.GetAllStickmonMoves();

        foreach (StickmonMove stickmonMove in allStickmonMoves)
        {
            if (stickmonMove.GetMoveName() == moveName)
            {
                amountOfDamage = stickmonMove.GetAmountOfDamage();
            }
        }

        DamageEncounter(amountOfDamage);
    }

    private void DamageEncounter(int amountOfDamage)
    {
        bool isOpponentDead = _currentEncounter.DealDamage(amountOfDamage);

        if (isOpponentDead == false)
        {
            opponentHealth.text = $"Health: {_currentEncounter.GetEncouterCurrentHealth()}/{_currentEncounter.GetEncounterMaxHealth()}";
            playerTurn = false;
        }
        else
        {
            opponentHealth.text = $"Health: 0/{_currentEncounter.GetEncounterMaxHealth()}";
            opponentImage.enabled = false;            
            CalculateExperiencePoints();
        }
    }

    #endregion

    #region ExperiencePoints

    private void CalculateExperiencePoints()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
        int levelDifference = _currentEncounter.GetEncounterLevel() - firstStickmon.GetStickmonLevel();
        float multiplier = 1;

        if (levelDifference < 0)
        {
            multiplier = 0.5f;
        }
        else if (levelDifference > 0)
        {
            multiplier = 1.5f;
        }

        float amountOfExperiencePoints = multiplier * _currentEncounter.GetEncounterLevel();
        AddExperiencePoints(firstStickmon, amountOfExperiencePoints);
    }

    private void AddExperiencePoints(CurrentStickmon firstStickmon, float amountOfExperiencePoints)    
    {
        bool levelIncreased = firstStickmon.AddExperiencePoints(amountOfExperiencePoints);
        if (levelIncreased == true)
        {
            LevelUpStickmon(firstStickmon);
        }
        StartCoroutine(GoBack());        
    }

    private void LevelUpStickmon(CurrentStickmon firstStickmon)
    {        
        float extraHealth = Random.Range(1, 3);
        float newHealth = firstStickmon.IncreaseMaxHealth(extraHealth);

        playerHealth.text = $"Health: {firstStickmon.GetCurrentHealthPoints()}/{newHealth}";
    }

    IEnumerator GoBack()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("StartScene");
    }

    #endregion

    #endregion

    #region OpponentTurn



    #endregion

    #endregion

    #endregion

    #endregion

    #region GetFunctions

    private CurrentStickmon GetFirstStickmon()
    {
        return GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
    }

    #endregion
}
