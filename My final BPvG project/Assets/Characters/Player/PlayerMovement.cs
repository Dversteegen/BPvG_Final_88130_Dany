using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region AllVariables

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

    [Header("Third Stickmon")]
    [SerializeField] Canvas secondStickmonCanvas;
    [SerializeField] Image secondStickmonImage;
    [SerializeField] TextMeshProUGUI secondStickmonNameText;
    [SerializeField] TextMeshProUGUI secondStickmonLevelHealthText;

    [Header("Second Stickmon")]
    [SerializeField] Canvas thirdStickmonCanvas;
    [SerializeField] Image thirdStickmonImage;
    [SerializeField] TextMeshProUGUI thirdStickmonNameText;
    [SerializeField] TextMeshProUGUI thirdStickmonLevelHealthText;

    #endregion

    #region GeneralVariables

    Rigidbody2D myBody;
    Animator myAnimator;
    SpriteRenderer mySpriteRenderer;

    private bool menuIsOpen = false;

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

    #region StartUp

    // Start is called before the first frame update
    void Start()
    {
        myBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

        ToggleOverlay("disable");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            ToggleMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartBattle("Encounter");
        }
    }

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Grass")
        {
            if (isWalking == true)
            {
                CheckGrass("firstArea");
            }
        }

        if (collision.tag == "BattleDetector")
        {
            if (Input.GetKeyDown(KeyCode.E) == true)
            {
                StartBattle("SetBattle");
            }
        }
    }

    private void CheckGrass(string area)
    {
        float encounter = Random.Range(0, 250);

        if (encounter == 5)
        {
            PlayerPrefs.SetString("EncounterArea", area);
            StartBattle("Encounter");
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Healer")
        {
            HealStickmon();
        }
    }

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

    IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2);
        ToggleOverlay("disable");
    }

    #endregion

    #region ManuPanel

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

    private void DefineMenuData()
    {
        List<CurrentStickmon> allAlliedStickmon = GameManagerScript.myGameManagerScript.GetAllAlliedStickmon();

        for (int count = 0; count < allAlliedStickmon.Count; count++)
        {
            Debug.Log(count);
            if (count == 0)
            {
                firstStickmonImage.sprite = allAlliedStickmon[count].GetStickmonSprite();
                firstStickmonNameText.text = allAlliedStickmon[count].GetStickmonName();
                firstStickmonLevelHealthText.text = $"Level: {allAlliedStickmon[count].GetStickmonLevel()}, {allAlliedStickmon[count].GetCurrentHealthPoints()}/{allAlliedStickmon[count].GetMaxHealthPoints()}";
            }
            if (count == 1)
            {
                secondStickmonImage.sprite = allAlliedStickmon[count].GetStickmonSprite();
                secondStickmonNameText.text = allAlliedStickmon[count].GetStickmonName();
                secondStickmonLevelHealthText.text = $"Level: {allAlliedStickmon[count].GetStickmonLevel()}, {allAlliedStickmon[count].GetCurrentHealthPoints()}/{allAlliedStickmon[count].GetMaxHealthPoints()}";
            }
            if (count == 2)
            {
                thirdStickmonImage.sprite = allAlliedStickmon[count].GetStickmonSprite();
                thirdStickmonNameText.text = allAlliedStickmon[count].GetStickmonName();
                thirdStickmonLevelHealthText.text = $"Level: {allAlliedStickmon[count].GetStickmonLevel()}, {allAlliedStickmon[count].GetCurrentHealthPoints()}/{allAlliedStickmon[count].GetMaxHealthPoints()}";
            }
        }
    }

    #endregion
    #endregion
}
