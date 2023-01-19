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

    #region Variables

    //Colors
    private Color basicButtonColor = new Color(64, 64, 64, 1);
    private Color basicButtonTransparentColor = new Color(64, 64, 64, 0);

    //CurrentEncounter
    private Encounter currentEncounter;

    //Booleans
    private bool playerCanAttack = false;
    private bool battleIsOver = false;
    private bool experiencePointsCalculated = false;
    private bool playerTurn = false;
    private bool optionToRecruit = false;
    private bool currentStickmonDied = false;
    private bool outOfStickmon = false;

    //Array
    private int[] allLevelBarriers = new int[] { 5, 11, 18, 26, 35, 47, 61, 78, 98, 123, 153 };

    //Lists
    List<string> upComingLines = new List<string>();
    List<Encounter> allCurrentEncounters = new List<Encounter>();

    #endregion
    
    void Start()
    {
        ToggleRecruitElements("disable");
        SetUpPlayer();

        if (PlayerPrefs.GetString("BattleType") == "Encounter")
        {
            currentEncounter = GetEncounter();
        }
        else
        {
            int maxLevel = PlayerPrefs.GetInt("MaxLevel");
            currentEncounter = GetArrangedStickmon(maxLevel);
        }

        SetUpOpponent();
        SetUpText();
    }

    #region SetUpBattleScene

    #region SetUpPlayer

    private void SetUpPlayer()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstHealthyAllliedStickmon();
        SetUpPlayerStickmon(firstStickmon);
    }

    private void SetUpPlayerStickmon(CurrentStickmon firstStickmon)
    {
        Debug.Log(firstStickmon.GetBackStickmonImage());
        playerImage.sprite = firstStickmon.GetBackStickmonImage();
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

    #region SetUpEncounter  

    private Encounter GetEncounter()
    {
        Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
        CurrentStickmon firstStickmon = GetFirstStickmon();

        int encounterLevel = GetEncounterLevel(firstStickmon.GetStickmonLevel());        

        float baseHealth = encounter.GetBaseHealthPoints();
        float maxHealth = GetEncounterHealthPoints(encounterLevel, baseHealth);

        List<StickmonMove> allEncounterStickmonMoves = new List<StickmonMove>();
        StickmonMove currentStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStickmonMove(0, 6);
        allEncounterStickmonMoves.Add(currentStickmonMove);

        if (encounterLevel >= 3)
        {
            currentStickmonMove = GameManagerScript.myGameManagerScript.GetNewRandomStickmonMove(3, 7, allEncounterStickmonMoves);
            allEncounterStickmonMoves.Add(currentStickmonMove);
        }        

        Encounter currentEncounter = new Encounter(encounter.GetStickmonName(), encounterLevel, maxHealth, maxHealth, allEncounterStickmonMoves, encounter.GetStickmonImage("normal"), encounter.GetStickmonImage("back"));
        return currentEncounter;
    }

    private int GetEncounterLevel(int firstStickmonLevel)
    {
        int maxLevel = PlayerPrefs.GetInt("MaxEncounterLevel");

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
                if (encounterLevel - 2 > 0)
                {
                    encounterLevel -= 2;
                }
                break;

            case 4:
                if (encounterLevel - 1 > 0)
                {
                    encounterLevel--;
                }
                break;
        }
        if (encounterLevel > maxLevel)
        {
            encounterLevel = maxLevel;
        } else if (encounterLevel < 3)
        {
            encounterLevel = 3;
        }
        return encounterLevel;
    }

    private float GetEncounterHealthPoints(int encounterLevel, float baseHealth)
    {
        float maxHealth = baseHealth;

        for (int count = 0; count < encounterLevel; count++)
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

    #endregion

    #region SetUpSetOpponent

    private Encounter GetArrangedStickmon(int maxLevel)
    {
        float maxhealth = GetEncounterHealthPoints(maxLevel, 30);
        Stickmon arrangedStickmon = GameManagerScript.myGameManagerScript.GetStickmonPaul();

        List<StickmonMove> allCurrentMoves = new List<StickmonMove>();
        StickmonMove currentStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStickmonMove(0, 8);
        allCurrentMoves.Add(currentStickmonMove);

        currentStickmonMove = GameManagerScript.myGameManagerScript.GetNewRandomStickmonMove(3, 11, allCurrentMoves);
        allCurrentMoves.Add(currentStickmonMove);

        currentStickmonMove = GameManagerScript.myGameManagerScript.GetNewRandomStickmonMove(7, 11, allCurrentMoves);
        allCurrentMoves.Add(currentStickmonMove);

        currentEncounter = new Encounter("Paul", maxLevel, maxhealth, maxhealth, allCurrentMoves, arrangedStickmon.GetStickmonImage("normal"), arrangedStickmon.GetStickmonImage("back"));
        return currentEncounter;
    }

    #endregion

    private void SetUpOpponent()
    {
        string encounterName = currentEncounter.GetEncounterName();

        opponentImage.sprite = currentEncounter.GetEncounterNormalImage();
        opponentName.text = encounterName;
        opponentLevel.text = $"Level: {currentEncounter.GetEncounterLevel()}";

        opponentHealth.text = $"Health: {currentEncounter.GetEncouterCurrentHealth()}/{currentEncounter.GetEncouterCurrentHealth()}";
    }

    #region SetUpUI

    private void SetUpText()
    {
        battleSceneText.text = $"Oh no, {currentEncounter.GetEncounterName()} has appeared!";
        DefineFirstAttacker();
    }

    #endregion

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

            //Check
            battleSceneText.text = $"{GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName()} used {moveName} and dealt {amountOfDamage} damage";
            ToggleContinueTextElements("enable");
            DamageEncounter(amountOfDamage);
        }
    }

    private void DamageEncounter(int amountOfDamage)
    {
        bool isOpponentDead = currentEncounter.DealDamage(amountOfDamage);

        if (isOpponentDead == false)
        {
            opponentHealth.text = $"Health: {currentEncounter.GetEncouterCurrentHealth()}/{currentEncounter.GetEncounterMaxHealth()}";
            playerTurn = false;
        }
        else
        {
            opponentHealth.text = $"Health: 0/{currentEncounter.GetEncounterMaxHealth()}";
            upComingLines.Add($"{currentEncounter.GetEncounterName()} isn't able to continue fighting");
            opponentImage.enabled = false;
            battleIsOver = true;
        }
    }

    #endregion

    #region ExperiencePoints

    private void CalculateExperiencePoints()
    {
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstHealthyAllliedStickmon();
        int levelDifference = currentEncounter.GetEncounterLevel() - firstStickmon.GetStickmonLevel();
        float multiplier = 1;

        if (levelDifference < 0)
        {
            multiplier = 0.5f;
        }
        else if (levelDifference > 0)
        {
            multiplier = 1.5f;
        }

        float amountOfExperiencePoints = multiplier * currentEncounter.GetEncounterLevel();

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
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstHealthyAllliedStickmon();

        List<StickmonMove> allStickmonMoves = currentEncounter.GetEnounterMoves();
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
        battleSceneText.text = $"{currentEncounter.GetEncounterName()} has used {opponentMove} and dealt {moveDamage} damage";

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
        playerTurn = true;
    }

    private void CheckSquad()
    {
        if (GameManagerScript.myGameManagerScript.GetAmountOfHealthyStickmon() == 0)
        {
            battleSceneText.text = $"You ran out of healthy Stickmon";
            battleIsOver = true;
            outOfStickmon = true;

            PlayerPrefs.SetFloat("PlayerPositionX", -2f);
            PlayerPrefs.SetFloat("PlayerPositionY", 0.5f);
        }
        else
        {
            playerImage.enabled = true;
            currentStickmonDied = false;
            battleSceneText.text = $"{GameManagerScript.myGameManagerScript.GetFirstHealthyAllliedStickmon().GetStickmonName()} has entered the battle";
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
            //int randomNumber = Random.Range(0, 3);
            int randomNumber = 2;
            if (randomNumber == 2)
            {
                optionToRecruit = true;
                upComingLines.Add($"{currentEncounter.GetEncounterName()} wishes to join you, do you accept?");
            }
        }
    }

    public void RecruitEncounter()
    {
        int level = currentEncounter.GetEncounterLevel();
        float currentAmountOfExperiencePoints = 0;
        for (int count = 0; count < level - 1; count++)
        {
            currentAmountOfExperiencePoints = allLevelBarriers[0];
            allLevelBarriers = allLevelBarriers.Where(levelBarrier => levelBarrier != allLevelBarriers[0]).ToArray();
        }

        CurrentStickmon newAlliedStickmon = new CurrentStickmon(currentEncounter, allLevelBarriers, currentAmountOfExperiencePoints);
        Debug.Log(newAlliedStickmon.GetBackStickmonImage());
        Debug.Log(newAlliedStickmon.GetNormalStickmonImage());
        GameManagerScript.myGameManagerScript.AddAlliedStickmon(newAlliedStickmon);
        upComingLines.Add($"{newAlliedStickmon.GetStickmonName()} has joined your squad, good luck!");

        optionToRecruit = false;

        ToggleRecruitElements("disable");
        ToggleContinueTextElements("enable");

        EnableAttacking();
    }

    #endregion

    #endregion

    #region SetBattle

    #endregion

    #endregion

    #region GeneralSceneFunctions

    #region ToggleFunctions

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
        else if (status == "recruitment")
        {
            continueTextButton.enabled = true;
            continueTextButton.image.color = basicButtonColor;
            continueButtonText.text = "Reject";
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
        if (continueButtonText.text == "Reject")
        {
            ToggleRecruitElements("disable");
            continueButtonText.text = "Continue";
            optionToRecruit = false;
            battleSceneText.text = $"You decided not to recruit {currentEncounter.GetEncounterName()}";
        }
        else
        {
            if (upComingLines.Count > 0)
            {
                battleSceneText.text = upComingLines[0];
                upComingLines.RemoveAt(0);
                Debug.Log("EnableAttacking 1");
            }
            else if (playerTurn == true && battleIsOver == false && currentStickmonDied == false)
            {
                ToggleContinueTextElements("disable");

                battleSceneText.text = $"Choose a move:";

                playerCanAttack = true;
                Debug.Log("EnableAttacking 2");
            }
            else if (playerTurn == false && battleIsOver == false && currentStickmonDied == false)
            {
                OpponentAttack();
                ToggleContinueTextElements("enable");
                Debug.Log("EnableAttacking 3");
            }
            else if (currentStickmonDied == true && battleIsOver == false)
            {
                CheckSquad();
                Debug.Log("EnableAttacking 4");
            }
            else if (battleIsOver == true && experiencePointsCalculated == false && outOfStickmon == false)
            {
                CalculateExperiencePoints();
                ToggleContinueTextElements("enable");
                Debug.Log("EnableAttacking 5");
            }
            else if (optionToRecruit == true)
            {
                ToggleRecruitElements("enable");
                ToggleContinueTextElements("recruitment");
                Debug.Log("EnableAttacking 6");
            }
            else
            {
                GoBack();
            }
        }
    }

    #endregion

    #endregion

    #region GetFunctions

    private CurrentStickmon GetFirstStickmon()
    {
        return GameManagerScript.myGameManagerScript.GetFirstHealthyAllliedStickmon();
    }

    #endregion

}
