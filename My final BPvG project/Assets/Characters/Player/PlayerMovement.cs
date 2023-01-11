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
    }

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
        float encounter = Random.Range(0, 249);
        
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
        PlayerPrefs.Save();

        SceneManager.LoadScene("BattleScene");
    }   

    #endregion
}
