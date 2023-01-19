using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region UIElements

    [Header("Short message")]
    [SerializeField] Canvas levelOverlayCanvas;
    [SerializeField] Image levelOverlayPanel;
    [SerializeField] TextMeshProUGUI shortMessageText;

    [Header("Menu")]
    [SerializeField] Image menuPanel;
    [SerializeField] TextMeshProUGUI menuText;
    [SerializeField] Image stickmonPanel;

    [Header("First Stickmon")]
    [SerializeField] Canvas firstStickmonCanvas;
    [SerializeField] Image firstStickmonImage;

    [SerializeField] TextMeshProUGUI firstStickmonNameText;
    [SerializeField] TextMeshProUGUI firstStickmonLevelHealthText;

    [SerializeField] TextMeshProUGUI firstStickmonFirstMoveText;
    [SerializeField] TextMeshProUGUI firstStickmonSecondMoveText;
    [SerializeField] TextMeshProUGUI firstStickmonThirdMoveText;

    [Header("Second Stickmon")]
    [SerializeField] Canvas secondStickmonCanvas;
    [SerializeField] Image secondStickmonImage;
    [SerializeField] Button assignSecondStickMonFirstButton;

    [SerializeField] TextMeshProUGUI secondStickmonNameText;
    [SerializeField] TextMeshProUGUI secondStickmonLevelHealthText;

    [SerializeField] TextMeshProUGUI secondStickmonFirstMoveText;
    [SerializeField] TextMeshProUGUI secondStickmonSecondMoveText;
    [SerializeField] TextMeshProUGUI secondStickmonThirdMoveText;

    [Header("Third Stickmon")]
    [SerializeField] Canvas thirdStickmonCanvas;
    [SerializeField] Image thirdStickmonImage;
    [SerializeField] Button assignThirdStickMonFirstButton;

    [SerializeField] TextMeshProUGUI thirdStickmonNameText;
    [SerializeField] TextMeshProUGUI thirdStickmonLevelHealthText;

    [SerializeField] TextMeshProUGUI thirdStickmonFirstMoveText;
    [SerializeField] TextMeshProUGUI thirdStickmonSecondMoveText;
    [SerializeField] TextMeshProUGUI thirdStickmonThirdMoveText;

    #endregion

    #region AllVariables

    #region GeneralVariables

    Rigidbody2D myBody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    //For toggling the menu
    private bool menuIsOpen = false;

    #endregion

    #region MovingVeriables

    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;

    //Keeps track wether the player is moving or not
    private bool isWalking = false;

    //
    public ContactFilter2D movementFilter;

    //The direction the player wants to move into
    Vector2 movementInput;

    //Prevents the walking animation from playing when walking against a collider
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    #endregion

    #endregion

    #region StartUp

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        ToggleOverlay("disable");
        GetCurrentGameState();
        SaveProgress();
    }

    /// <summary>
    /// Detects if the user has been on the battleScene before switching back to this scene
    /// </summary>
    private void GetCurrentGameState()
    {
        if (PlayerPrefs.HasKey("GameState"))
        {
            float positionX = PlayerPrefs.GetFloat("PlayerPositionX");
            float positionY = PlayerPrefs.GetFloat("PlayerPositionY");

            //TODO: Test
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

    private void Update()
    {
        //If the Q key is pressed, a menu will either appear or hide based on a boolean
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            ToggleMenu();
        }

        //if (Input.GetKeyDown(KeyCode.H) == true)
        //{
        //    HealStickmon();
        //}
    }

    private void FixedUpdate()
    {
        #region Moving

        //If a key used for moving is being pressed
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

        if (movementInput.x < 0)
        {
            mySpriteRenderer.flipX = true;
            PlayerPrefs.SetString("isFlipped", "true");

        }
        else if (movementInput.x > 0)
        {
            mySpriteRenderer.flipX = false;
            PlayerPrefs.SetString("isFlipped", "false");
        }

        #endregion                
    }

    #region Moving

    //If the player can move and it not walking against a collider, this will return true
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

        }
        else
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

    /// <summary>
    /// Detects continuous collision with a collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Based on the grass area the max level can differ
        if (collision.tag == "FirstGrass")
        {
            PlayerPrefs.SetInt("MaxEncounterLevel", 7);
            if (isWalking == true)
            {
                CheckGrass("firstArea");
            }
        }

        if (collision.tag == "SecondGrass")
        {
            PlayerPrefs.SetInt("MaxEncounterLevel", 15);
            if (isWalking == true)
            {
                CheckGrass("firstArea");
            }
        }

        //If the player is in contact with the collider with this tag, an arranged battle will start
        if (collision.tag == "FirstBattleDetector")
        {
            if (Input.GetKeyDown(KeyCode.E) == true)
            {
                PlayerPrefs.SetInt("MaxLevel", 13);
                StartBattle("SetBattle");
            }
        }
    }

    /// <summary>
    /// Gets a random number between 0 and 150 and if that random number is 5, a random ecnounter will appear. 
    /// Based in the area there will be a max level
    /// </summary>
    /// <param name="area"></param>
    private void CheckGrass(string area)
    {
        float encounter = Random.Range(0, 150);        

        if (encounter == 5)
        {
            PlayerPrefs.SetString("EncounterArea", area);
            StartBattle("Encounter");
        }
    }

    /// <summary>
    /// Saves the current player positions for when the player switches back to this scene and loads the other scene.
    /// It also defines wether the current battle is a random encounter or an arranged battle
    /// </summary>
    /// <param name="battleType"></param>
    private void StartBattle(string battleType)
    {
        PlayerPrefs.SetFloat("PlayerPositionX", myBody.position.x);
        PlayerPrefs.SetFloat("PlayerPositionY", myBody.position.y);
        PlayerPrefs.SetString("GameState", "BattleScene");

        if (battleType == "Encounter")
        {
            PlayerPrefs.SetString("BattleType", "Encounter");
        }
        else
        {
            PlayerPrefs.SetString("BattleType", "SetBattle");
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("BattleScene");
    }

    #endregion

    #region HealingStickmon

    /// <summary>
    /// When the player is in contact with the healer gameobject it heals all the Stickmon
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Healer")
        {
            HealStickmon();
        }
    }

    /// <summary>
    /// Heals all the Stickmon if they're not at full health already.
    /// Also shows a message after healing or not.
    /// </summary>
    private void HealStickmon()
    {
        bool allFullHealth = true;

        List<CurrentStickmon> allAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon();
        foreach (CurrentStickmon currentStickmon in allAlliedStickmon)
        {
            if (currentStickmon.GetCurrentHealthPoints() != currentStickmon.GetMaxHealthPoints())
            {
                allFullHealth = false;
            }
        }

        if (allFullHealth == false)
        {
            GameManagerScript.myGameManagerScript.HealAllAlliedStickmon();
            shortMessageText.text = $"Your Stickmon have been successfully healed!";
        }
        else
        {
            shortMessageText.text = $"Your Stickmon are already fully healed!";
        }
        ToggleOverlay("message");
        StartCoroutine(HideMessage());
    }

    #endregion

    #region UIFunctions

    #region ShortMessage

    /// <summary>
    /// Hides the message regarding healing after two seconds this function has been called
    /// </summary>
    /// <returns></returns>
    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2);
        ToggleOverlay("disable");
    }

    #endregion

    #region ManuPanel

    /// <summary>
    /// Defines what UI elements will show what data on the menu
    /// </summary>
    private void DefineMenuData()
    {
        List<CurrentStickmon> allAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon();

        for (int count = 0; count < allAlliedStickmon.Count; count++)
        {
            CurrentStickmon currentAlliedStickmon = allAlliedStickmon[count];

            if (count == 0)
            {
                firstStickmonImage.sprite = currentAlliedStickmon.GetNormalStickmonImage();
                firstStickmonNameText.text = currentAlliedStickmon.GetStickmonName();
                firstStickmonLevelHealthText.text = $"Level: {currentAlliedStickmon.GetStickmonLevel()}, {currentAlliedStickmon.GetCurrentHealthPoints()}/{currentAlliedStickmon.GetMaxHealthPoints()}";

                int amountOfMoves = currentAlliedStickmon.GetAllStickmonMoves().Count;

                firstStickmonFirstMoveText.text = currentAlliedStickmon.GetStickmonMove(0).GetMoveName();
                if (amountOfMoves > 1)
                {
                    firstStickmonSecondMoveText.text = currentAlliedStickmon.GetStickmonMove(1).GetMoveName();
                }
                if (amountOfMoves > 2)
                {
                    firstStickmonThirdMoveText.text = currentAlliedStickmon.GetStickmonMove(2).GetMoveName();
                }
            }
            if (count == 1)
            {
                secondStickmonImage.sprite = currentAlliedStickmon.GetNormalStickmonImage();
                secondStickmonNameText.text = currentAlliedStickmon.GetStickmonName();
                secondStickmonLevelHealthText.text = $"Level: {currentAlliedStickmon.GetStickmonLevel()}, {currentAlliedStickmon.GetCurrentHealthPoints()}/{currentAlliedStickmon.GetMaxHealthPoints()}";

                int amountOfMoves = currentAlliedStickmon.GetAllStickmonMoves().Count;
                secondStickmonFirstMoveText.text = currentAlliedStickmon.GetStickmonMove(0).GetMoveName();
                if (amountOfMoves > 1)
                {
                    secondStickmonSecondMoveText.text = currentAlliedStickmon.GetStickmonMove(1).GetMoveName();
                }
                if (amountOfMoves > 2)
                {
                    secondStickmonThirdMoveText.text = currentAlliedStickmon.GetStickmonMove(2).GetMoveName();
                }
            }
            if (count == 2)
            {
                thirdStickmonImage.sprite = currentAlliedStickmon.GetNormalStickmonImage();
                thirdStickmonNameText.text = currentAlliedStickmon.GetStickmonName();
                thirdStickmonLevelHealthText.text = $"Level: {currentAlliedStickmon.GetStickmonLevel()}, {currentAlliedStickmon.GetCurrentHealthPoints()}/{currentAlliedStickmon.GetMaxHealthPoints()}";

                int amountOfMoves = currentAlliedStickmon.GetAllStickmonMoves().Count;
                thirdStickmonFirstMoveText.text = currentAlliedStickmon.GetStickmonMove(0).GetMoveName();
                if (amountOfMoves > 1)
                {
                    thirdStickmonSecondMoveText.text = currentAlliedStickmon.GetStickmonMove(1).GetMoveName();
                }
                if (amountOfMoves > 2)
                {
                    thirdStickmonThirdMoveText.text = currentAlliedStickmon.GetStickmonMove(2).GetMoveName();
                }
            }
        }
    }

    #endregion

    #region ToggleFunctions

    /// <summary>
    /// Toggles the canvas over the walking scene.
    /// That canvas will either show the message for healing or the menu
    /// </summary>
    /// <param name="status"></param>
    private void ToggleOverlay(string status)
    {
        if (status == "message")
        {
            levelOverlayCanvas.enabled = true;
            levelOverlayPanel.enabled = true;
            shortMessageText.enabled = true;
        }
        else if (status == "menu")
        {
            levelOverlayCanvas.enabled = true;
            menuPanel.enabled = true;
            stickmonPanel.enabled = true;

            int amountOfStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon().Count;
            if (amountOfStickmon > 0)
            {
                firstStickmonCanvas.enabled = true;
            }
            if (amountOfStickmon > 1)
            {
                secondStickmonCanvas.enabled = true;
            }
            if (amountOfStickmon > 2)
            {
                thirdStickmonCanvas.enabled = true;
            }
        }
        else if (status == "disable")
        {
            levelOverlayCanvas.enabled = false;
            menuPanel.enabled = false;
            menuText.enabled = false;
            stickmonPanel.enabled = false;

            firstStickmonCanvas.enabled = false;
            secondStickmonCanvas.enabled = false;
            thirdStickmonCanvas.enabled = false;

            levelOverlayPanel.enabled = false;
            shortMessageText.enabled = false;
        }
    }

    /// <summary>
    /// Appears and disappears the menu based on a boolean.
    /// When the menu is open, the player can't move the character
    /// </summary>
    private void ToggleMenu()
    {
        if (menuIsOpen == false)
        {
            InputSystem.DisableDevice(Keyboard.current);
            DefineMenuData();
            ToggleOverlay("menu");
            menuIsOpen = true;
        }
        else
        {
            InputSystem.EnableDevice(Keyboard.current);
            ToggleOverlay("disable");
            menuIsOpen = false;
        }
    }

    #endregion

    #region ChangeStickmonPositions

    /// <summary>
    /// Assigns either the second or third Stickmon as the first one.
    /// </summary>
    /// <param name="position"></param>
    public void AssignAsFirstStickmon(string position)
    {
        CurrentStickmon currentFirstStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
        GameManagerScript.myGameManagerScript.SwapStickmonPositions(currentFirstStickmon, position);
        DefineMenuData();        
    }

    #endregion

    #endregion

    #region Save

    /// <summary>
    /// Checks the current allied Stickmon and based on that calls another function to save them with PlayerPrefs
    /// </summary>
    private void SaveProgress()
    {
        int amountOfStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon().Count;
        PlayerPrefs.SetInt("AmountOfStickmon", amountOfStickmon);

        CurrentStickmon currentAlliedStickmon = GameManagerScript.myGameManagerScript.GetFirstAlliedStickmon();
        SaveAlliedStickmon("First", currentAlliedStickmon);
        if (amountOfStickmon > 1)
        {
            currentAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[1];
            SaveAlliedStickmon("Second", currentAlliedStickmon);
        }
        if (amountOfStickmon > 2)
        {
            currentAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon()[2];
            SaveAlliedStickmon("Third", currentAlliedStickmon);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Based on data given by parameters, this will save the data in the PlayerPrefs
    /// </summary>
    /// <param name="position"></param>
    /// <param name="currentAlliedStickmon"></param>
    private void SaveAlliedStickmon(string position, CurrentStickmon currentAlliedStickmon)
    {
        List<StickmonMove> allCurrentStickmonMoves = currentAlliedStickmon.GetAllStickmonMoves();

        PlayerPrefs.SetInt($"{position}StickmonLevel", currentAlliedStickmon.GetStickmonLevel());

        PlayerPrefs.SetFloat($"{position}StickmonCurrentHealth", currentAlliedStickmon.GetCurrentHealthPoints());
        PlayerPrefs.SetFloat($"{position}StickmonMaxHealth", currentAlliedStickmon.GetMaxHealthPoints());
        PlayerPrefs.SetFloat($"{position}StickmonExperiencePoints", currentAlliedStickmon.GetExperiencePoints());

        PlayerPrefs.SetString($"{position}StickmonName", currentAlliedStickmon.GetStickmonName());
        PlayerPrefs.SetString($"{position}StickmonFirstMove", allCurrentStickmonMoves[0].GetMoveName());
        PlayerPrefs.SetString($"{position}StickmonSecondMove", allCurrentStickmonMoves[1].GetMoveName());

        if (allCurrentStickmonMoves.Count > 2)
        {
            PlayerPrefs.SetString($"{position}StickmonThidrMove", allCurrentStickmonMoves[2].GetMoveName());
        }
    }

    #endregion
}
