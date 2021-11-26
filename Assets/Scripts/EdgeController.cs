using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NodeController;

public class EdgeController : MonoBehaviour
{
    public PlayerColor color;
    public NodeController[] nodes;
    // Start is called before the first frame update
    void Start()
    {
        nodes = new NodeController[2];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
