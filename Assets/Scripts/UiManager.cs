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
    public Text[] devCardTexts;
    public Text victoryPointsText;
    public Button actionButton;
    public Button settlementButton;
    public Button cityButton;
    public Button roadButton;
    public Button devCardButton;
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
            actionButton.interactable = false;
            settlementButton.interactable = false;
            cityButton.interactable = false;
            roadButton.interactable = false;
            devCardButton.interactable = false;
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
            actionButton.interactable = true;
            victoryPointsText.text = "VP: " + game.players[game.currentPlayer].victoryPoints;
            if (game.state == Game.State.Roll)
            {
                settlementButton.interactable = false;
                cityButton.interactable = false;
                roadButton.interactable = false;
                devCardButton.interactable = false;
            }
            else
            {
                settlementButton.interactable = game.players[game.currentPlayer].CanBuySettlement();
                cityButton.interactable = game.players[game.currentPlayer].CanBuyCity();
                roadButton.interactable = game.players[game.currentPlayer].CanBuyRoad();
                devCardButton.interactable = game.players[game.currentPlayer].CanBuyDevCard() && (game.GetRemainingDevCards().Count >= 1);
            }
            settlementButton.GetComponentInChildren<Text>().text = "Buy Settlement: "
                + game.players[game.currentPlayer].buildingsLeft[Game.BuildingType.Settlement];
            cityButton.GetComponentInChildren<Text>().text = "Buy City: "
                + game.players[game.currentPlayer].buildingsLeft[Game.BuildingType.City];
            roadButton.GetComponentInChildren<Text>().text = "Buy Road: "
                + game.players[game.currentPlayer].roadsLeft;
            if (game.state == Game.State.PlaceSettlement)
            {
                promptText.text = "Place the settlement.\nRight click to cancel";
            }
            else if (game.state == Game.State.PlaceCity)
            {
                promptText.text = "Place the city.\nRight click to cancel";
            }
            if (game.state == Game.State.PlaceRoad)
            {
                promptText.text = "Place the road.\nRight click to cancel";
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
            else if (game.state == Game.State.UseKnight)
            {
                promptText.text = "Place the thief.\nRight click to cancel";
            }
            else if (game.state == Game.State.UseMonopoly)
            {
                promptText.text = "Choose a resource.\nRight click to cancel";
            }
            else if (game.state == Game.State.UsePlenty)
            {
                promptText.text = "Choose a resource.\nRight click to cancel";
            }
            else if (game.state == Game.State.UseRoadBuilding1)
            {
                promptText.text = "Place the 1st road.\nRight click to cancel";
            }
            else if (game.state == Game.State.UseRoadBuilding2)
            {
                promptText.text = "Place the 2nd road.";
            }
        }
        else if (game.phase == Game.GamePhase.Finished)
        {
            promptText.text = game.currentPlayer.ToString() + " Won!";
            actionButton.GetComponentInChildren<Text>().text = "Quit";
        }
        var player = game.players[game.currentPlayer];
        foreach (var resourceType in player.resources.Keys)
            resourcesTexts[(int)resourceType].text = player.resources[resourceType].ToString();
        foreach (var devCard in player.devCards.Keys)
            devCardTexts[(int)devCard].text = player.GetDevCardCount(devCard).ToString();
    }
}
