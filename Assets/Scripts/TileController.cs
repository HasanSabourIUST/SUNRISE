using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public enum TileType { Forest, ClayHill, Field, Farm, Mountain, Desert}
    public TileType type;
    public NodeController[] nodes;
    // Start is called before the first frame update
    void Start()
    {
        nodes = new NodeController[6];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
