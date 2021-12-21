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
    public BoardController board;
    public List<Player> players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
