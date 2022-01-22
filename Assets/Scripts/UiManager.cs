using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Game game;
    public Text playerTurnText;
    public Text promptText;
    public Text[] buildingsTexts;
    public Text roadsTexts;
    public Text[] resourcesTexts;
    public Button actionButton;
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
            {
                promptText.text = "";
                actionButton.GetComponentInChildren<Text>().text = "Roll";
            }
            else if (game.state == Game.State.PlaceThief)
            {
                promptText.text = "Place the thief on one of the tiles";
            }
            else if (game.state == Game.State.Wait)
            {
                actionButton.GetComponentInChildren<Text>().text = "End Turn";
            }
        }
        var player = game.players[game.currentPlayer];
        buildingsTexts[0].text = "Settlements Left: " + player.buildingsLeft[Game.BuildingType.Settlement].ToString();
        buildingsTexts[1].text = "Cities Left: " + player.buildingsLeft[Game.BuildingType.City].ToString();
        roadsTexts.text = "Roads Left: " + player.roadsLeft;
        foreach (var resourceType in player.resources.Keys)
            resourcesTexts[(int)resourceType].text = resourceType.ToString() + ": " + player.resources[resourceType].ToString();
    }
}
