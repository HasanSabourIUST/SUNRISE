using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum TileType { ClayHill, Desert, Farm, Field, Forest, Mountain }
    public enum BuildingType { Settlement, City }
    public enum PlayerColor { None, Orange, Green, Blue, Red }
    public enum ResourceType { Brick, None, Wheat, Sheep, Wood, Ore }
    public enum DevCardType { Plenty, Monopoly, Roadbuilding, Knight, VP }
    public enum State { PlaceBuilding, PlaceRoad, Roll, PlaceThief, Wait }
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
    public PlayerColor nextPlayer()
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
    public PlayerColor previousPlayer()
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                bool nodeHit = false;
                bool edgeHit = false;
                foreach (var node in board.nodes)
                {
                    if (hit.collider.gameObject == node.gameObject)
                    {
                        if (node.color == PlayerColor.None)
                        {
                            node.color = currentPlayer;
                            board.AddSettlement(node);
                        }
                        else if (node.buildingType == BuildingType.Settlement)
                        {
                            board.AddCity(node);
                        }
                        nodeHit = true;
                        break;
                    }
                }
                if (!nodeHit)
                {
                    foreach (var edge in board.edges)
                    {
                        if (hit.collider.gameObject == edge.gameObject)
                        {
                            if (edge.color == PlayerColor.None)
                            {
                                edge.color = currentPlayer;
                                board.AddRoad(edge);
                            }
                            edgeHit = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
