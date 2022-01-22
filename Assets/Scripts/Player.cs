﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class Player
{
    public PlayerColor color;
    public Dictionary<ResourceType, int> resources;
    public Dictionary<DevCardType, int> devCards;
    public Dictionary<BuildingType, int> buildingsLeft;
    public int roadsLeft;
    public List<NodeController> buildings;
    public List<EdgeController> roads;
    public int victoryPoints;
    public bool CanBuyRoad()
    {
        return roadsLeft >= 1
            && resources[ResourceType.Brick] >= 1
            && resources[ResourceType.Wood] >= 1;
    }
    public bool CanBuySettlement()
    {
        return buildingsLeft[BuildingType.Settlement] >= 1
            && resources[ResourceType.Brick] >= 1
            && resources[ResourceType.Wood] >= 1
            && resources[ResourceType.Wheat] >= 1
            && resources[ResourceType.Sheep] >= 1;
    }
    public bool CanBuyCity()
    {
        return buildingsLeft[BuildingType.City] >= 1
            && resources[ResourceType.Wheat] >= 2
            && resources[ResourceType.Ore] >= 3;
    }
    public void BuyRoad()
    {
        roadsLeft -= 1;
        resources[ResourceType.Brick] -= 1;
        resources[ResourceType.Wood] -= 1;
    }
    public void BuySettlement()
    {
        buildingsLeft[BuildingType.Settlement] -= 1;
        resources[ResourceType.Brick] -= 1;
        resources[ResourceType.Wood] -= 1;
        resources[ResourceType.Wheat] -= 1;
        resources[ResourceType.Sheep] -= 1;
    }
    public void BuyCity()
    {
        buildingsLeft[BuildingType.City] -= 1;
        resources[ResourceType.Wheat] -= 2;
        resources[ResourceType.Ore] -= 3;
    }
    public Player(PlayerColor color)
    {
        this.color = color;
        resources = new Dictionary<ResourceType, int>()
        {
            { ResourceType.Brick, 0 },
            { ResourceType.Wheat, 0 },
            { ResourceType.Sheep, 0 },
            { ResourceType.Wood, 0 },
            { ResourceType.Ore, 0 },
        };
        devCards = new Dictionary<DevCardType, int>()
        {
            { DevCardType.Plenty, 0 },
            { DevCardType.Monopoly, 0 },
            { DevCardType.Roadbuilding, 0 },
            { DevCardType.Knight, 0 },
            { DevCardType.VP, 0 },
        };
        buildingsLeft = new Dictionary<BuildingType, int>()
        {
            { BuildingType.Settlement, 5 },
            { BuildingType.City, 4 },
        };
        roadsLeft = 15;
        buildings = new List<NodeController>();
        roads = new List<EdgeController>();
        victoryPoints = 0;
    }
}
