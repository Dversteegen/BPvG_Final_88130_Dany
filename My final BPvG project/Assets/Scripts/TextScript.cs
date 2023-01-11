using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TextScript : MonoBehaviour
{    
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] TextMeshProUGUI opponentName;

    private void Start()
    {
        ChangeNames();

        playerName = GetComponent<TextMeshProUGUI>();
        opponentName = GetComponent<TextMeshProUGUI>();
    }

    private void ChangeNames()
    {
        playerName.text = GameManagerScript.myGameManagerScript.GetFirstStickmon().GetStickmonName();
        opponentName.text = "Hoi";
    }
}
