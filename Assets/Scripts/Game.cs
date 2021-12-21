﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum TileType { ClayHill, Desert, Farm, Field, Forest, Mountain }
    public enum BuildingType { Settlement, City }
    public enum PlayerColor { None, Orange, Green, Blue, Red }
    public enum ResourceType { Brick, None, Wheat, Sheep, Wood, Ore }
    public enum DevCardType { Plenty, Monopoly, Roadbuilding, Knight, VP }
    public enum State { PlaceSettlement, PlaceCity, PlaceRoad, Roll, PlaceThief, Wait }
    public enum GamePhase { Start1, Start2, Middle }
    public ResourceType ResourceFrom(TileType tileType)
    {
        switch (tileType)
        {
            case TileType.ClayHill:
                return ResourceType.Brick;
            case TileType.Farm:
                return ResourceType.Wheat;
            case TileType.Field:
                return ResourceType.Sheep;
            case TileType.Forest:
                return ResourceType.Wood;
            case TileType.Mountain:
                return ResourceType.Ore;
        }
        return ResourceType.None;
    }
    public PlayerColor GetNextPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerColor.Orange:
                return PlayerColor.Green;
            case PlayerColor.Green:
                return PlayerColor.Blue;
            case PlayerColor.Blue:
                return (playersCount == 3) ? PlayerColor.None : PlayerColor.Red;
            case PlayerColor.Red:
                return PlayerColor.None;
        }
        return PlayerColor.Orange;
    }
    public PlayerColor GetPreviousPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerColor.Orange:
                return PlayerColor.None;
            case PlayerColor.Green:
                return PlayerColor.Orange;
            case PlayerColor.Blue:
                return PlayerColor.Green;
            case PlayerColor.Red:
                return PlayerColor.Blue;
        }
        return (playersCount == 3) ? PlayerColor.Blue : PlayerColor.Red;
    }
    public int playersCount;
    public BoardController board;
    public Dictionary<PlayerColor, Player> players;
    public Dictionary<ResourceType, int> resources;
    public Dictionary<DevCardType, int> devCards;
    public TileController tileWithThief;
    public PlayerColor currentPlayer;
    public State state;
    public GamePhase phase;
    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    void NewGame()
    {
        phase = GamePhase.Start1;
        state = State.PlaceSettlement;
        players = new Dictionary<PlayerColor, Player>();
        for (int i = 1; i <= playersCount; ++i)
        {
            PlayerColor color = (PlayerColor)i;
            players.Add(color, new Player(color));
        }
        currentPlayer = PlayerColor.Orange;
        resources = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Brick, 19 },
            { ResourceType.Wheat, 19 },
            { ResourceType.Sheep, 19 },
            { ResourceType.Wood, 19 },
            { ResourceType.Ore, 19 },
        };
        devCards = new Dictionary<DevCardType, int>()
        {
            { DevCardType.Plenty, 2 },
            { DevCardType.Monopoly, 2 },
            { DevCardType.Roadbuilding, 2 },
            { DevCardType.Knight, 14 },
            { DevCardType.VP, 5 },
        };
        foreach (var tile in board.tiles)
        {
            if (tile.type == TileType.Desert)
            {
                tileWithThief = tile;
                break;
            }
        }
    }
    bool PlaceBuilding(Collider2D collider, BuildingType buildingType)
    {
        foreach (var node in board.nodes)
        {
            if (collider.gameObject == node.gameObject)
            {
                if (buildingType == BuildingType.Settlement)
                {
                    if (node.color == PlayerColor.None)
                    {
                        node.color = currentPlayer;
                        board.AddSettlement(node);
                        return true;
                    }
                }
                else
                {
                    if (node.color == currentPlayer && node.buildingType == BuildingType.Settlement)
                    {
                        board.AddCity(node);
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }
    bool PlaceRoad(Collider2D collider)
    {
        foreach (var edge in board.edges)
        {
            if (collider.gameObject == edge.gameObject)
            {
                if (edge.color == PlayerColor.None)
                {
                    edge.color = currentPlayer;
                    board.AddRoad(edge);
                    return true;
                }
                break;
            }
        }
        return false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (state == State.PlaceSettlement)
                {
                    if (PlaceBuilding(hit.collider, BuildingType.Settlement))
                    {
                        if (phase == GamePhase.Start1)
                        {
                            state = State.PlaceRoad;
                        }
                        else if (phase == GamePhase.Start2)
                        {
                            state = State.PlaceRoad;
                        }
                    }
                }
                else if (state == State.PlaceRoad)
                {
                    if (PlaceRoad(hit.collider))
                    {
                        if (phase == GamePhase.Start1)
                        {
                            currentPlayer = GetNextPlayer();
                            state = State.PlaceSettlement;
                            if (currentPlayer == PlayerColor.None)
                            {
                                phase = GamePhase.Start2;
                                currentPlayer = GetPreviousPlayer();
                            }
                        }
                        else if (phase == GamePhase.Start2)
                        {
                            // TODO: Add resources to player
                            currentPlayer = GetPreviousPlayer();
                            if (currentPlayer == PlayerColor.None)
                            {
                                phase = GamePhase.Middle;
                                state = State.Roll;
                                currentPlayer = GetNextPlayer();
                            }
                            else
                            {
                                state = State.PlaceSettlement;
                            }
                        }
                    }
                }
            }
        }
    }
}
