using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    #endregion

    #region Sprites

    [Header("Sprites")]
    public Sprite larrySprite;
    public Sprite paulSprite;
    public Sprite griffinSprite;
    public Sprite julianSprite;

    #endregion

    //private void Start()
    void Start()
    {
        //This needs to be above the GetComponent lines for the text, god knows why
        UpdateBattleScene();

        playerName = GetComponent<TextMeshProUGUI>();
        opponentName = GetComponent<TextMeshProUGUI>();

        firstMove = GetComponent<TextMeshProUGUI>();
        secondMove = GetComponent<TextMeshProUGUI>();
        thirdMove = GetComponent<TextMeshProUGUI>();
    }

    #region UpdatebattleScene

    private void UpdateBattleScene()
    {
        Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
        UpdateOpponent(encounter);

        CurrentStickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
        UpdatePlayer(firstStickmon);
    }

    #region UpdatePlayers

    private void UpdatePlayer(CurrentStickmon firstStickmon)
    {
        switch (firstStickmon.GetStickmonName())
        {
            case "Julian":
                playerImage.sprite = julianSprite;
                break;
        }
        playerName.text = firstStickmon.GetStickmonName();
        
        List<StickmonMove> allStickmonMoves = firstStickmon.GetAllStickmonMoves();

        UpdatePlayerMoves(allStickmonMoves);
        UpdatePlayerData(firstStickmon);
    }

    private void UpdatePlayerMoves(List<StickmonMove>allStickmonMoves)
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
        }

        if (allStickmonMoves.Count > 2)
        {
            thirdMove.text = allStickmonMoves[2].GetMoveName();
        }
        else
        {
            thirdMove.text = "";
        }
    }

    private void UpdatePlayerData(CurrentStickmon firstStickmon)
    {
        float maxHealth = firstStickmon.GetMaxHealthPoints();
        float currentHealth = firstStickmon.GetCurrentHealthPoints();
        float level = firstStickmon.GetStickmonLevel();

        playerLevel.text = $"Level: {level}";
        playerHealth.text = $"Health: {currentHealth}/{maxHealth}";        
    }

    #endregion

    #region UpdateOpponent

    private void UpdateOpponent(Stickmon encounter)
    {
        switch (encounter.GetStickmonName())
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
        opponentName.text = encounter.GetStickmonName();

        UpdateOpponentData(encounter);
    }

    private void UpdateOpponentData(Stickmon encouter)
    {
        float baseHealth = encouter.GetBaseHealthPoints();
        CurrentStickmon firstStickmon = GetFirstStickmon();

        int encounterLevel = GetEncounterLevel(firstStickmon.GetStickmonLevel());

        float maxHealth = GetHealthPoints(encounterLevel, baseHealth);

        opponentLevel.text = $"Level: {encounterLevel}";
        opponentHealth.text = $"Health: {maxHealth}/{maxHealth}";
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
                encounterLevel+= 2;
                break;

            case 2:                
                break;

            case 3:
                encounterLevel-=2;
                break;

            case 4:
                encounterLevel--;
                break;
        }
        return encounterLevel;
    }

    private float GetHealthPoints(int encounterLevel, float baseHealth)
    {
        float maxHealth = encounterLevel * baseHealth;

        for (int count = 0; count < encounterLevel; count++)
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

    #endregion

    #endregion

    #region GetFunctions

    private CurrentStickmon GetFirstStickmon()
    {
        return GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
    }

    #endregion
}
