using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private Tile[] tiles;

    private int[,] grid;

    private Dictionary<char, GameObject> tilemap = new Dictionary<char, GameObject>();

    private void Start()
    {
        foreach(Tile tile in tiles)
        {
            tilemap.Add(tile.Symbol, tile.TileObject);
        }
    }
}

[System.Serializable]
public class Tile
{
    [SerializeField]
    private char symbol;
    [SerializeField]
    private GameObject tileObject;

    public char Symbol
    { get { return symbol; } set { symbol = value; } }
    public GameObject TileObject
    {  get { return tileObject; } set { tileObject = value; } }
}