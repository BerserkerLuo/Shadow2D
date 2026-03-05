using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Tool;

namespace StoryEditor
{
    public class EventWindow : FloatingWindowBase<EventWindow>
    {
        public static int MakeEventId = 0;

        private Button FoldButton;

        private VisualElement ContentBlock;
        public Dictionary<int, EventItem> EventMap = new Dictionary<int, EventItem>();

        public EventWindow() {
            TitleLabel.text = "EventList";

            TitleLabel.style.width = Length.Percent(85);

            //折叠全部
            VisualElement buttonBlock = UITool.Block();
            buttonBlock.style.flexDirection = FlexDirection.Row;
            buttonBlock.style.alignItems = Align.FlexEnd;
            buttonBlock.style.width = Length.Percent(15);
            TopBlock.Add(buttonBlock);

            FoldButton = UITool.Button();
            FoldButton.text = "=";
            FoldButton.style.width = 16;
            FoldButton.style.paddingRight = 5;
            UITool.SetTransparentHover(FoldButton, UITool.rgb(256));
            FoldButton.RegisterCallback<MouseUpEvent>(_ => { OnFoldAll(); });
            buttonBlock.Add(FoldButton);

            //事件列表
            var EventList = new ScrollView();
            EventList.style.width = Length.Percent(100);
            EventList.style.height = 400;
            Add(EventList);

            ContentBlock = UITool.Block();
            ContentBlock.style.flexDirection = FlexDirection.Column;
            ContentBlock.style.minHeight = 50;
            EventList.Add(ContentBlock);

            StoryContainer AddBlock = UITool.CreateGeneralContainer<StoryContainer>();
            AddBlock.MainBlock.Remove(AddBlock.ContentBlock);
            AddBlock.ButtonBlock.style.width = Length.Percent(100);
            AddBlock.Button.style.height = 16;
            AddBlock.Button.text = "+";
            AddBlock.Button.style.color = UITool.rgb(65, 200, 25);
            UITool.SetHoverColor(AddBlock.Button, UITool.rgb(32), UITool.rgb(64));
            AddBlock.Button.RegisterCallback<MouseUpEvent>(_ => {
                AddNewEvent();
            });
            Add(AddBlock.MainBlock);
        }

        public EventItem AddNewEvent() {
            EventItem item = CreateNewEventItem();
            item.EventId = ++MakeEventId;
            item.EventIDLabel.text = item.EventId.ToString();

            AddEventItem(item);

            OnChangeEventColor(item, ColorSelectWindow.Singleton.GetColorByIndex(item.EventId - 1));

            return item; 
        }

        public void AddEventItem(EventItem item) {

            Global.EventList.Add(item.EventId);
            ContentBlock.Add(item.MainBlock);
            EventMap.Add(item.EventId, item);

            item.Button.RegisterCallback<MouseUpEvent>(_ => {
                ContentBlock.Remove(item.MainBlock);
                EventMap.Remove(item.EventId);
                Global.EventList.Remove(item.EventId);
            });
        }

        public EventItem CreateNewEventItem() {
            EventItem item = UITool.CreateGeneralContainer<EventItem>();

            item.MainBlock.style.flexDirection = FlexDirection.Column;
            UITool.SetBorder(item.MainBlock, 1, UITool.rgb(10));
            item.MainBlock.Remove(item.ContentBlock);
            item.MainBlock.Add(item.ContentBlock);
            item.ContentBlock.style.flexDirection = FlexDirection.Column;
            item.ButtonBlock.style.height = new StyleLength(StyleKeyword.Auto); 

            //缩放按钮
            item.FoldButton = UITool.Button();
            item.FoldButton.style.height = Length.Percent(100);
            
            item.FoldButton.text = "="; 
            UITool.SetHoverColor(item.FoldButton, UITool.rgb(32), UITool.rgb(64));
            item.FoldButton.RegisterCallback<MouseUpEvent>(_ => { OnFold(item); });

            //删除按钮
            UITool.SetHoverColor(item.Button, UITool.rgb(32), UITool.rgb(64));
            item.ButtonBlock.Remove(item.Button);

            //ID标签
            item.EventIDLabel = UITool.Label();
            item.EventIDLabel.text = "0";
            item.EventIDLabel.style.backgroundColor = UITool.rgb(25);
            UITool.SetBorder(item.EventIDLabel, 1, UITool.rgb(25));
            item.EventIDLabel.style.height = Length.Percent(100);

            //颜色标签
            item.EventColor = UITool.Block();
            item.EventColor.style.height = Length.Percent(100);
            item.EventColor.pickingMode = PickingMode.Position;
            UITool.SetBorder(item.EventColor, 1, UITool.rgb(10));
            item.EventColor.RegisterCallback<PointerDownEvent>((evt => {
                evt.PreventDefault();
                evt.StopImmediatePropagation();
                ColorSelectWindow.Singleton.Show(evt, (color => {
                    OnChangeEventColor(item, color);
                }));
            }));

            item.EventName = UITool.TextInput();
            UITool.ListenDirty(item.EventName);

            item.ButtonBlock.style.width = Length.Percent(100);

            item.EventIDLabel.style.width = Length.Percent(15);
            item.EventName.style.width = Length.Percent(50);
            item.EventColor.style.width = Length.Percent(15);
            item.FoldButton.style.width = Length.Percent(10);
            item.Button.style.width = Length.Percent(10);


            item.ButtonBlock.Add(item.EventIDLabel);
            item.ButtonBlock.Add(item.EventName);
            item.ButtonBlock.Add(item.EventColor);
            item.ButtonBlock.Add(item.FoldButton);
            item.ButtonBlock.Add(item.Button);

            //item.ContentBlock.Add(CreateLine("Name", item.EventName));

            item.EventDescribe = UITool.TextInput();
            item.EventDescribe.style.maxWidth = 208;
            item.EventDescribe.style.width = Length.Percent(100);
            UITool.ListenDirty(item.EventDescribe);
            item.ContentBlock.Add(item.EventDescribe);

            OnFold(item);
            return item;
        }

