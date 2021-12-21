using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum TileType { ClayHill, Desert, Farm, Field, Forest, Mountain }
    public enum BuildingType { Settlement, City }
    public enum PlayerColor { None, Red, Green, Blue, Orange }
    public enum ResourceType { Brick, None, Wheat, Sheep, Wood, Ore }
    public enum DevCardType { Plenty, Monopoly, Roadbuilding, Knight, VP }
    public enum State { PlaceBuilding, PlaceRoad, Roll, PlaceThief, Wait }
    public enum GamePhase { Start1, Start2, Middle }
    public static ResourceType ResourceFrom(TileType tileType)
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
    public int playersCount;
    public BoardController board;
    public List<Player> players;
    public Dictionary<ResourceType, int> resources;
    public Dictionary<DevCardType, int> devCards;
    public TileController tileWithThief;
    public PlayerColor turn;
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
        players = new List<Player>();
        if (playersCount == 4)
        {
            players.Add(new Player(PlayerColor.Red));
        }
        players.Add(new Player(PlayerColor.Green));
        players.Add(new Player(PlayerColor.Blue));
        players.Add(new Player(PlayerColor.Orange));
        turn = players[0].color;
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
                            node.color = turn;
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
                                edge.color = turn;
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
