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
    public PlayerColor playerTurn;
    // Start is called before the first frame update
    void Start()
    {
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
        if (playersCount == 4)
        {
            players.Add(new Player(PlayerColor.Red));
        }
        players.Add(new Player(PlayerColor.Green));
        players.Add(new Player(PlayerColor.Blue));
        players.Add(new Player(PlayerColor.Orange));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
