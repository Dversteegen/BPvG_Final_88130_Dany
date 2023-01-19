using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManagerScript : MonoBehaviour
{
    //With this being static I can access it from every script, so all the data won't get lost
    public static GameManagerScript myGameManagerScript;

    //List of objects
    private List<StickmonMove> myStickmonMoves;
    private List<Stickmon> myStickmon;
    private List<CurrentStickmon> myAlliedStickmon;    

    public GameManagerScript()
    {
        myStickmonMoves = new List<StickmonMove>();
        myStickmon = new List<Stickmon>();
        myAlliedStickmon = new List<CurrentStickmon>();
    }
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Awake()
    {
        MakeThisTheOnlyGameManager();
    }    

    //Prevents multiple objects of this type getting made
    void MakeThisTheOnlyGameManager()
    {
        if (myGameManagerScript == null)
        {
            DontDestroyOnLoad(gameObject);
            myGameManagerScript = this;
        }
        else
        {
            if (myGameManagerScript != this){
                Destroy(gameObject);
            }
        }
    }    

    #region Stickmon

    public void AddStickmon(Stickmon newStickmon)
    {
        myStickmon.Add(newStickmon);
    }

    //Check
    public Stickmon GetFirstStickmon()
    {
        Stickmon firstStickmon = myStickmon.Where(stickmon => stickmon.GetStickmonName() == "Julian").Single();
        //foreach (Stickmon stickmon in myStickmon)
        //{
        //    if (stickmon.GetStickmonName() == "Julian")
        //    {
        //        return stickmon;
        //    }
        //}
        return firstStickmon;
    }

    /// <summary>
    /// Returns a random Stickmon which is not Julian
    /// </summary>
    /// <returns></returns>
    public Stickmon GetRandomStickmon()
    {
        List<Stickmon> allOpponentStickmon = myStickmon.Where(stickmon => stickmon.GetStickmonName() != "Julian").ToList();

        Stickmon currentStickmon = allOpponentStickmon[Random.Range(0, allOpponentStickmon.Count)];
        return currentStickmon;
    }

    /// <summary>
    /// Returns the Stickmon with the name Paul
    /// </summary>
    /// <returns></returns>
    public Stickmon GetStickmonPaul()
    {
        foreach (Stickmon stickmon in myStickmon)
        {
            if (stickmon.GetStickmonName() == "Paul")
            {
                return stickmon;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns either the normal or the back sprite of the Stickmon belonging to the name
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Sprite GetStickmonByImageName(string name, string type)
    {
        Stickmon currentStickmon = myStickmon.Where(stickmon => stickmon.GetStickmonName() == name).Single();

        if (type == "normal")
        {
            return currentStickmon.GetStickmonImage("normal");
        }
        else
        {
            return currentStickmon.GetStickmonImage("back");
        }
    }

    #endregion

    #region StickmonMoves

    public void AddStickmonMove(StickmonMove newStickmonMove)
    {
        myStickmonMoves.Add(newStickmonMove);
    }

    /// <summary>
    /// Returns a random StickmonMove which is withing the two values
    /// </summary>
    /// <param name="minimumDamage"></param>
    /// <param name="maximumDamage"></param>
    /// <returns></returns>
    public StickmonMove GetRandomStickmonMove(int minimumDamage, int maximumDamage)
    {
        List<StickmonMove> allPossibleMoves = myStickmonMoves
            .Where(move => move.GetAmountOfDamage() > minimumDamage && move.GetAmountOfDamage() < maximumDamage)
            .Select(move => move).ToList();

        return allPossibleMoves[Random.Range(0, allPossibleMoves.Count)];
    }

    /// <summary>
    /// Returns a random StickmonMove which is witin the two values and not included in the list given as parameter
    /// </summary>
    /// <param name="minimumDamage"></param>
    /// <param name="maximumDamage"></param>
    /// <param name="currentStickmonMoves"></param>
    /// <returns></returns>
    public StickmonMove GetNewRandomStickmonMove(int minimumDamage, int maximumDamage, List<StickmonMove> currentStickmonMoves)
    {
        List<StickmonMove> allPossibleMoves = myStickmonMoves
            .Where(move => move.GetAmountOfDamage() > minimumDamage && move.GetAmountOfDamage() < maximumDamage && currentStickmonMoves.Contains(move) == false)
            .Select(move => move).ToList();

        return allPossibleMoves[Random.Range(0, allPossibleMoves.Count)];
    }

    public List<StickmonMove> GetAllStickmonMoves()
    {
        return myStickmonMoves;
    }

    /// <summary>
    /// Returns a StickmonMove by name
    /// </summary>
    /// <param name="moveName"></param>
    /// <returns></returns>
    public StickmonMove GetStickmonMoveByName(string moveName)
    {
        StickmonMove currentMove = myStickmonMoves.Where(move => move.GetMoveName() == moveName).Single();
        return currentMove;
    }

    #endregion

    #region AlliedStickmon

    #region AddAlliedStickmon

    public void AddFirstStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    public void AddAlliedStickmon(CurrentStickmon alliedStickmon)
    {
        myAlliedStickmon.Add(alliedStickmon);
    }

    #endregion

    #region GetFunctions

    /// <summary>
    /// Gets the Stickmon who's at that moment the first one in the list
    /// </summary>
    /// <returns></returns>
    public CurrentStickmon GetFirstAlliedStickmon()
    {
        return myAlliedStickmon[0];
    }

    /// <summary>
    /// Returns the first healthy Stickmon
    /// </summary>
    /// <returns></returns>
    public CurrentStickmon GetFirstHealthyAllliedStickmon()
    {
        foreach (CurrentStickmon alliedStickmon in myAlliedStickmon)
        {
            if (alliedStickmon.GetCurrentHealthPoints() > 0)
            {
                return alliedStickmon;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns all allied Stickmon
    /// </summary>
    /// <returns></returns>
    public List<CurrentStickmon> GetAllAlliedStickmon()
    {
        return myAlliedStickmon;
    }

    /// <summary>
    /// Returns the amount of healthy Stickmon
    /// </summary>
    /// <returns></returns>
    public int GetAmountOfHealthyStickmon()
    {
        int amountOfHealthyStickmon = 0;
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            if (currentStickmon.GetCurrentHealthPoints() > 0)
            {
                amountOfHealthyStickmon++;
            }
        }
        return amountOfHealthyStickmon;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Heals all allied the Stickmon
    /// </summary>
    public void HealAllAlliedStickmon()
    {
        foreach (CurrentStickmon currentStickmon in myAlliedStickmon)
        {
            currentStickmon.HealStickmon();
        }        
    }

    /// <summary>
    /// Swaps positions between the first and either the second or third Stickmon
    /// </summary>
    /// <param name="currentFirstStickmon"></param>
    /// <param name="position"></param>
    public void SwapStickmonPositions(CurrentStickmon currentFirstStickmon, string position)
    {
        if (position == "second")
        {
            myAlliedStickmon[0] = myAlliedStickmon[1];
            myAlliedStickmon[1] = currentFirstStickmon;
        }
        else
        {
            myAlliedStickmon[0] = myAlliedStickmon[2];
            myAlliedStickmon[2] = currentFirstStickmon;
        }        
    }

    #endregion

    #endregion
}
