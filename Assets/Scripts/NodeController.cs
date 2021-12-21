using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class NodeController : MonoBehaviour
{
    public BuildingType buildingType;
    public PlayerColor color;
    public List<EdgeController> edges;
    // Start is called before the first frame update
    void Start()
    {
        edges = new List<EdgeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
