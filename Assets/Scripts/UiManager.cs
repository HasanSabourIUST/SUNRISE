using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Game game;
    public Text playerTurnText;
    public Text promptText;
    public Text[] resourcesTexts;
    public Button actionButton;
    public Button settlementButton;
    public Button cityButton;
    public Button roadButton;
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
            settlementButton.enabled = false;
            cityButton.enabled = false;
            roadButton.enabled = false;
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
            settlementButton.enabled = game.players[game.currentPlayer].CanBuySettlement();
            cityButton.enabled = game.players[game.currentPlayer].CanBuyCity();
            roadButton.enabled = game.players[game.currentPlayer].CanBuyRoad();
            settlementButton.GetComponentInChildren<Text>().text = "Buy Settlement: "
                + game.players[game.currentPlayer].buildingsLeft[Game.BuildingType.Settlement];
            cityButton.GetComponentInChildren<Text>().text = "Buy City: "
                + game.players[game.currentPlayer].buildingsLeft[Game.BuildingType.City];
            roadButton.GetComponentInChildren<Text>().text = "Buy Road: "
                + game.players[game.currentPlayer].roadsLeft;
            if (game.state == Game.State.PlaceSettlement)
            {
                promptText.text = "Place settlement.\n Right click to cancel";
            }
            else if (game.state == Game.State.PlaceCity)
            {
                promptText.text = "Place city.\n Right click to cancel";
            }
            if (game.state == Game.State.PlaceRoad)
            {
                promptText.text = "Place road.\n Right click to cancel";
            }
            else if (game.state == Game.State.Roll)
            {
                promptText.text = "";
                actionButton.GetComponentInChildren<Text>().text = "Roll";
            }
            else if (game.state == Game.State.PlaceThief)
            {
                promptText.text = "Place the thief";
            }
            else if (game.state == Game.State.Wait)
            {
                promptText.text = "";
                actionButton.GetComponentInChildren<Text>().text = "End Turn";
            }
        }
        var player = game.players[game.currentPlayer];
        //buildingsTexts[0].text = "Settlements Left: " + player.buildingsLeft[Game.BuildingType.Settlement].ToString();
        //buildingsTexts[1].text = "Cities Left: " + player.buildingsLeft[Game.BuildingType.City].ToString();
        //roadsTexts.text = "Roads Left: " + player.roadsLeft;
        foreach (var resourceType in player.resources.Keys)
            resourcesTexts[(int)resourceType].text = player.resources[resourceType].ToString();
    }
}
