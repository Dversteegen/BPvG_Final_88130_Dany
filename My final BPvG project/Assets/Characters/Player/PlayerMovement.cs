using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    #region AllVariables

    #region GeneralVariables

    Rigidbody2D myBody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    #endregion

    #region MovingVeriables

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    private bool isWalking;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;    
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    #endregion

    #region StickmonVariables    

    #endregion

    #endregion
    
    //GameManagerScript GameManagerScript.myGameManagerScript;

    #region StartUp

    // Start is called before the first frame update
    void Start()
    {        
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();        

        GetCurrentGameState();
        //SetupData();
    }

    //#region SetupFunctions

    //private void SetupData()
    //{
    //    AddAllStickmonMoves();
    //    AddAllStickmons();
    //    AddFirstStickmon();
    //}

    //#region AddFunctions

    //private void AddAllStickmonMoves()
    //{
    //    StickmonMove newStickmonMove = new StickmonMove("Punch", 4);        
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Kick", 5);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Push", 3);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Beat up", 6);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Pray", 0);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Sleep", 0);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Break", 8);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Squash", 7);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);

    //    newStickmonMove = new StickmonMove("Hit brick", 4);
    //    GameManagerScript.myGameManagerScript.AddStickmonMove(newStickmonMove);
    //}

    //private void AddAllStickmons()
    //{
    //    Stickmon newStickmon = new Stickmon("Larry", GameManagerScript.myGameManagerScript.GetAllStickmonMoves());
    //    GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

    //    newStickmon = new Stickmon("Julian", GameManagerScript.myGameManagerScript.GetAllStickmonMoves());
    //    GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

    //    newStickmon = new Stickmon("Griffin", GameManagerScript.myGameManagerScript.GetAllStickmonMoves());
    //    GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);

    //    newStickmon = new Stickmon("Paul", GameManagerScript.myGameManagerScript.GetAllStickmonMoves());
    //    GameManagerScript.myGameManagerScript.AddStickmon(newStickmon);
    //}

    //private void AddFirstStickmon()
    //{
    //    Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();
    //    StickmonMove randomStickmonMove = GameManagerScript.myGameManagerScript.GetRandomStandardStickmonMove();

    //    List<StickmonMove> allCurrentMoves = new List<StickmonMove>();
    //    allCurrentMoves.Add(randomStickmonMove);

    //    CurrentStickmon newAlliedStickmon = new CurrentStickmon(firstStickmon.GetStickmonName(), 5, 0, allCurrentMoves);
    //    GameManagerScript.myGameManagerScript.AddFirstStickmon(newAlliedStickmon);
    //}

    //#endregion

    //#endregion

    /// <summary>
    /// Detects if the user has been on the battleScene before switching back to this scene
    /// </summary>
    private void GetCurrentGameState()
    {
        if (PlayerPrefs.GetString("GameState") == "BattleScene")
        {
            float positionX = PlayerPrefs.GetFloat("PlayerPositionX");
            float positionY = PlayerPrefs.GetFloat("PlayerPositionY");

            if (PlayerPrefs.GetString("isFlipped") == "true")
            {
                mySpriteRenderer.flipX = true;
            }

            PlayerPrefs.SetString("GameState", "StartScene");
            PlayerPrefs.Save();

            Vector3 oldPosition = new Vector3(positionX, positionY);
            transform.position = oldPosition;
        }
        else
        {
            Vector3 oldPosition = new Vector3(-6.1f, 4.4f);
            transform.position = oldPosition;
        }
    }

    #endregion    

    private void FixedUpdate()
    {
        #region Moving

        //Moving
        if (movementInput != Vector2.zero)
        {
            bool canMove = TryMove(movementInput);

            if (!canMove)
            {
                canMove = TryMove(new Vector2(movementInput.x, 0));                
            }

            if (!canMove)
            {
                canMove = TryMove(new Vector2(0, movementInput.y));
            }

            myAnimator.SetBool("isMoving", canMove);
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }

        if(movementInput.x < 0)
        {
            mySpriteRenderer.flipX = true;
            PlayerPrefs.SetString("isFlipped", "true");

        } else if (movementInput.x > 0)
        {
            mySpriteRenderer.flipX = false;
            PlayerPrefs.SetString("isFlipped", "false");
        }

        #endregion                
    }

    #region Moving

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = myBody.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                isWalking = true;

                myBody.MovePosition(myBody.position + direction * moveSpeed * Time.fixedDeltaTime);
                return isWalking;
            }
            isWalking = false;
            return isWalking;

        } else
        {
            return isWalking;
        }
    }

    void OnMove(InputValue movementValue)        
    {
        movementInput = movementValue.Get<Vector2>();        
    }

    #endregion

    #region BattleScene    

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Grass")
        {
            if (isWalking == true)
            {                
                CheckGrass();
            }            
        }
    }

    private void CheckGrass()
    {

        //Debug.Log(GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName());
        float encounter = Random.Range(0, 299);
        //Debug.Log("Hoi");
        if (encounter == 5)
        {
            FoundEncounter();
        }
    }

    private void FoundEncounter()
    {
        PlayerPrefs.SetFloat("PlayerPositionX", myBody.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", myBody.position.y);
        PlayerPrefs.SetString("GameState", "BattleScene");
        //DetermineEncounter();
        PlayerPrefs.Save();

        SceneManager.LoadScene("BattleScene");
    }

    private void DetermineEncounter()
    {
        PlayerPrefs.SetString("Encounter", GameManagerScript.myGameManagerScript.GetRandomStickmon().GetStickmonName());

        List<CurrentStickmon> allAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon();

        for (int count = 0; count < allAlliedStickmon.Count; count++)
        {
            switch (count)
            {
                case 0:
                    PlayerPrefs.SetString("FirstStickmonName", allAlliedStickmon[count].GetStickmonName());
                    PlayerPrefs.SetInt("FirstStickmonLevel", allAlliedStickmon[count].GetStickmonLevel());
                    PlayerPrefs.SetInt("FirstStickmonExperiencePoints", allAlliedStickmon[count].GetExperiencePoints());
                    if (allAlliedStickmon[count].GetAmountOfStickmonMoves() == 1)
                    {
                        PlayerPrefs.SetString("FirstStickmonFirstMoveName", allAlliedStickmon[count].GetStickmonMove(0).GetMoveName());
                        PlayerPrefs.SetInt("FirstStickmonFirstMoveDamage", allAlliedStickmon[count].GetStickmonMove(0).GetAmountOfDamage());
                    }
                    
                    break;
            }
        }
        
        //PlayerPrefs.SetString("FirstStickmonSecondMove", );
        //PlayerPrefs.SetString("FirstStickmonFirstMove", );
    }

    #endregion
}
