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
    public ContactFilter2D movementFilter;
    Vector2 movementInput;    
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    #endregion

    #region StickmonVariables

    StickmonManager myStickmonManager;

    #endregion

    #endregion

    #region StartUp

    // Start is called before the first frame update
    void Start()
    {        
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        GetCurrentGameState();
        SetupData();
    }

    #region SetupFunctions

    private void SetupData()
    {
        myStickmonManager = new StickmonManager();

        AddAllStickmonMoves();
        AddAllStickmons();
    }

    #region AddFunctions

    private void AddAllStickmonMoves()
    {
        StickmonMove newStickmonMove = new StickmonMove("Punch", 5);
        myStickmonManager.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Kick", 6);
        myStickmonManager.AddStickmonMove(newStickmonMove);

        newStickmonMove = new StickmonMove("Push", 3);
        myStickmonManager.AddStickmonMove(newStickmonMove);
    }

    private void AddAllStickmons()
    {
        Stickmon newStickmon = new Stickmon("Larry", myStickmonManager.GetAllStickmonMoves());
        myStickmonManager.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Griffin", myStickmonManager.GetAllStickmonMoves());
        myStickmonManager.AddStickmon(newStickmon);

        newStickmon = new Stickmon("Paul", myStickmonManager.GetAllStickmonMoves());
        myStickmonManager.AddStickmon(newStickmon);
    }

    #endregion

    #endregion

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
                myBody.MovePosition(myBody.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            return false;
        } else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)        
    {
        movementInput = movementValue.Get<Vector2>();        
    }

    #endregion

    #region BattleScene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string currentGrass = PlayerPrefs.GetString("CurrentGrass");

        if (collision.tag == "Grass" && collision.name != currentGrass)
        {
            PlayerPrefs.SetString("CurrentGrass", collision.name);            
            CheckGrass();            
        }
    }    

    private void CheckGrass()
    {        
        float encounter = Random.Range(0, 19);
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
        DetermineEncounter();
        PlayerPrefs.Save();

        SceneManager.LoadScene("BattleScene");
    }

    private void DetermineEncounter()
    {
        PlayerPrefs.SetString("Encounter", myStickmonManager.GetRandomStickmon().GetStickmonName());
        PlayerPrefs.SetString("FirstStickmon", "Julian");
    }

    #endregion
}
