using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class StartScript : MonoBehaviour
{
    #region Buttons

    [SerializeField] Button ContinueGameButton;
    [SerializeField] TextMeshProUGUI ContinueGameButtonText;

    #endregion

    #region Sprites

    #region Normal

    [Header("Normal Sprites")]

    [SerializeField] Sprite julianSprite;
    [SerializeField] Sprite larrySprite;
    [SerializeField] Sprite paulSprite;
    [SerializeField] Sprite griffinSprite;
    [SerializeField] Sprite emilySprite;
    [SerializeField] Sprite bellaSprite;

    #endregion

    #region Back

    [Header("Back Sprites")]

    [SerializeField] Sprite julianBackSprite;
    [SerializeField] Sprite larryBackSprite;
    [SerializeField] Sprite paulBackSprite;
    [SerializeField] Sprite griffinBackSprite;
    [SerializeField] Sprite emilyBackSprite;
    [SerializeField] Sprite bellaBackSprite;

    #endregion

    #endregion

    #region Variables

    //When a Stickmon has experiencePoints over one of these values, it gains a level
    private int[] allLevelBarriers = new int[] { 5, 11, 18, 26, 35, 47, 61, 78, 98, 123, 153 };

    #endregion

    private void Start()
    {
        if (PlayerPrefs.HasKey("GameState") == false)
        {
            DisableContinueButton();
        }
    }

    #region SetupFunctions

    public void SetupData()
    {
        AddAllStickmonMoves();
        AddAllStickmons();
    }

    #region AddFunctions

    private void AddAllStickmonMoves()
    {
        StickmonMove newStickmonMove = new StickmonMove("Punch", 4);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Kick", 5);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Push", 3);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Beat up", 6);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Break", 8);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Squash", 7);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Spit", 1);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Elbow hit", 6);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Crush", 11);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Knee Jab", 7);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Headbutt", 8);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Scratch", 3);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Stomp", 4);
        GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);
    }

    private void AddAllStickmons()
    {
        Stickmon newStickmon = new Stickmon("Larry", 12, larrySprite, larryBackSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);        

        newStickmon = new Stickmon("Julian", 9, julianSprite, julianSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Griffin", 10, griffinSprite, griffinBackSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);        

        newStickmon = new Stickmon("Paul", 8, paulSprite, paulBackSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);        

        newStickmon = new Stickmon("Bella", 11, bellaSprite, bellaBackSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);        

        newStickmon = new Stickmon("Emily", 9, emilySprite, emilyBackSprite);
        GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);        
    }

    /// <summary>
    /// Add the very first Stickmon to the list, this one will always be the same one, but it's max health points can vary.
    /// The same goes for the two Stickmon moves he gets
    /// </summary>
    private void AddFirstStickmon()
    {
        Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();
        float maxHealthPoints = firstStickmon.GetBaseHealthPoints();

        for (int count = 0; count < 3; count++)
        {
            int randomNumber = Random.Range(0, 3);

            if (randomNumber == 0)
            {
                maxHealthPoints++;
            }
            else if (randomNumber == 2)
            {
                maxHealthPoints += 2;
            }
        }

        StickmonMove randomStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStickmonMove(0, 6);
        List<StickmonMove> allCurrentMoves = new List<StickmonMove>();

        allCurrentMoves.Add(randomStickmonMove);
        randomStickmonMove = GameManagerScript.myGameManagerScript.GetNewRandomStickmonMove(7, 9, allCurrentMoves);
        allCurrentMoves.Add(randomStickmonMove);

        //Because the Stickmon starts at level three, the first two level barriers don't count and they will be removed before defining the CurrentStickmon
        int level = 3;
        int[] currentLevelbarriers = allLevelBarriers;

        for (int count = 0; count < level - 1; count++)
        {
            currentLevelbarriers = currentLevelbarriers.Where(levelBarrier => levelBarrier != currentLevelbarriers[0]).ToArray();
        }

        CurrentStickmon newAlliedStickmon = new CurrentStickmon(firstStickmon.GetStickmonName(), 3, 11, maxHealthPoints, allCurrentMoves, julianSprite, julianBackSprite, currentLevelbarriers);
        GameManagerScript.myGameManagerScript.AddFirstStickmon(newAlliedStickmon);
    }

    #endregion

    #endregion

    #region BasicMethods

    /// <summary>
    /// Starts a new game and removes all clears all the PlayerPrefs
    /// </summary>
    public void StartNewGame()
    {        
        SetupData();
        AddFirstStickmon();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartScene");
    }

    /// <summary>
    /// Lets you continue the game with the data stored in the PlayerPrefs
    /// </summary>
    public void ContinueGame()
    {
        SetupData();
        GetProgress();
        SceneManager.LoadScene("StartScene");
    }

    #endregion

    #region ToggleFunctions

    /// <summary>
    /// If there are no PlayerPrefs set, this will function will make sure the "Continue" button won't be enabled and appearing
    /// </summary>
    private void DisableContinueButton()
    {
        ContinueGameButton.enabled = false;
        ContinueGameButton.image.color = new Color(64, 64, 64, 0);
        ContinueGameButtonText.text = "";
    }

    #endregion

    #region LoadProgress

    /// <summary>
    /// Gets the data form the PlayerPrefs and defines them in the GameManager script
    /// </summary>
    private void GetProgress()
    {
        string[] positions = new string[] { "First", "Second", "Third" };
        int amountOfStickmon = PlayerPrefs.GetInt("AmountOfStickmon");
        
        for (int count = 0; count < amountOfStickmon; count++)
        {
            string stickmonName = PlayerPrefs.GetString($"{positions[count]}StickmonName");
            int stickmonLevel = PlayerPrefs.GetInt($"{positions[count]}StickmonLevel");
            Debug.Log($"Name: {stickmonName}");

            float stickmonCurrenthealth = PlayerPrefs.GetFloat($"{positions[count]}StickmonCurrentHealth");
            float stickmonMaxHealth = PlayerPrefs.GetFloat($"{positions[count]}StickmonMaxHealth");
            float experiencePoints = PlayerPrefs.GetFloat($"{positions[count]}StickmonExperiencePoints");

            List<StickmonMove> allStickmonMoves = GetAllCurrentStickmonMoves(positions, count);
            int[] allLevelBarriers = GetAllCurrentLevelBarriers(stickmonLevel);

            Sprite normalSprite = GetStickmonSprite("normal", stickmonName);
            Sprite backSprite = GetStickmonSprite("back", stickmonName);

            CurrentStickmon newAlliedStickmon = new CurrentStickmon(stickmonName, stickmonLevel, experiencePoints, stickmonCurrenthealth, stickmonMaxHealth, allStickmonMoves, normalSprite, backSprite, allLevelBarriers);
            GameManagerScript.myGameManagerScript.AddAlliedStickmon(newAlliedStickmon);
        }
    }

    /// <summary>
    /// Defines the StickmonMoves belonging to the Stickmon which correspondents with the string
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private List<StickmonMove> GetAllCurrentStickmonMoves(string[] positions, int count)
    {
        string firstMove = PlayerPrefs.GetString($"{positions[count]}StickmonFirstMove");
        string secondMove = PlayerPrefs.GetString($"{positions[count]}StickmonSecondMove");
        string thirdMove = "";
        if (PlayerPrefs.HasKey($"{positions[count]}StickmonThirdMove"))
        {
            thirdMove = PlayerPrefs.GetString($"{positions[count]}StickmonThirdMove");
        }
        List<StickmonMove> allStickmonMoves = new List<StickmonMove>();
        allStickmonMoves.Add(GameManagerScript.myGameManagerScript.GetStickmonMoveByName(firstMove));
        allStickmonMoves.Add(GameManagerScript.myGameManagerScript.GetStickmonMoveByName(secondMove));
        if (thirdMove != "")
        {
            allStickmonMoves.Add(GameManagerScript.myGameManagerScript.GetStickmonMoveByName(thirdMove));
        }

        return allStickmonMoves;
    }

    /// <summary>
    /// Removes level barriers based on the level of the Stickmon
    /// </summary>
    /// <param name="stickmonLevel"></param>
    /// <returns></returns>
    private int[] GetAllCurrentLevelBarriers(int stickmonLevel)
    {
        int[] currentLevelbarriers = allLevelBarriers;

        for (int countToLevel = 0; countToLevel < stickmonLevel - 1; countToLevel++)
        {
            currentLevelbarriers = currentLevelbarriers.Where(levelBarrier => levelBarrier != currentLevelbarriers[0]).ToArray();
        }
        return currentLevelbarriers;
    }

    /// <summary>
    /// Returns the sprites for the Stickmon, because you can't save those in PlayerPrefs
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private Sprite GetStickmonSprite(string type, string name)
    {
        Sprite currentSprite = GameManagerScript.myGameManagerScript.GetStickmonByImageName(name, type);
        return currentSprite;
    }

    #endregion    
}
