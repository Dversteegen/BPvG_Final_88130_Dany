using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageScript : MonoBehaviour
{    
    public Image opponentImage;
    public Image playerImage;

    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite thirdSprite;
    public Sprite playerSprite;

    // Start is called before the first frame update
    void Start()
    {        
        ChangeOpponentImage();
        ChangePlayerImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeOpponentImage()
    {        
        string randomEncounter = GameManagerScript.myGameManagerScript.GetRandomStickmon().GetStickmonName();        

        switch (randomEncounter)
        {
            case "Larry":
                opponentImage.sprite = firstSprite;
                break;

            case "Paul":
                opponentImage.sprite = secondSprite;
                break;

            case "Griffin":
                opponentImage.sprite = thirdSprite;
                break;
        } 
    }

    private void ChangePlayerImage()
    {
        string currentStickmon = GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName();

        playerImage.sprite = playerSprite;
    }
}
