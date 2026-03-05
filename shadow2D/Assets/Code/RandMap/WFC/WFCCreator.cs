using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using WFC;

public class WFCCreator : MonoBehaviour
{
    public Tilemap outputImage;
    public GameObject inputList;
    public List<Tilemap> inputTiles;
    [Tooltip("Output image width")]
    public int outputWidth = 5;
    [Tooltip("Output image height")]
    public int outputHeight = 5;

    WFC2 wfc2;

    public void CreateNewMap() {
        wfc2 = new WFC2(inputTiles,outputImage, outputWidth, outputHeight);
        wfc2.CreateNewTileMap();
    }
    public void SaveTilemap()
    {
        GameObject objectToSave = outputImage.gameObject;
        //PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Resources/output.prefab");
    }

    public void TestCreate() {
        wfc2 = new WFC2(inputTiles, outputImage, outputWidth, outputHeight);
        wfc2.TestCreate();

        outputImage.ClearAllTiles();
    }

    public void TestTick()
    {
        wfc2.ExcuteTick();
    }


}