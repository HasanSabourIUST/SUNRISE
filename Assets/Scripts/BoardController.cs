using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public const int TILES_COUNT = 19;
    public const int NODES_COUNT = 56;
    public const int EDGES_COUNT = 72;
    public TileController[] tiles;
    public NodeController[] nodes;
    public EdgeController[] edges;
    // Start is called before the first frame update
    void Start()
    {
        tiles = new TileController[TILES_COUNT];
        nodes = new NodeController[NODES_COUNT];
        edges = new EdgeController[EDGES_COUNT];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
