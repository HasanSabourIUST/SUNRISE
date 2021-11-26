using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public enum BuildingType { Settlement, City}
    public enum PlayerColor { None, Blue, Red, Green, Yellow}
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
