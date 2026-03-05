using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WFC;

public class TileEditerMgr : MonoBehaviour
{
    public GameObject inputList;
    public GameObject subRoot;

    public int MakeIndex = 0;

    Dictionary<int, TileSeting> tileSetingMap;
    Dictionary<int, TileSeting> subTileIdToSetingMap;

    public void OnClick(GameObject obj,int type) {
        TileEditerCell script = obj.GetComponent<TileEditerCell>();
        Debug.Log("Click " + script.number);

        script.active = !script.active;

        TileSeting seting = tileSetingMap.GetValueOrDefault(script.tileId, null);
        if (seting == null)
            return;

        TileSeting BaseSeting = subTileIdToSetingMap.GetValueOrDefault(script.number, null);
        if (BaseSeting == null)
            return;

        ToolUtil.Log("BaseId {} {} SubTileId {} subNuber {}", script.tileId, BaseSeting.number, seting.number,script.number);

        List<TileSeting> setingList = BaseSeting.neighbours.GetValueOrDefault(script.dire,null);
        if (setingList == null) {
            setingList = new List<TileSeting>();
            BaseSeting.neighbours.Add(script.dire,setingList);
        }

        if (script.active)
            setingList.Add(seting);
        else
            setingList.Remove(seting);

        SetColor(script);
    }

    public void InitTileList()
    {
        DestoryAllSubTile();
        MakeIndex = 0;
        subTileIdToSetingMap = new Dictionary<int, TileSeting>();
        tileSetingMap = new Dictionary<int, TileSeting>();

        Tilemap[] Tilemaps = inputList.GetComponentsInChildren<Tilemap>();
        int width = (int)Mathf.Sqrt(Tilemaps.Length) + 1;
        for (var index = 0; index < Tilemaps.Length; ++index)
        {
            TileSeting seting = Tilemaps[index].GetComponent<TileSeting>();
            seting.InitNeighbour();
            tileSetingMap.Add(seting.number,seting);

            InitTile(index, Tilemaps[index], seting, Tilemaps, width);
            //return;
        }

    }

    public void DestoryAllSubTile() {
        for (;subRoot.transform.childCount > 0;)
            DestroyImmediate(subRoot.transform.GetChild(0).gameObject);
    }

    public void InitTile(int index, Tilemap tileMap, TileSeting seting, Tilemap[] Tilemaps, int width) {

        int x = index % width;
        int y = index / width;

        x = x * width * 3;
        y = y * width * 3;

        tileMap.transform.localScale = new Vector3(width, width, 1);
        tileMap.transform.position = new Vector3(x,y,0);

        InitSubList(x,          y+width,    Tilemaps, seting, width, 0);
        InitSubList(x+width,    y+width-1,  Tilemaps, seting, width, 1);
        InitSubList(x+width-1,  y-1 ,       Tilemaps, seting, width, 2);
        InitSubList(x-1,        y,          Tilemaps, seting, width, 3);
    }

    public void InitSubList(int srcX,int srcY, Tilemap[] Tilemaps, TileSeting baseSeting, int width, int dire) {
        for (var index = 0; index < Tilemaps.Length; ++index) {

            TileSeting seting = Tilemaps[index].gameObject.GetComponent<TileSeting>();

            var tile = Tilemaps[index].GetTile(new Vector3Int(0, 0, 0));
            GameObject subTile = CreateObj(tile);

            TileEditerCell script = subTile.AddComponent<TileEditerCell>();
            script.number = MakeIndex++;
            script.tileEditerMgr = this;
            script.dire = dire;
            script.tileId = seting.number;

            List<TileSeting> list = baseSeting.neighbours.GetValueOrDefault(dire,null);
            script.active = CheckContains(list,seting.number);
            SetColor(script);

            subTileIdToSetingMap.Add(script.number, baseSeting);

            int x = index % width; 
            int y = index / width; 
            Vector3 pos = new Vector3(x,y,0);
            pos = TurnPos(pos, dire);

            ToolUtil.Log("Index {} pos {}",index,pos.ToString());

            pos.x += srcX + 0.05f;
            pos.y += srcY + 0.05f;

            subTile.transform.position = pos;
        }
    }

    public bool CheckContains(List<TileSeting> list,int tileId) {
        if (list == null)
            return false;
        foreach (var it in list)
            if (it.number == tileId)
                return true;
        return false;
    }

    float tempTurnValue;
    Vector3 TurnPos(Vector3 pos, int dire)
    {
        var retPos = pos;
        switch (dire)
        {
            case 1:{
                    retPos.x = pos.y;
                    retPos.y = -pos.x;
                } break;
            case 2:{
                    retPos.x = -pos.x;
                    retPos.y = -pos.y;
                } break;
            case 3:{
                    retPos.x = -pos.y;
                    retPos.y = pos.x;
                } break;
        }
        return retPos;
    }

    public GameObject CreateObj(TileBase tile) {
        GameObject obj = new GameObject();
        Tilemap tileMap = obj.AddComponent<Tilemap>();
        obj.AddComponent<TilemapRenderer>();
        obj.AddComponent<TilemapCollider2D>();
        obj.transform.parent = subRoot.transform;
        obj.transform.localScale = new Vector3(0.9f,0.9f,1);
        tileMap.SetTile(Vector3Int.zero,tile);

        return obj;
    }

    public void SetColor(TileEditerCell script) {
        Tilemap tileMap = script.GetComponent<Tilemap>();
        if(script.active)
            tileMap.color = new Color(1, 1, 1, 1);
        else
            tileMap.color = new Color(0.3f, 0.3f, 0.3f, 1);
    }

}
