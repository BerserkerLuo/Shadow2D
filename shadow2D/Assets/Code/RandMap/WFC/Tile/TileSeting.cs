using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WFC;

public enum NeighberType
{
    Grass = 1,  //草
    Road = 2,   //路
    Wall = 3,   //墙
    Water = 4,  //水
}
public enum TileType
{
    Water = 1,    //水
    Land = 2,   //陆地
    Wall = 3,   //墙
}
public class TileSeting : MonoBehaviour
{
    public int number = 1;

    public TileType tileType = TileType.Land;
    public Dictionary<int, List<TileSeting>> neighbours = new Dictionary<int, List<TileSeting>>();
    public List<TileSeting> Up = new List<TileSeting>();
    public List<TileSeting> Right = new List<TileSeting>();
    public List<TileSeting> Down = new List<TileSeting>();
    public List<TileSeting> Left = new List<TileSeting>();
    public int weight = 1;

    public string name = "";

    public void Start()
    {
        InitNeighbour();
    }

    public void InitNeighbour() {
        neighbours.Clear();
        neighbours.Add(Dire.Up, Up);
        neighbours.Add(Dire.Right, Right);
        neighbours.Add(Dire.Down, Down);
        neighbours.Add(Dire.Left, Left);

        //Up.Clear();
        //Right.Clear();
        //Down.Clear();
        //Left.Clear();
    }

}