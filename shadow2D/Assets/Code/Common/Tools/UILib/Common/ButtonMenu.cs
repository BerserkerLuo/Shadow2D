using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{
    private Button button;
    private Text text;
    private EventTrigger eventTrigger;
    private Color origiralColor;

    void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        eventTrigger = GetComponent<EventTrigger>();

        // 鼠标按下事件
        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerDown;
        entry1.callback = new EventTrigger.TriggerEvent();
        entry1.callback.AddListener(OnPointerDown);
        eventTrigger.triggers.Add(entry1);

        // 鼠标抬起事件
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback = new EventTrigger.TriggerEvent();
        entry2.callback.AddListener(OnPointerUp);
        eventTrigger.triggers.Add(entry2);
    }

    public void OnPointerDown(BaseEventData pointData)
    {
        //if (null == text)
        //    return;
        //text.fontSize = 28;
        //origiralColor = text.color;
        //text.color = Color.black;
    }

    public void OnPointerUp(BaseEventData pointData)
    {
        //if (null == text)
        //    return;
        //text.fontSize = 33;
        //text.color = origiralColor;
    }
}
