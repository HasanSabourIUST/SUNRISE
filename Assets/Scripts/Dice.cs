using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public Sprite[] sprites;
    public int value;

    private void Start()
    {
        Roll();
    }

    public void Roll()
    {
        value = Random.Range(0, 6) + 1;
        GetComponent<SpriteRenderer>().sprite = sprites[value - 1];
    }
}
