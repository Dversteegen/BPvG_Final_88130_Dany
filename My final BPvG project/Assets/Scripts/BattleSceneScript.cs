using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneScript : MonoBehaviour
{
    #region UIElements

    public Image opponentImage;
    public Image playerImage;

    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI opponentName;

    #endregion

    #region Sprites

    public Sprite larrySprite;
    public Sprite paulSprite;
    public Sprite griffinSprite;
    public Sprite julianSprite;

    #endregion

    //private void Start()
    void Start()
    {
        //This needs to be above the two GetComponent lines for the text, god knows why
        UpdateBattleScene();

        playerName = GetComponent<TextMeshProUGUI>();
        opponentName = GetComponent<TextMeshProUGUI>();        
    }

    #region UpdatebattleScene

    private void UpdateBattleScene()
    {
        Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
        UpdateOpponent(encounter);

        Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();
        UpdatePlayer(firstStickmon);
    }

    private void UpdatePlayer(Stickmon firstStickmon)
    {
        switch (firstStickmon.GetStickmonName())
        {
            case "Julian":
                playerImage.sprite = julianSprite;
                break;
        }
        playerName.text = firstStickmon.GetStickmonName();
    }

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
    }

    #endregion

    //private void ChangeOpponentImage()
    //{
    //    Stickmon encounter = GameManagerScript.myGameManagerScript.GetRandomStickmon();
    //    string encounterName = encounter.GetStickmonName();

    //    Stickmon firstStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon();

    //    switch (encounterName)
    //    {
    //        case "Larry":
    //            opponentImage.sprite = firstSprite;
    //            break;

    //        case "Paul":
    //            opponentImage.sprite = secondSprite;
    //            break;

    //        case "Griffin":
    //            opponentImage.sprite = thirdSprite;
    //            break;
    //    }

    //    playerName.text = firstStickmon.GetStickmonName();
    //    opponentName.text = encounterName;
    //}

    //private void ChangePlayerImage()
    //{
    //    string currentStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName();

    //    playerImage.sprite = playerSprite;
    //}

    //private void ChangeNames()
    //{
    //    playerName.text = GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName();
    //    opponentName.text = GameManagerScript.myGameManagerScript
    //}
}
