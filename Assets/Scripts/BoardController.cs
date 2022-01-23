using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Game;

public class BoardController : MonoBehaviour
{
    public const int TILES_COUNT = 19;
    public const int NODES_COUNT = 54;
    public const int EDGES_COUNT = 72;
    public float size;
    public TileController tilePrefab;
    public NodeController nodePrefab;
    public EdgeController edgePrefab;
    // Sprite prefabs should be in the following order:
    // ClayHill, Farm, Field, Forest, Mountain, Desert
    public GameObject[] tileSpritePrefabs;
    // Numbers 2 through 12 (7 should be null)
    public GameObject[] tileNumberPrefabs;
    // The prefabs should be in the following order:
    // None (null), Orange, Green, Blue, Red
    public GameObject[] roadPrefabs;
    public GameObject[] settlementPrefabs;
    public GameObject[] cityPrefabs;
    public TileController[] tiles;
    public NodeController[] nodes;
    public EdgeController[] edges;
    public float tileLength;
    public Dictionary<int, List<TileController>> tilesByNumber;
    // Start is called before the first frame update
    void Start()
    {
        LoadBoard();
    }

    private void LoadBoard()
    {
        tilesByNumber = new Dictionary<int, List<TileController>>();
        for (int i = 2; i <= 12; ++i)
            tilesByNumber.Add(i, new List<TileController>());

        Vector3 diagonalTileStep = new Vector3(tileLength * Mathf.Sqrt(3) / 2, -tileLength * 1.5f, 0);
        Vector3 straightTileStep = new Vector3(tileLength * Mathf.Sqrt(3), 0, 0);
        Vector3 diagonalNodeStep = new Vector3(tileLength * Mathf.Sqrt(3) / 2, -tileLength / 2, 0);
        Vector3 straightNodeStep = new Vector3(0, -tileLength, 0);
        Vector3 diagonalEdgeStep = diagonalTileStep / 2;
        Vector3 straightEdgeStep = straightTileStep / 2;
        //diagonalTileStep *= size;
        //diagonalNodeStep *= size;
        //diagonalEdgeStep *= size;
        //straightTileStep *= size;
        //straightNodeStep *= size;
        //straightEdgeStep *= size;

        int[][] neighborNodesOfTile = new int[TILES_COUNT][]
        {
            new int[6] {0, 1, 2, 8, 9, 10 },
            new int[6] {2, 3, 4, 10, 11, 12 },
            new int[6] {4, 5, 6, 12, 13, 14 },

            new int[6] {7, 8, 9, 17, 18, 19 },
            new int[6] {9, 10, 11, 19, 20, 21 },
            new int[6] {11, 11, 13, 21, 22, 23 },
            new int[6] {13, 14, 15, 23, 24, 25 },

            new int[6] {16, 17, 18, 27, 28, 29 },
            new int[6] {18, 19, 20, 29, 30, 31 },
            new int[6] {20, 21, 22, 31, 32, 33 },
            new int[6] {22, 23, 24, 33, 34, 35 },
            new int[6] {24, 25, 26, 35, 36, 37 },

            new int[6] {28, 29, 30, 38, 39, 40 },
            new int[6] {30, 31, 32, 40, 41, 42 },
            new int[6] {32, 33, 34, 42, 43, 44 },
            new int[6] {34, 35, 36, 44, 45, 46 },

            new int[6] {39, 40, 41, 47, 48, 49 },
            new int[6] {41, 42, 43, 49, 50, 51 },
            new int[6] {43, 44, 45, 51, 52, 53 },
            };

        int[][] neighborEdgesOfNode = new int[NODES_COUNT][]
        {
            new int[2] {0, 6},
            new int[2] {0, 1},
            new int[3] {1, 2, 7},
            new int[2] {2, 3},
            new int[3] {3, 4, 8},
            new int[2] {4, 5},
            new int[2] {5, 9},

            new int[2] {10, 18},
            new int[3] {10, 11, 6},
            new int[3] {11, 12, 19},
            new int[3] {12, 13, 7},
            new int[3] {13, 14, 20},
            new int[3] {14, 15, 8},
            new int[3] {15, 16, 21},
            new int[3] {16, 17, 9},
            new int[2] {17, 22},

            new int[2] {23, 33},
            new int[3] {23, 24, 18},
            new int[3] {24, 25, 34},
            new int[3] {25, 26, 19},
            new int[3] {26, 27, 35},
            new int[3] {27, 28, 20},
            new int[3] {28, 29, 36},
            new int[3] {29, 30, 21},
            new int[3] {30, 31, 37},
            new int[3] {31, 32, 22},
            new int[2] {32, 38},

            new int[2] {39, 33},
            new int[3] {39, 40, 49},
            new int[3] {40, 41, 34},
            new int[3] {41, 42, 50},
            new int[3] {42, 43, 35},
            new int[3] {43, 44, 51},
            new int[3] {44, 45, 36},
            new int[3] {45, 46, 52},
            new int[3] {46, 47, 37},
            new int[3] {47, 48, 53},
            new int[2] {48, 38},

            new int[2] {54, 49},
            new int[3] {54, 55, 62},
            new int[3] {55, 56, 50},
            new int[3] {56, 57, 63},
            new int[3] {57, 58, 51},
            new int[3] {58, 59, 64},
            new int[3] {59, 60, 52},
            new int[3] {60, 61, 65},
            new int[2] {61, 53},

            new int[2] {66, 62},
            new int[2] {66, 67},
            new int[3] {67, 68, 63},
            new int[2] {68, 69},
            new int[3] {69, 70, 64},
            new int[2] {70, 71},
            new int[2] {71, 65},
        };

        int[][] neighborNodesOfEdge = new int[EDGES_COUNT][]
        {
            new int[2] {0, 1},
            new int[2] {1, 2},
            new int[2] {2, 3},
            new int[2] {3, 4},
            new int[2] {4, 5},
            new int[2] {5, 6},

            new int[2] {0, 8},
            new int[2] {2, 10},
            new int[2] {4, 12},
            new int[2] {6, 14},

            new int[2] {7, 8},
            new int[2] {8, 9},
            new int[2] {9, 10},
            new int[2] {10, 11},
            new int[2] {11, 12},
            new int[2] {12, 13},
            new int[2] {13, 14},
            new int[2] {14, 15},

            new int[2] {7, 17},
            new int[2] {9, 19},
            new int[2] {11, 21},
            new int[2] {13, 23},
            new int[2] {15, 25},

            new int[2] {16, 17},
            new int[2] {17, 18},
            new int[2] {18, 19},
            new int[2] {19, 20},
            new int[2] {20, 21},
            new int[2] {21, 22},
            new int[2] {22, 23},
            new int[2] {23, 24},
            new int[2] {24, 25},
            new int[2] {25, 26},

            new int[2] {16, 27},
            new int[2] {18, 29},
            new int[2] {20, 31},
            new int[2] {22, 33},
            new int[2] {24, 35},
            new int[2] {26, 37},

            new int[2] {27, 28},
            new int[2] {28, 29},
            new int[2] {29, 30},
            new int[2] {30, 31},
            new int[2] {31, 32},
            new int[2] {32, 33},
            new int[2] {33, 34},
            new int[2] {34, 35},
            new int[2] {35, 36},
            new int[2] {36, 37},

            new int[2] {28, 38},
            new int[2] {30, 40},
            new int[2] {32, 42},
            new int[2] {34, 44},
            new int[2] {36, 46},

            new int[2] {38, 39},
            new int[2] {39, 40},
            new int[2] {40, 41},
            new int[2] {41, 42},
            new int[2] {42, 43},
            new int[2] {43, 44},
            new int[2] {44, 45},
            new int[2] {45, 46},

            new int[2] {39, 47},
            new int[2] {41, 49},
            new int[2] {43, 51},
            new int[2] {45, 53},

            new int[2] {47, 48},
            new int[2] {48, 49},
            new int[2] {49, 50},
            new int[2] {50, 51},
            new int[2] {51, 52},
            new int[2] {52, 53},
        };

        Vector3[] tilePositions = new Vector3[TILES_COUNT]
        {
            -2 * diagonalTileStep +0 * straightTileStep,
            -2 * diagonalTileStep +1 * straightTileStep,
            -2 * diagonalTileStep +2 * straightTileStep,

            -1 * diagonalTileStep -1 * straightTileStep,
            -1 * diagonalTileStep +0 * straightTileStep,
            -1 * diagonalTileStep +1 * straightTileStep,
            -1 * diagonalTileStep +2 * straightTileStep,

            +0 * diagonalTileStep -2 * straightTileStep,
            +0 * diagonalTileStep -1 * straightTileStep,
            +0 * diagonalTileStep +0 * straightTileStep,
            +0 * diagonalTileStep +1 * straightTileStep,
            +0 * diagonalTileStep +2 * straightTileStep,

            +1 * diagonalTileStep -2 * straightTileStep,
            +1 * diagonalTileStep -1 * straightTileStep,
            +1 * diagonalTileStep +0 * straightTileStep,
            +1 * diagonalTileStep +1 * straightTileStep,

            +2 * diagonalTileStep -2 * straightTileStep,
            +2 * diagonalTileStep -1 * straightTileStep,
            +2 * diagonalTileStep +0 * straightTileStep,
        };

        Vector3[] nodePositions = new Vector3[NODES_COUNT]
        {
            -3 * diagonalNodeStep -2 * straightNodeStep,
            -2 * diagonalNodeStep -3 * straightNodeStep,
            -1 * diagonalNodeStep -3 * straightNodeStep,
            +0 * diagonalNodeStep -4 * straightNodeStep,
            +1 * diagonalNodeStep -4 * straightNodeStep,
            +2 * diagonalNodeStep -5 * straightNodeStep,
            +3 * diagonalNodeStep -5 * straightNodeStep,

            -4 * diagonalNodeStep +0 * straightNodeStep,
            -3 * diagonalNodeStep -1 * straightNodeStep,
            -2 * diagonalNodeStep -1 * straightNodeStep,
            -1 * diagonalNodeStep -2 * straightNodeStep,
            +0 * diagonalNodeStep -2 * straightNodeStep,
            +1 * diagonalNodeStep -3 * straightNodeStep,
            +2 * diagonalNodeStep -3 * straightNodeStep,
            +3 * diagonalNodeStep -4 * straightNodeStep,
            +4 * diagonalNodeStep -4 * straightNodeStep,

            -5 * diagonalNodeStep +2 * straightNodeStep,
            -4 * diagonalNodeStep +1 * straightNodeStep,
            -3 * diagonalNodeStep +1 * straightNodeStep,
            -2 * diagonalNodeStep +0 * straightNodeStep,
            -1 * diagonalNodeStep +0 * straightNodeStep,
            +0 * diagonalNodeStep -1 * straightNodeStep,
            +1 * diagonalNodeStep -1 * straightNodeStep,
            +2 * diagonalNodeStep -2 * straightNodeStep,
            +3 * diagonalNodeStep -2 * straightNodeStep,
            +4 * diagonalNodeStep -3 * straightNodeStep,
            +5 * diagonalNodeStep -3 * straightNodeStep,

            -5 * diagonalNodeStep +3 * straightNodeStep,
            -4 * diagonalNodeStep +3 * straightNodeStep,
            -3 * diagonalNodeStep +2 * straightNodeStep,
            -2 * diagonalNodeStep +2 * straightNodeStep,
            -1 * diagonalNodeStep +1 * straightNodeStep,
            +0 * diagonalNodeStep +1 * straightNodeStep,
            +1 * diagonalNodeStep +0 * straightNodeStep,
            +2 * diagonalNodeStep +0 * straightNodeStep,
            +3 * diagonalNodeStep -1 * straightNodeStep,
            +4 * diagonalNodeStep -1 * straightNodeStep,
            +5 * diagonalNodeStep -2 * straightNodeStep,

            -4 * diagonalNodeStep +4 * straightNodeStep,
            -3 * diagonalNodeStep +4 * straightNodeStep,
            -2 * diagonalNodeStep +3 * straightNodeStep,
            -1 * diagonalNodeStep +3 * straightNodeStep,
            +0 * diagonalNodeStep +2 * straightNodeStep,
            +1 * diagonalNodeStep +2 * straightNodeStep,
            +2 * diagonalNodeStep +1 * straightNodeStep,
            +3 * diagonalNodeStep +1 * straightNodeStep,
            +4 * diagonalNodeStep +0 * straightNodeStep,

            -3 * diagonalNodeStep +5 * straightNodeStep,
            -2 * diagonalNodeStep +5 * straightNodeStep,
            -1 * diagonalNodeStep +4 * straightNodeStep,
            +0 * diagonalNodeStep +4 * straightNodeStep,
            +1 * diagonalNodeStep +3 * straightNodeStep,
            +2 * diagonalNodeStep +3 * straightNodeStep,
            +3 * diagonalNodeStep +2 * straightNodeStep,
        };

        Vector3[] edgePositions = new Vector3[EDGES_COUNT]
        {
            -5 * diagonalEdgeStep +0 * straightEdgeStep,
            -5 * diagonalEdgeStep +1 * straightEdgeStep,
            -5 * diagonalEdgeStep +2 * straightEdgeStep,
            -5 * diagonalEdgeStep +3 * straightEdgeStep,
            -5 * diagonalEdgeStep +4 * straightEdgeStep,
            -5 * diagonalEdgeStep +5 * straightEdgeStep,

            -4 * diagonalEdgeStep -1 * straightEdgeStep,
            -4 * diagonalEdgeStep +1 * straightEdgeStep,
            -4 * diagonalEdgeStep +3 * straightEdgeStep,
            -4 * diagonalEdgeStep +5 * straightEdgeStep,

            -3 * diagonalEdgeStep -2 * straightEdgeStep,
            -3 * diagonalEdgeStep -1 * straightEdgeStep,
            -3 * diagonalEdgeStep -0 * straightEdgeStep,
            -3 * diagonalEdgeStep +1 * straightEdgeStep,
            -3 * diagonalEdgeStep +2 * straightEdgeStep,
            -3 * diagonalEdgeStep +3 * straightEdgeStep,
            -3 * diagonalEdgeStep +4 * straightEdgeStep,
            -3 * diagonalEdgeStep +5 * straightEdgeStep,

            -2 * diagonalEdgeStep -3 * straightEdgeStep,
            -2 * diagonalEdgeStep -1 * straightEdgeStep,
            -2 * diagonalEdgeStep +1 * straightEdgeStep,
            -2 * diagonalEdgeStep +3 * straightEdgeStep,
            -2 * diagonalEdgeStep +5 * straightEdgeStep,

            -1 * diagonalEdgeStep -4 * straightEdgeStep,
            -1 * diagonalEdgeStep -3 * straightEdgeStep,
            -1 * diagonalEdgeStep -2 * straightEdgeStep,
            -1 * diagonalEdgeStep -1 * straightEdgeStep,
            -1 * diagonalEdgeStep +0 * straightEdgeStep,
            -1 * diagonalEdgeStep +1 * straightEdgeStep,
            -1 * diagonalEdgeStep +2 * straightEdgeStep,
            -1 * diagonalEdgeStep +3 * straightEdgeStep,
            -1 * diagonalEdgeStep +4 * straightEdgeStep,
            -1 * diagonalEdgeStep +5 * straightEdgeStep,

            +0 * diagonalEdgeStep -5 * straightEdgeStep,
            +0 * diagonalEdgeStep -3 * straightEdgeStep,
            +0 * diagonalEdgeStep -1 * straightEdgeStep,
            +0 * diagonalEdgeStep +1 * straightEdgeStep,
            +0 * diagonalEdgeStep +3 * straightEdgeStep,
            +0 * diagonalEdgeStep +5 * straightEdgeStep,

            +1 * diagonalEdgeStep -5 * straightEdgeStep,
            +1 * diagonalEdgeStep -4 * straightEdgeStep,
            +1 * diagonalEdgeStep -3 * straightEdgeStep,
            +1 * diagonalEdgeStep -2 * straightEdgeStep,
            +1 * diagonalEdgeStep -1 * straightEdgeStep,
            +1 * diagonalEdgeStep +0 * straightEdgeStep,
            +1 * diagonalEdgeStep +1 * straightEdgeStep,
            +1 * diagonalEdgeStep +2 * straightEdgeStep,
            +1 * diagonalEdgeStep +3 * straightEdgeStep,
            +1 * diagonalEdgeStep +4 * straightEdgeStep,

            +2 * diagonalEdgeStep -5 * straightEdgeStep,
            +2 * diagonalEdgeStep -3 * straightEdgeStep,
            +2 * diagonalEdgeStep -1 * straightEdgeStep,
            +2 * diagonalEdgeStep +1 * straightEdgeStep,
            +2 * diagonalEdgeStep +3 * straightEdgeStep,

            +3 * diagonalEdgeStep -5 * straightEdgeStep,
            +3 * diagonalEdgeStep -4 * straightEdgeStep,
            +3 * diagonalEdgeStep -3 * straightEdgeStep,
            +3 * diagonalEdgeStep -2 * straightEdgeStep,
            +3 * diagonalEdgeStep -1 * straightEdgeStep,
            +3 * diagonalEdgeStep +0 * straightEdgeStep,
            +3 * diagonalEdgeStep +1 * straightEdgeStep,
            +3 * diagonalEdgeStep +2 * straightEdgeStep,

            +4 * diagonalEdgeStep -5 * straightEdgeStep,
            +4 * diagonalEdgeStep -3 * straightEdgeStep,
            +4 * diagonalEdgeStep -1 * straightEdgeStep,
            +4 * diagonalEdgeStep +1 * straightEdgeStep,

            +5 * diagonalEdgeStep -5 * straightEdgeStep,
            +5 * diagonalEdgeStep -4 * straightEdgeStep,
            +5 * diagonalEdgeStep -3 * straightEdgeStep,
            +5 * diagonalEdgeStep -2 * straightEdgeStep,
            +5 * diagonalEdgeStep -1 * straightEdgeStep,
            +5 * diagonalEdgeStep +0 * straightEdgeStep,
        };

        float[] edgeRotations = new float[EDGES_COUNT]
        {
            -60, 60, -60, 60, -60, 60,
            0, 0, 0, 0,
            -60, 60, -60, 60, -60, 60, -60, 60,
            0, 0, 0, 0, 0,
            -60, 60, -60, 60, -60, 60, -60, 60, -60, 60,
            0, 0, 0, 0, 0, 0,
            60, -60, 60, -60, 60, -60, 60, -60, 60, -60,
            0, 0, 0, 0, 0,
            60, -60, 60, -60, 60, -60, 60, -60,
            0, 0, 0, 0,
            60, -60, 60, -60, 60, -60,
        };

        TileType[] defaultTileTypes = new TileType[TILES_COUNT]
        {
            TileType.Mountain,
            TileType.Field,
            TileType.Forest,
            TileType.Farm,
            TileType.ClayHill,
            TileType.Field,
            TileType.ClayHill,
            TileType.Farm,
            TileType.Forest,
            TileType.Desert,
            TileType.Forest,
            TileType.Mountain,
            TileType.Forest,
            TileType.Mountain,
            TileType.Farm,
            TileType.Field,
            TileType.ClayHill,
            TileType.Farm,
            TileType.Field,
        };

        int[] defaultTileNumbers = new int[TILES_COUNT]
        {
            10, 2, 9,
            12, 6, 4, 10,
            9, 11, 7, 3, 8,
            8, 3, 4, 5,
            5, 6, 11,
        };

        tiles = new TileController[TILES_COUNT];
        nodes = new NodeController[NODES_COUNT];
        edges = new EdgeController[EDGES_COUNT];

        for (int i = 0; i < TILES_COUNT; ++i)
        {
            tiles[i] = Instantiate(tilePrefab);
            tiles[i].type = defaultTileTypes[i];
            tilesByNumber[defaultTileNumbers[i]].Add(tiles[i]);
            tiles[i].transform.parent = transform;
            int tileTypeIdx = (int)tiles[i].type;
            var tileSprite = Instantiate(tileSpritePrefabs[tileTypeIdx]);
            tileSprite.transform.parent = tiles[i].transform;
            int tileNumberIdx = defaultTileNumbers[i] - 2;
            if (tileNumberPrefabs[tileNumberIdx] != null)
            {
                var numberSprite = Instantiate(tileNumberPrefabs[tileNumberIdx]);
                numberSprite.transform.parent = tiles[i].transform;
                numberSprite.transform.position = new Vector3(0, -0.4f, -1) * size;
                numberSprite.transform.localScale = new Vector3(0.3f, 0.3f, 1);
            }
            tiles[i].transform.position = transform.position + tilePositions[i] * size + new Vector3(0, 0, 2);
            tiles[i].transform.localScale *= size;
        }
        for (int i = 0; i < NODES_COUNT; ++i)
        {
            nodes[i] = Instantiate(nodePrefab);
            nodes[i].transform.parent = transform;
            if (nodes[i].color != PlayerColor.None)
            {
                int nodeColorIdx = (int)nodes[i].color;
                GameObject buildingSprite = null;
                if (nodes[i].buildingType == BuildingType.Settlement)
                    buildingSprite = Instantiate(settlementPrefabs[nodeColorIdx]);
                else
                    buildingSprite = Instantiate(cityPrefabs[nodeColorIdx]);
                buildingSprite.transform.parent = nodes[i].transform;
            }
            nodes[i].transform.position = transform.position + nodePositions[i] * size;
            nodes[i].transform.localScale *= size;
        }
        for (int i = 0; i < EDGES_COUNT; ++i)
        {
            edges[i] = Instantiate(edgePrefab);
            edges[i].transform.parent = transform;
            int edgeColorIdx = (int)edges[i].color;
            if (roadPrefabs[edgeColorIdx] != null)
            {
                var roadSprite = Instantiate(roadPrefabs[edgeColorIdx]);
                roadSprite.transform.parent = edges[i].transform;
            }
            edges[i].transform.position = transform.position + edgePositions[i] * size + new Vector3(0, 0, 1);
            edges[i].transform.Rotate(0, 0, edgeRotations[i]);
            edges[i].transform.localScale *= size;
        }

        for (int i = 0; i < TILES_COUNT; ++i)
            tiles[i].nodes = neighborNodesOfTile[i].Select(n => nodes[n]).ToArray();
        for (int i = 0; i < NODES_COUNT; ++i)
            nodes[i].edges = neighborEdgesOfNode[i].Select(e => edges[e]).ToList();
        for (int i = 0; i < EDGES_COUNT; ++i)
            edges[i].nodes = neighborNodesOfEdge[i].Select(n => nodes[n]).ToArray();
    }
    public void AddSettlement(NodeController node)
    {
        node.buildingType = BuildingType.Settlement;
        GameObject buildingSprite = Instantiate(settlementPrefabs[ColorIndex(node.color)]);
        buildingSprite.transform.position = node.transform.position;
        buildingSprite.transform.parent = node.transform;
    }
    public void AddCity(NodeController node)
    {
        node.buildingType = BuildingType.City;
        Destroy(node.transform.GetChild(0).gameObject);
        GameObject buildingSprite = Instantiate(cityPrefabs[ColorIndex(node.color)]);
        buildingSprite.transform.position = node.transform.position;
        buildingSprite.transform.parent = node.transform;
    }
    public void AddRoad(EdgeController edge)
    {
        var roadSprite = Instantiate(roadPrefabs[ColorIndex(edge.color)]);
        roadSprite.transform.position = edge.transform.position;
        roadSprite.transform.rotation = edge.transform.rotation;
        roadSprite.transform.parent = edge.transform;
    }
    int ColorIndex(PlayerColor color) => (int)color;
    // Update is called once per frame
    void Update()
    {
        
    }
}
