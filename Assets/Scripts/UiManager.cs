using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text playerTurnText;
    public Text promptText;
    public Game game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerTurnText.text = "Turn: " + game.currentPlayer;
        if (game.phase == Game.GamePhase.Start1)
        {
            if (game.state == Game.State.PlaceSettlement)
                promptText.text = "Place your 1st settlement";
            else if (game.state == Game.State.PlaceRoad)
                promptText.text = "Place your 1st road";
        }
        else if (game.phase == Game.GamePhase.Start2)
        {
            if (game.state == Game.State.PlaceSettlement)
                promptText.text = "Place your 2nd settlement";
            else if (game.state == Game.State.PlaceRoad)
                promptText.text = "Place your 2nd road";
        }
        else if (game.phase == Game.GamePhase.Middle)
        {
            if (game.state == Game.State.Roll)
                promptText.text = "Roll";
        }
    }
}
