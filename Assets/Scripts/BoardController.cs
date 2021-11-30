using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const int TILES_COUNT = 19;
    public const int NODES_COUNT = 54;
    public const int EDGES_COUNT = 72;
    public TileController tilePrefab;
    public NodeController nodePrefab;
    public EdgeController edgePrefab;
    public TileController[] tiles;
    public NodeController[] nodes;
    public EdgeController[] edges;
    public const float tileLength = 1;
    public Vector2 diagonalStep = new Vector2(tileLength / 2, tileLength * Mathf.Sqrt(3) / 2);
    public Vector2 straightStep = new Vector2(tileLength, 0);
    // Start is called before the first frame update
    void Start()
    {
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
        
        tiles = new TileController[TILES_COUNT];
        nodes = new NodeController[NODES_COUNT];
        edges = new EdgeController[EDGES_COUNT];

        for (int i = 0; i < TILES_COUNT; ++i)
        {
            tiles[i] = Instantiate(tilePrefab);
            tiles[i].transform.parent = transform;
        }
        for (int i = 0; i < NODES_COUNT; ++i)
        {
            nodes[i] = Instantiate(nodePrefab);
            nodes[i].transform.parent = transform;
        }
        for (int i = 0; i < EDGES_COUNT; ++i)
        {
            edges[i] = Instantiate(edgePrefab);
            edges[i].transform.parent = transform;
        }

        for (int i = 0; i < TILES_COUNT; ++i)
            tiles[i].nodes = neighborNodesOfTile[i].Select(n => nodes[n]).ToArray();
        for (int i = 0; i < NODES_COUNT; ++i)
            nodes[i].edges = neighborEdgesOfNode[i].Select(e => edges[e]).ToList();
        for (int i = 0; i < EDGES_COUNT; ++i)
            edges[i].nodes = neighborNodesOfEdge[i].Select(n => nodes[n]).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
