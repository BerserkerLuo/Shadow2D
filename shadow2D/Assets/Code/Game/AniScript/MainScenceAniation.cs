using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ImageObj
{
    public RectTransform IamgeTrans;
    public float Speed;

    private RectTransform cloneIamgeTrans;
    private float screenWidth;

    public void Init() {
        RawImage rawImage = IamgeTrans.GetComponent<RawImage>();
        RawImage cloneImage = GameObject.Instantiate(rawImage, IamgeTrans.parent);
        cloneIamgeTrans = cloneImage.rectTransform;
        screenWidth = cloneIamgeTrans.rect.width;
        cloneIamgeTrans.anchoredPosition = new Vector3(screenWidth,0,0);
    }

    public void Move(float deltaTime) {
        float movestep = deltaTime * Speed;
        Move(IamgeTrans, movestep);
        Move(cloneIamgeTrans, movestep);
    }
    public void Move(RectTransform trans, float moveStep)
    {
        Vector3 pos = trans.anchoredPosition;
        pos.x -= moveStep;
        if (pos.x < -screenWidth)
            pos.x += screenWidth * 2;
        trans.anchoredPosition = pos;
    }
}

public class MainScenceAniation : MonoBehaviour
{
    public List<ImageObj> ImageList;
    void Start()
    {
        foreach (var it in ImageList)
            it.Init();
    }


    void Update()
    {
        float time = Time.deltaTime;
        foreach (var it in ImageList)
            it.Move(time);
    }
}
