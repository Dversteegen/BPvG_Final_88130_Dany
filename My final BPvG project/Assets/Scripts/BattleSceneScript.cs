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

    [SerializeField] TextMeshProUGUI firstMoveText;
    [SerializeField] TextMeshProUGUI secondMoveText;
    [SerializeField] TextMeshProUGUI thirdMoveText;

    [SerializeField] TextMeshProUGUI playerLevel;
    [SerializeField] TextMeshProUGUI opponentLevel;

    [SerializeField] TextMeshProUGUI playerHealth;
    [SerializeField] TextMeshProUGUI opponentHealth;

    [SerializeField] TextMeshProUGUI battleSceneText;


    [Header("Buttons")]
    [SerializeField] Button firstMoveButton;
    [SerializeField] Button secondMoveButton;
    [SerializeField] Button thirdMoveButton;

    [SerializeField] Button continueTextButton;

    #endregion

    #region Sprites

    [Header("Sprites")]
    public Sprite larrySprite;
    public Sprite paulSprite;
    public Sprite griffinSprite;
    public Sprite julianSprite;

    #endregion

    private Encounter _currentEncounter;

    private bool beginOfBattle = true;
    private bool playerCanAttack = false;
    private bool opponentHasAttacked = false;
    private bool battleIsOver = false;
    private bool experiencePointsCalculated = false;
    private bool hasANewLevel = false;
    private bool playerTurn = false;

    List<string> upComingLines = new List<string>();

    //private void Start()
    void Start()
    {
        //This needs to be above the GetComponent lines for the text, god knows why
        SetUpPlayer();
        _currentEncounter = GetEncounter();
        SetUpOpponent();
        SetUpText();

        //DefineFirstAttacker();

    }

    #region SetUpBattleScene

    private void SetUpPlayer()
    {
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
        firstStickmon.AddExperiencePoints(5);

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

    private void SetUpText()
    {
        battleSceneText.text = $"Oh no, {_currentEncounter.GetEncounterName()} has appeared!";
        DefineFirstAttacker();
    }

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
            continueTextButton.enabled = true;
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
            battleSceneText.text = $"{_currentEncounter.GetEncounterName()} isn't able to continue fighting";
            opponentImage.enabled = false;
            battleIsOver = true;
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
        //StartCoroutine(GoBack());        
    }

    private void LevelUpStickmon(CurrentStickmon firstStickmon)
    {
        float extraHealth = Random.Range(1, 3);
        float newHealth = firstStickmon.IncreaseMaxHealth(extraHealth);

        upComingLines.Add($"{firstStickmon.GetStickmonName()} has reached level {firstStickmon.GetStickmonLevel()}");
        upComingLines.Add($"{firstStickmon.GetStickmonName()} has their max health increased to {newHealth}");
    }

    //IEnumerator GoBack()
    //{
    //    yield return new WaitForSeconds(2);
    //    SceneManager.LoadScene("StartScene");
    //}

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
        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();

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
                int randomNumber = Random.Range(0, allStickmonMoves.Count - 1);
                opponentMove = allStickmonMoves[randomNumber].GetMoveName();
                moveDamage = allStickmonMoves[randomNumber].GetAmountOfDamage();
                break;
        }

        bool stickmonHasDied = firstStickmon.DecreaseHealth(moveDamage);

        if (stickmonHasDied == false)
        {
            battleSceneText.text = $"{_currentEncounter.GetEncounterName()} has used {opponentMove} and dealt {moveDamage} damage";
            playerHealth.text = $"Health: {firstStickmon.GetCurrentHealthPoints()}/{firstStickmon.GetMaxHealthPoints()}";
        }
        opponentHasAttacked = true;
    }

    #endregion

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

    #region GeneralSceneFunctions

    public void ToggleAllButtons(string status)
    {
        //if (status == "enable")
        //{
        //    Debug.Log(firstMoveButton);
        //    Debug.Log(secondMoveButton);
        //    Debug.Log(thirdMoveButton);

        //    firstMoveButton.interactable = true;
        //    secondMoveButton.interactable = true;
        //    thirdMoveButton.interactable = true;

        //    //firstMoveButton.enabled = true;
        //    //secondMoveButton.enabled = true;
        //    //thirdMoveButton.enabled = true;
        //}
        //else
        //{
        //    firstMoveButton.interactable = false;
        //    secondMoveButton.interactable = false;
        //    thirdMoveButton.interactable = false;

        //    //firstMoveButton.enabled = false;
        //    //secondMoveButton.enabled = false;
        //    //thirdMoveButton.enabled = false;            
        //}
    }

    //private void ToggleContiueTextButton(string status)
    //{
    //    if (status == "enable")
    //    {
    //        continueTextButton.enabled = true;            
    //    }
    //    else
    //    {
    //        continueTextButton.enabled = false;            
    //    }
    //}

    public void EnableAttacking()
    {
        continueTextButton.enabled = true;
        if (upComingLines.Count > 0)
        {
            Debug.Log("1");
            battleSceneText.text = upComingLines[0];
            upComingLines.RemoveAt(0);
        }
        else if (beginOfBattle == true || (opponentHasAttacked == true && battleIsOver == false))
        {
            Debug.Log("2");
            battleSceneText.text = $"Choose a move:";

            continueTextButton.enabled = false;

            beginOfBattle = false;
            playerCanAttack = true;
            opponentHasAttacked = false;
        }
        else if (opponentHasAttacked == false && playerTurn == false && battleIsOver == false)
        {
            Debug.Log("3");
            OpponentAttack();
        }
        else if (battleIsOver == true && experiencePointsCalculated == false)
        {
            Debug.Log("4");
            CalculateExperiencePoints();
        }
        else
        {
            Debug.Log("5");
            GoBack();
        }
    }

    #endregion
}
