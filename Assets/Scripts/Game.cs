using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum TileType { ClayHill, Farm, Field, Forest, Mountain, Desert }
    public enum BuildingType { Settlement, City }
    public enum PlayerColor { None, Orange, Green, Blue, Red }
    public enum ResourceType { Brick, Wheat, Sheep, Wood, Ore, None }
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
    bool IsValidPlaceForBuilding(NodeController node)
    {
        foreach (var edge in node.edges)
        {
            foreach (var neighbor in edge.nodes)
            {
                if (neighbor.color != PlayerColor.None)
                {
                    return false;
                }
            }
        }
        return true;
    }
    NodeController PlaceBuilding(Collider2D collider, BuildingType buildingType)
    {
        if (players[currentPlayer].buildingsLeft[buildingType] == 0)
            return null;
        foreach (var node in board.nodes)
        {
            if (collider.gameObject == node.gameObject)
            {
                if (buildingType == BuildingType.Settlement)
                {
                    if ((node.color == PlayerColor.None) && IsValidPlaceForBuilding(node))
                    {
                        --players[currentPlayer].buildingsLeft[buildingType];
                        players[currentPlayer].buildings.Add(node);
                        node.color = currentPlayer;
                        board.AddSettlement(node);
                        return node;
                    }
                }
                else if (buildingType == BuildingType.City)
                {
                    if (node.color == currentPlayer && node.buildingType == BuildingType.Settlement)
                    {
                        --players[currentPlayer].buildingsLeft[buildingType];
                        players[currentPlayer].buildings.Add(node);
                        board.AddCity(node);
                        return node;
                    }
                }
                break;
            }
        }
        return null;
    }
    bool PlaceRoad(Collider2D collider)
    {
        if (players[currentPlayer].roadsLeft == 0)
            return false;
        foreach (var edge in board.edges)
        {
            if (collider.gameObject == edge.gameObject)
            {
                if (edge.color == PlayerColor.None)
                {
                    --players[currentPlayer].roadsLeft;
                    players[currentPlayer].roads.Add(edge);
                    edge.color = currentPlayer;
                    board.AddRoad(edge);
                    return true;
                }
                break;
            }
        }
        return false;
    }
    void GiveResourceToPlayer(Player player, ResourceType resourceType, int count)
    {
        if (resources[resourceType] <= count)
        {
            player.resources[resourceType] += resources[resourceType];
            resources[resourceType] = 0;
        }
        else
        {
            player.resources[resourceType] += count;
            resources[resourceType] -= count;
        }
    }
    void GatherResourcesOnSetup(NodeController node)
    {
        foreach (var tile in board.tiles)
        {
            if ((tile.type != TileType.Desert) && tile.nodes.Contains(node))
            {
                GiveResourceToPlayer(players[currentPlayer], ResourceFrom(tile.type), 1);
            }
        }
    }
    void GatherResources(TileController tile)
    {
        foreach (var node in tile.nodes)
        {
            if (node.color != PlayerColor.None)
            {
                if (node.buildingType == BuildingType.Settlement)
                    GiveResourceToPlayer(players[node.color], ResourceFrom(tile.type), 1);
                else if (node.buildingType == BuildingType.City)
                    GiveResourceToPlayer(players[node.color], ResourceFrom(tile.type), 2);
            }
        }
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
                    var node = PlaceBuilding(hit.collider, BuildingType.Settlement);
                    if (node != null)
                    {
                        if (phase == GamePhase.Start1)
                        {
                            state = State.PlaceRoad;
                        }
                        else if (phase == GamePhase.Start2)
                        {
                            GatherResourcesOnSetup(node);
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