        bool foldAll = true;
        public void OnFoldAll() {
            foreach (var it in EventMap.Values) {
                it.FoldFlag = foldAll;
                OnFold(it);
            }
            foldAll = !foldAll;
            FoldButton.text = foldAll ? "=" : "-";
        }
        public void OnFold(EventItem item) {
            item.FoldFlag = !item.FoldFlag;
            item.FoldButton.text = item.FoldFlag ? "=" : "-";
            item.ContentBlock.style.display = item.FoldFlag ? DisplayStyle.None : DisplayStyle.Flex;

            UITool.SetBorder(item.MainBlock, 1, item.FoldFlag ? UITool.rgb(10) : UITool.rgb(128));
        }

        public VisualElement CreateLine(string title,VisualElement e)
        {
            VisualElement main = UITool.Block();

            main.style.backgroundColor = Color.black;
            UITool.SetPadding(main, 1);

            Label titleL = UITool.Label();
            titleL.style.width = 40;
            titleL.style.height = Length.Percent(100);
            titleL.text = title;
            titleL.style.backgroundColor = UITool.rgb(25);
            UITool.SetBorder(titleL, 1, UITool.rgb(10));
            UITool.SetPadding(titleL, 0);
            main.Add(titleL);

            e.style.height = Length.Percent(100);
            e.style.width = 133;
            UITool.SetBorder(e, 1, UITool.rgb(10));
            UITool.SetPadding(e, 0);
            e.style.marginLeft = 1;
            main.Add(e);

            return main;
        }

        public void OnChangeEventColor(EventItem item,Color color) {
            item.Color = color;
            item.EventColor.style.backgroundColor = color;
            Dictionary<int, StoryNodeView> NodeMap = StoryGraphView.Singleton.NodeMap;
            foreach(var it in NodeMap.Values) {
                if (it.EventId != item.EventId)
                    continue;
                it.ChangeEventColor(color);
            }
        }

        private Color DefaultColor = UITool.rgb(32);
        public Color GetEventColor(int eventId) {
            EventItem eventItem = EventMap.GetValueOrDefault(eventId, null);
            if (eventItem == null)
                return DefaultColor;
            return eventItem.Color;
        }
        //====================================================================================
        public string GetEventName(int eventId) {
            EventItem eventItem = EventMap.GetValueOrDefault(eventId, null);
            if (eventItem == null)
                return "未定义";
            return eventItem.EventName.value;
        }
        //====================================================================================
        public void ClearAll() {
            MakeEventId = 0;
            for (int i = 0; i < 1000 && ContentBlock.childCount > 0; ++i)
                ContentBlock.RemoveAt(0);
            EventMap.Clear();
            Global.EventList.Clear();
        }

        public void SetEventList(List<EventInfo> EventInfoList) {

            foreach (var info in EventInfoList) {
                EventItem item = CreateNewEventItem();
                item.EventId = info.Id;
                item.EventIDLabel.text = info.Id.ToString();
                item.EventName.value = info.Name;
                item.EventDescribe.value = info.Desc;

                OnChangeEventColor(item,UITool.rgb(info.r, info.g, info.b));

                AddEventItem(item);

                if (MakeEventId < info.Id) MakeEventId = info.Id;
            }
        }

        public List<EventInfo> GetEventInfoList() {
            List<EventInfo> EventInfoList = new List<EventInfo>();
            foreach (var item in EventMap.Values) {
                EventInfo info = new EventInfo();

                info.Id = item.EventId;
                info.Name = item.EventName.value;
                info.Desc = item.EventDescribe.value;
                info.r = (int)(item.Color.r * 25500) / 100;
                info.g = (int)(item.Color.g * 25500) / 100;
                info.b = (int)(item.Color.b * 25500) / 100;

                EventInfoList.Add(info);
            }
            return EventInfoList;
        }
    }

}