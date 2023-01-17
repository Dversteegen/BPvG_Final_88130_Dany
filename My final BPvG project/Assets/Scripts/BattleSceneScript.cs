using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class BattleSceneScript : MonoBehaviour
{
    #region UIElements

    [Header("Images")]
    //Images
    public Image opponentImage;
    public Image playerImage;

    [Header("TextMesh")]
    //Names
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI opponentName;

    //Moves
    [SerializeField] TextMeshProUGUI firstMoveText;
    [SerializeField] TextMeshProUGUI secondMoveText;
    [SerializeField] TextMeshProUGUI thirdMoveText;

    //Levels
    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] TextMeshProUGUI opponentLevel;

    //Health
    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI opponentHealth;

    //Dialogue
    [SerializeField] TextMeshProUGUI battleSceneText;

    //Continue text button text
    [SerializeField] TextMeshProUGUI continueButtonText;

    //Recruiting
    [SerializeField] TextMeshProUGUI recruitEncounterButtonText;

    [Header("Buttons")]
    //Moves
    [SerializeField] Button firstMoveButton;
    [SerializeField] Button secondMoveButton;
    [SerializeField] Button thirdMoveButton;

    //Continue text button
    [SerializeField] Button continueTextButton;

    //Recruit
    [SerializeField] Button recruitEncounterButton;

    #endregion

    //Colors
    private Color basicButtonColor = new Color(64, 64, 64, 1);
    private Color basicButtonTransparentColor = new Color(64, 64, 64, 0);

    //CurrentEncounter
    private Encounter _currentEncounter;

    //Booleans
    private bool playerCanAttack = false;
    private bool battleIsOver = false;
    private bool experiencePointsCalculated = false;
    private bool playerTurn = false;
    private bool optionToRecruit = false;
    private bool currentStickmonDied = false;
    private bool outOfStickmon = false;

    private int indexOfAlliedStickmon = 0;

    private bool beginOfBattle = true;
    private bool opponentHasAttacked = false;

    private int[] allLevelBarriers = new int[] { 5, 11, 18, 26, 35, 47, 61, 78, 98, 123, 153 };

    List<string> upComingLines = new List<string>();
    List<Encounter> allCurrentEncounters = new List<Encounter>();

    //private void Start()
    void Start()
    {
        ToggleRecruitElements("disable");

        SetUpPlayer();

        if (PlayerPrefs.GetString("BattleType") == "Encounter")
        {
            _currentEncounter = GetEncounter();
        }
        else
        {
            allCurrentEncounters = new List<Encounter>();
        }

        SetUpOpponent();
        SetUpText();
    }

    #region SetUpBattleScene

    private void SetUpPlayer()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[indexOfAlliedStickmon];
        SetUpPlayerStickmon(firstStickmon);
    }

    #region SetUpPlayers

    private void SetUpPlayerStickmon(CurrentStickmon firstStickmon)
    {
        playerImage.sprite = firstStickmon.GetStickmonSprite();
        playerName.text = firstStickmon.GetStickmonName();

        List<StickmonMove> allStickmonMoves = firstStickmon.GetAllStickmonMoves();

        SetUpPlayerMoves(allStickmonMoves);
        SetUpPlayerData(firstStickmon);
    }

    private void SetUpPlayerMoves(List<StickmonMove> allStickmonMoves)
    {
        if (allStickmonMoves.Count > 0)
        {
            firstMoveText.text = allStickmonMoves[0].GetMoveName();
        }

        if (allStickmonMoves.Count > 1)
        {
            secondMoveText.text = allStickmonMoves[1].GetMoveName();
        }
        else
        {
            secondMoveText.text = "";
            secondMoveButton.enabled = false;
        }

        if (allStickmonMoves.Count > 2)
        {
            thirdMoveText.text = allStickmonMoves[2].GetMoveName();
        }
        else
        {
            thirdMoveText.text = "";
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

        Encounter currentEncounter = new Encounter(encounter.GetStickmonName(), encounterLevel, maxHealth, maxHealth, allEncounterStickmonMoves, encounter.GetStickmonImage());
        return currentEncounter;
    }

    private int GetEncounterLevel(int firstStickmonLevel)
    {
        int encounterLevel = firstStickmonLevel;
        int randomNumber = Random.Range(0, 5);
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
                if (encounterLevel - 2 > 1)
                {
                    encounterLevel -= 2;
                }
                break;

            case 4:
                if (encounterLevel - 1 > 1)
                {
                    encounterLevel--;
                }
                break;
        }
        return encounterLevel;
    }

    private float GetEncounterHealthPoints(int levelDifference, float baseHealth)
    {
        float maxHealth = baseHealth;

        for (int count = 0; count < levelDifference; count++)
        {
            int randomNumber = Random.Range(0, 3);

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

        opponentImage.sprite = _currentEncounter.GetEncounterImage();
        opponentName.text = encounterName;
        opponentLevel.text = $"Level: {_currentEncounter.GetEncounterLevel()}";

        opponentHealth.text = $"Health: {_currentEncounter.GetEncouterCurrentHealth()}/{_currentEncounter.GetEncouterCurrentHealth()}";
    }

    #endregion

    private void SetUpText()
    {
        battleSceneText.text = $"Oh no, {_currentEncounter.GetEncounterName()} has appeared!";
        DefineFirstAttacker();
    }

    #region DecideFirstAttacker

    private void DefineFirstAttacker()
    {
        int randomNumber = Random.Range(0, 2);
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
        if (playerCanAttack == true)
        {
            playerCanAttack = false;
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

            battleSceneText.text = $"{GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName()} used {moveName} and dealt {amountOfDamage} damage";
            ToggleContinueTextElements("enable");
            DamageEncounter(amountOfDamage);
        }
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
            upComingLines.Add($"{_currentEncounter.GetEncounterName()} isn't able to continue fighting");
            opponentImage.enabled = false;
            battleIsOver = true;
        }
    }

    #endregion

    #region ExperiencePoints

    private void CalculateExperiencePoints()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[indexOfAlliedStickmon];
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

        battleSceneText.text = $"{firstStickmon.GetStickmonName()} has obtained {amountOfExperiencePoints} experience points";
        experiencePointsCalculated = true;
        AddExperiencePoints(firstStickmon, amountOfExperiencePoints);
    }

    private void AddExperiencePoints(CurrentStickmon firstStickmon, float amountOfExperiencePoints)
    {
        bool levelIncreased = firstStickmon.AddExperiencePoints(amountOfExperiencePoints);
        if (levelIncreased == true)
        {
            LevelUpStickmon(firstStickmon);
        }
        DefineRecruitment();
    }

    private void LevelUpStickmon(CurrentStickmon firstStickmon)
    {
        float extraHealth = Random.Range(1, 4);
        float newHealth = firstStickmon.IncreaseMaxHealth(extraHealth);

        upComingLines.Add($"{firstStickmon.GetStickmonName()} has reached level {firstStickmon.GetStickmonLevel()}");
        upComingLines.Add($"{firstStickmon.GetStickmonName()} has their max health increased to {newHealth}");
    }

    private void GoBack()
    {
        SceneManager.LoadScene("StartScene");
    }

    #endregion

    #endregion

    #region OpponentTurn

    #region DealDamage

    private void OpponentAttack()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[0];

        List<StickmonMove> allStickmonMoves = _currentEncounter.GetEnounterMoves();
        string opponentMove = "";
        int moveDamage = 0;

        switch (allStickmonMoves.Count)
        {
            case 1:
                opponentMove = allStickmonMoves[0].GetMoveName();
                moveDamage = allStickmonMoves[0].GetAmountOfDamage();
                break;

            case > 1:
                int randomNumber = Random.Range(0, allStickmonMoves.Count);
                opponentMove = allStickmonMoves[randomNumber].GetMoveName();
                moveDamage = allStickmonMoves[randomNumber].GetAmountOfDamage();
                break;
        }

        bool stickmonHasDied = firstStickmon.DecreaseHealth(moveDamage);
        battleSceneText.text = $"{_currentEncounter.GetEncounterName()} has used {opponentMove} and dealt {moveDamage} damage";

        if (stickmonHasDied == false)
        {
            playerHealth.text = $"Health: {firstStickmon.GetCurrentHealthPoints()}/{firstStickmon.GetMaxHealthPoints()}";
        }
        else
        {
            playerHealth.text = $"Health: 0/{firstStickmon.GetMaxHealthPoints()}";
            playerImage.enabled = false;
            upComingLines.Add($"{firstStickmon.GetStickmonName()} isn't able to continue fighting");
            currentStickmonDied = true;
        }
        opponentHasAttacked = true;
        playerTurn = true;
    }

    private void CheckSquad()
    {
        if (GameManagerScript.myGameManagerScript.GetAmountOfHealthyStickmon() == 0)
        {
            battleSceneText.text = $"You have run out of available Stickmon";
            battleIsOver = true;
            outOfStickmon = true;
        }
        else
        {
            indexOfAlliedStickmon++;
            SetUpPlayer();
        }
    }

    #endregion

    #endregion

    #region Recruiting

    private void DefineRecruitment()
    {
        if (GameManagerScript.myGameManagerScript.GetAllAlliedStickmon().Count < 3)
        {
            int randomNumber = 2;
            if (randomNumber == 2)
            {
                optionToRecruit = true;
                upComingLines.Add($"{_currentEncounter.GetEncounterName()} wishes to join you, do you accept?");
            }
        }
    }

    public void RecruitEncounter()
    {
        int level = _currentEncounter.GetEncounterLevel();
        float currentAmountOfExperiencePoints = 0;
        for (int count = 0; count < level - 1; count++)
        {
            currentAmountOfExperiencePoints = allLevelBarriers[0];
            allLevelBarriers = allLevelBarriers.Where(levelBarrier => levelBarrier != allLevelBarriers[0]).ToArray();
        }

        CurrentStickmon newAlliedStickmon = new CurrentStickmon(_currentEncounter, allLevelBarriers, currentAmountOfExperiencePoints);
        GameManagerScript.myGameManagerScript.AddAlliedStickmon(newAlliedStickmon);
        upComingLines.Add($"{newAlliedStickmon.GetStickmonName()} has joined your squad, good luck!");

        optionToRecruit = false;

        ToggleRecruitElements("disable");
        ToggleContinueTextElements("enable");

        EnableAttacking();
    }

    #endregion

    #endregion

    #endregion

    #endregion

    #region GetFunctions

    private CurrentStickmon GetFirstStickmon()
    {
        return GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[indexOfAlliedStickmon];
    }

    #endregion

    #region GeneralSceneFunctions

    #region ToggleFunctions

    public void ToggleAllButtons(string status)
    {
        //if (status == "enable")
        //{
        //    //Debug.Log(firstMoveButton);
        //    //Debug.Log(secondMoveButton);
        //    //Debug.Log(thirdMoveButton);            

        //    firstMoveButton.enabled = true;
        //    secondMoveButton.enabled = true;
        //    thirdMoveButton.enabled = true;
        //}
        //else
        //{
        //    //firstMoveButton.interactable = false;
        //    //secondMoveButton.interactable = false;
        //    //thirdMoveButton.interactable = false;

        //    firstMoveButton.enabled = false;
        //    secondMoveButton.enabled = false;
        //    thirdMoveButton.enabled = false;
        //}
    }

    private void ToggleRecruitElements(string status)
    {
        if (status == "disable")
        {
            recruitEncounterButton.image.color = basicButtonTransparentColor;
            recruitEncounterButtonText.text = "";
            recruitEncounterButton.enabled = false;
        }
        else
        {
            recruitEncounterButton.image.color = basicButtonColor;
            recruitEncounterButtonText.text = "Recruit";
            recruitEncounterButton.enabled = true;
        }
    }

    private void ToggleContinueTextElements(string status)
    {
        if (status == "disable")
        {
            continueTextButton.enabled = false;
            continueTextButton.image.color = basicButtonTransparentColor;
            continueButtonText.text = "";
        }
        else
        {
            continueTextButton.enabled = true;
            continueTextButton.image.color = basicButtonColor;
            continueButtonText.text = "Continue";
        }
    }

    #endregion

    public void EnableAttacking()
    {
        if (upComingLines.Count > 0)
        {
            battleSceneText.text = upComingLines[0];
            upComingLines.RemoveAt(0);
        }
        else if (playerTurn == true && battleIsOver == false && currentStickmonDied == false)
        {
            ToggleContinueTextElements("disable");

            battleSceneText.text = $"Choose a move:";

            beginOfBattle = false;
            playerCanAttack = true;
            opponentHasAttacked = false;
        }
        else if (playerTurn == false && battleIsOver == false && currentStickmonDied == false)
        {
            OpponentAttack();
            ToggleContinueTextElements("enable");
        }
        else if (currentStickmonDied == true)
        {
            CheckSquad();
        }
        else if (battleIsOver == true && experiencePointsCalculated == false && outOfStickmon == false)
        {
            CalculateExperiencePoints();
            ToggleContinueTextElements("enable");
        }
        else if (optionToRecruit == true)
        {
            ToggleRecruitElements("enable");
            ToggleContinueTextElements("disable");
        }
        else
        {
            GoBack();
        }
    }

    #endregion
}
