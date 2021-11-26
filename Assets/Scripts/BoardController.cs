using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const int TILES_COUNT = 19;
    public const int NODES_COUNT = 54;
    public const int EDGES_COUNT = 72;
    public TileController[] tiles;
    public NodeController[] nodes;
    public EdgeController[] edges;
    // Start is called before the first frame update
    void Start()
    {
        int[][] neighborNodesOfTile = new int[TILES_COUNT][] {
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

        tiles = new TileController[TILES_COUNT];
        nodes = new NodeController[NODES_COUNT];
        edges = new EdgeController[EDGES_COUNT];

        for (int i = 0; i < TILES_COUNT; ++i)
            tiles[i] = new TileController();
        for (int i = 0; i < NODES_COUNT; ++i)
            nodes[i] = new NodeController();
        for (int i = 0; i < EDGES_COUNT; ++i)
            edges[i] = new EdgeController();
        for (int i = 0; i < TILES_COUNT; ++i)
            tiles[i].nodes = neighborNodesOfTile[i].Select(n => nodes[n]).ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
