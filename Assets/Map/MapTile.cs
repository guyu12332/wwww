using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapTileType : byte
{
    Normal, Building
}
public class MapTile : MonoBehaviour
{

    // Use this for initialization
    private int x;
    private int y;
    public MapTileType type;

    public int X
    {
        get
        {
            return x;
        }
 
    }

    public int Y
    {
        get
        {
            return y;
        }
 
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setXY(int x, int y)
    {
        this.transform.GetComponentInChildren<Text>().text = x + "," + y;
        this.x = x;
        this.y = y;
    }
}
