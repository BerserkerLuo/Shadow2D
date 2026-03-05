
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using System.Collections.Generic;

using System.Linq;
using Table;

namespace StoryEditor {
    public class StoryNodeView : Node
    {
        public static int MakeGuId = 0;

        private int guId;
       
        public int EventId;

        public Label GUIDText;
        public TextField WeightText;
        public PopupField<int> EventIdOp;
        public TextField TitleText;
        public TextField StoryText;
        public PopupField<EGotoType> GotoType;
        public HashSet<StoryComponent> StoryComponentList = new HashSet<StoryComponent>();

        public Port inputPort;
        public Port outputPort;

        private Dictionary<string, StoryContainer> elementMap = new Dictionary<string, StoryContainer>();
        private VisualElement ComponentBlock;

        public bool FoldFlag = false;
        private Button FoldButton;

        public int GUID{
            get { return guId; }
            set{
                guId = value;
                GUIDText.text = value.ToString();
            }
        }

        public StoryNodeView()
        {
            titleContainer.style.display = DisplayStyle.None;
            mainContainer.style.minWidth = 180;
            mainContainer.style.backgroundColor = UITool.rgb(32);

            // 输入===================================================
            inputPort = CreatePort(Orientation.Vertical, Direction.Input);
            StoryContainer inputComp = AddElement("inputPort", outputPort, false, false);
            VisualElement inputContainer = inputComp.ContentBlock;
            inputContainer.style.flexDirection = FlexDirection.Row;
            inputContainer.style.alignItems = Align.Center;
            inputContainer.style.overflow = Overflow.Hidden;

            VisualElement topBlock1 = UITool.Block();
            topBlock1.style.width = 80;
            topBlock1.style.color = UITool.rgb(0, 0);
            inputContainer.Add(topBlock1);
            inputContainer.Add(inputPort);

            VisualElement topBlock2 = UITool.Block();
            topBlock2.style.width = 80;
            topBlock1.style.color = UITool.rgb(0, 0);
            topBlock2.style.flexDirection = FlexDirection.Row;
            topBlock2.style.justifyContent = Justify.FlexEnd;
            topBlock2.style.alignItems = Align.Center;

            FoldButton = UITool.Button();
            FoldButton.text = "-";
            FoldButton.style.width = 16;
            FoldButton.style.paddingRight = 5;
            UITool.SetTransparentHover(FoldButton, UITool.rgb(256));
            FoldButton.RegisterCallback<MouseUpEvent>(_ => { OnFold(); });
            topBlock2.Add(FoldButton);

            inputContainer.style.height = 16;
            inputContainer.Add(topBlock2);

            //剧情ID===================================================
            VisualElement Idlock = UITool.Block();
            Idlock.style.width = 180;
            Idlock.style.justifyContent = Justify.Center;
            Idlock.style.alignItems = Align.Center;
            AddElement("Idlock", Idlock, false, false);

            //剧情ID
            GUIDText = UITool.Label();
            GUIDText.style.height = 16;
            GUIDText.style.width = 60;
            //UITool.SetTransparentHover(GUIDText, UITool.rgb(256));
            Idlock.Add(GUIDText);

            //权重
            WeightText = UITool.TextInput();
            WeightText.style.height = 16;
            WeightText.style.width = 60;
            UITool.SetTransparentHover(WeightText, UITool.rgb(256));
            UITool.ListenDirty(WeightText);
            WeightText.value = "100";
            Idlock.Add(WeightText);

            //事件ID
            EventIdOp = UITool.PopupField(Global.EventList);
            EventIdOp.style.height = 16;
            EventIdOp.style.width = 60;
            UITool.SetTransparentHover(EventIdOp, UITool.rgb(256));
            EventIdOp.RegisterValueChangedCallback(evt =>{OnEventIdChange(evt.newValue); });
            UITool.ListenDirty(EventIdOp);
            Idlock.Add(EventIdOp);

            //标题===================================================
            TitleText = UITool.TextInput("");
            var input = TitleText.Q("unity-text-input");
            //input.style.color = GetColor(255, 0, 0);             
            input.style.fontSize = 18;
            input.style.color = UITool.rgb(116, 105, 182);
            AddElement("TitleText", TitleText);
            UITool.ListenDirty(TitleText);

            //剧情===================================================
            StoryText = UITool.TextInput("");  
            AddElement("StoryText", StoryText); 
            UITool.ListenDirty(StoryText);

            //组件块===================================================
            ComponentBlock = UITool.Block();
            ComponentBlock.style.flexDirection = FlexDirection.Column;
            AddElement("ComponentBlock", ComponentBlock, false, false);

            //添加组件按钮===================================================
            Button button = UITool.Button();
            button.text = "+";
            button.style.height = 16;
            button.style.color = UITool.rgb(65, 200, 25);
            UITool.SetHoverColor(button, UITool.rgb(32), UITool.rgb(64));
            button.RegisterCallback<MouseUpEvent>(_ => {
                StoryComponent comp = CreateNewComponent();
                AddItem(comp);
            });
            StoryContainer AddComponent = AddElement("AddComponent", button);

            //跳转方式===================================================
            GotoType = UITool.PopupField(Global.GotoOptions);
            UITool.SetHoverColor(GotoType, UITool.rgb(32), UITool.rgb(64));
            AddElement("GotoType", GotoType, false, false);
            UITool.ListenDirty(GotoType);

            //输出===================================================
            outputPort = CreatePort(Orientation.Vertical, Direction.Output);
            StoryContainer outputComp = AddElement("outputPort", outputPort, false, false);
            VisualElement outputContainer = outputComp.ContentBlock;
            outputContainer.style.alignItems = Align.Center;
            outputContainer.style.overflow = Overflow.Visible;

            outputContainer.style.height = 16;
            outputContainer.Add(outputPort);

            //===================================================
            OnEventIdChange(EventIdOp.value);
            //AddItem(CreateNewComponent());

            StoryGotoTypeHide(true);

            RefreshExpandedState();
            RefreshPorts();
            SetPosition(new Rect(300, 200, 200, 150));

            GUID = ++MakeGuId;
        }

        public Port CreatePort(Orientation orientation, Direction direction) {
            Port port = InstantiatePort(orientation, direction, Port.Capacity.Multi, typeof(int));
            port.portName = "";
            port.style.justifyContent = Justify.Center;
            VisualElement element = port.Query("type");
            element.style.display = DisplayStyle.None;
            return port;
        }

        private StoryContainer AddElement(string name, VisualElement element, bool showRemove = false, bool showBroder = true)
        {
            StoryContainer component = elementMap.GetValueOrDefault(name, null);
            if (component == null)
            {
                component = UITool.CreateGeneralContainer<StoryContainer>();

                if (showBroder)
                    UITool.AddBottomBorder(component.MainBlock);

                component.ButtonBlock.style.display = showRemove ? DisplayStyle.Flex : DisplayStyle.None;
                component.Button.RegisterCallback<MouseUpEvent>(_ => {
                    mainContainer.Remove(component.MainBlock);
                    elementMap.Remove(name);
                });

                mainContainer.Add(component.MainBlock);
                elementMap.Add(name, component);
            }

            component.ContentBlock.Add(element);
            return component;
        }
         
        //========================================================================================
        public StoryComponent CreateNewComponent() { 
            StoryComponent storyComponent = UITool.CreateStoryComponent();
            ComponentBlock.Add(storyComponent.MainBlock);
            StoryComponentList.Add(storyComponent);

            storyComponent.Button.RegisterCallback<MouseUpEvent>(_ => {
                ComponentBlock.Remove(storyComponent.MainBlock);
                StoryComponentList.Remove(storyComponent);
            });

            storyComponent.Top.Button.RegisterCallback<MouseUpEvent>(_ => {
                AddItem(storyComponent);
            });

            storyComponent.Option.RegisterValueChangedCallback(evt => {
                OnComponentTypeChange(storyComponent,evt.newValue);
            });

            return storyComponent;
        }

        public StoryComponentItem AddItem(StoryComponent storyComponent) {
            StoryComponentItem item = UITool.AddStoryComponentItem(storyComponent);
            storyComponent.ItemList.Add(item);

            item.Button.RegisterCallback<MouseUpEvent>(_ => {
                storyComponent.ContentBlock.Remove(item.MainBlock);
                storyComponent.ItemList.Remove(item);
            });

            item.ItemID.RegisterValueChangedCallback(evt => {
                item.ItemID.value = evt.newValue;
                OnItemIdChange(storyComponent,item);
            });

            item.Weight.value = "100";

            UITool.ListenDirty(item.ItemID);
            UITool.ListenDirty(item.Count);
            UITool.ListenDirty(item.Weight);

            return item;
        }

        public void OnComponentTypeChange(StoryComponent comp,ECompType type) {
            comp.Option.value = type;
            foreach (var it in comp.ItemList)
                OnItemIdChange(comp, it);
        }
        public void OnItemIdChange(StoryComponent comp, StoryComponentItem item) {
            int id = ParseInt(item.ItemID.value, 0);
            string name = "未定义";  
            switch (comp.Option.value) {
                case ECompType.Battle: name = GetEnemyName(id); break;
                case ECompType.Reward: name = GetItemName(id); break;
                case ECompType.EventUnlock: name = GetEventName(id); break;
            } 
            if (name.Length > 6) item.Name.text = name.Substring(0, 6);
            else item.Name.text = name;
        } 

        public string GetItemName(int id) {
            ItemCfg itemCfg = TableMgr.Singleton.GetItemCfg(id);
            if (itemCfg == null) return "未定义";
            return itemCfg.Name;
        }

        public string GetEnemyName(int id) {
            MonsterCfg monsterCfg = TableMgr.Singleton.GetMonsterCfg(id);
            if (monsterCfg == null) return "未定义";
            return monsterCfg.Name;
        }

        public string GetEventName(int id) {
            return EventWindow.Singleton.GetEventName(id);
        }

        //========================================================================================
        public void SetElementHide(string name, bool hide){
            StoryContainer container = elementMap.GetValueOrDefault(name, null);
            if (container == null)
                return;
            SetElementHide(container,hide);
        }
        public void SetElementHide(StoryContainer container, bool hide){
            SetElementHide(container.MainBlock,hide);
        }
        public void SetElementHide(VisualElement e, bool hide)
        {
            e.style.display = hide ? DisplayStyle.None : DisplayStyle.Flex;
        }

        public void StoryGotoTypeHide(bool display = true) { SetElementHide("GotoType", display); }

        public void OnFold(int flag = 0){
            if (flag == 1) FoldFlag = true;
            else if (flag == 2) FoldFlag = false;
            else FoldFlag = !FoldFlag;

            FoldButton.text = FoldFlag ? "=" : "-";

            SetElementHide("AddComponent", FoldFlag);
            if (TitleText.text == "") SetElementHide("TitleText", FoldFlag);
            if(StoryText.text == "") SetElementHide("StoryText", FoldFlag);

            foreach (var it in StoryComponentList)  
                OnZoom(it);
        } 
        public void OnZoom(StoryComponent comp) {
            SetElementHide(comp.ButtonBlock, FoldFlag);

            if (comp.Option.value == ECompType.EndResult ||
                comp.Option.value == ECompType.EventEnd)
                return;

            if (comp.ItemList.Count == 0) {
                SetElementHide(comp,FoldFlag);
                return;
            }

            bool hideAll = true;
            foreach (var it in comp.ItemList) {
                if (it.ItemID.text != "" || it.Count.text != ""){
                    hideAll = false;
                    continue;
                }
                SetElementHide(it, FoldFlag);
            }

            if(hideAll) SetElementHide(comp, FoldFlag);
        }

        public void OnGotoChange(int addcount = 0) {
            bool hide = GetNextNodeList().Count + addcount > 1 ? false: true ;
            StoryGotoTypeHide(hide);
        }

        //========================================================================================
        public void SetEventId(int eventId) {
            if (EventId == eventId) return;
            EventIdOp.value = eventId;
            OnEventIdChange(eventId);
        }
        public void OnEventIdChange(int eventId) {
            EventId = eventId;
            EventIdOp.value = eventId;
            Color color = EventWindow.Singleton.GetEventColor(EventId);
            ChangeEventColor(color);

            List<StoryNodeView> prevList = GetPrevNodeList();
            foreach (var it in prevList) it.SetEventId(eventId); 
            List<StoryNodeView> nextList = GetNextNodeList(); 
            foreach (var it in nextList) it.SetEventId(eventId);
        }

        public void ChangeEventColor(Color color) {
            inputPort.parent.style.backgroundColor = color;
        }

        //========================================================================================
        public void SetStoryNodeInfo(StoryNodeInfo info) {

            TitleText.value = info.Title;
            StoryText.value = info.Text;
            GotoType.value = (EGotoType)info.GoType;
            SetPosition(new Rect(info.pos.x, info.pos.y, 0, 0));
            if (info.EnemyList != null && info.EnemyList.Count > 0)
                AddStoryComponentByMap(ECompType.Battle, info.EnemyList); 
            if (info.RewardList != null && info.RewardList.Count > 0)
                AddStoryComponentByMap(ECompType.Reward, info.RewardList);
            if (info.ULEIdList != null && info.ULEIdList.Count > 0)
                AddStoryComponentByList(ECompType.EventUnlock, info.ULEIdList);

            if(info.IsEnd)
                AddStoryComponentByList(ECompType.EndResult, null);

            if (info.isEventEnd)
                AddStoryComponentByList(ECompType.EventEnd, null);

            SetEventId(info.eventId);

            if (MakeGuId < GUID) MakeGuId = GUID;
        }

        public void AddStoryComponentByMap(ECompType type, List<IntIntPair> valueMap) {
            StoryComponent comp = CreateNewComponent();
            comp.Option.value = type;
            foreach (var it in valueMap) {
                StoryComponentItem item = AddItem(comp);
                item.ItemID.value = it.Key.ToString();
                item.Count.value = it.Value.ToString();
                item.Weight.value = it.value2.ToString();
            }
        }

        public void AddStoryComponentByList(ECompType type, List<int> valueList){
            StoryComponent comp = CreateNewComponent();
            comp.Option.value = type;

            if (valueList == null) return;
            foreach (int it in valueList){
                StoryComponentItem item = AddItem(comp);
                item.ItemID.value = it.ToString();
            }
        }

        //========================================================================================
        public StoryNodeInfo GetStoryNodeInfo() {
            StoryNodeInfo retInfo = new StoryNodeInfo();
            retInfo.Id = GUID;
            retInfo.eventId = EventId;
            retInfo.Title = TitleText.value;
            retInfo.Text = StoryText.value;
            retInfo.GoType = (int)GotoType.value;
            retInfo.GoList = GetGotoList();

            Rect rect = GetPosition();
            retInfo.pos = new Vector2(rect.x,rect.y); 

            foreach (StoryComponent comp in StoryComponentList)
                if (comp.Option.value == ECompType.Battle)
                    GetIntMap(comp, retInfo.EnemyList);
                else if (comp.Option.value == ECompType.Reward)
                    GetIntMap(comp, retInfo.RewardList);
                else if (comp.Option.value == ECompType.EventUnlock)
                    GetIntList(comp, retInfo.ULEIdList);
                else if (comp.Option.value == ECompType.EndResult)
                    retInfo.IsEnd = true;
                else if (comp.Option.value == ECompType.EventEnd)
                    retInfo.isEventEnd = true;

            return retInfo;
        }

        public List<StoryNodeView> GetPrevNodeList() {
            return inputPort.connections
                .Select(edge => edge.output?.node as StoryNodeView)
                .Where(n => n != null)
                .ToList();
        }

        public List<StoryNodeView> GetNextNodeList()
        {
            return outputPort.connections
                .Select(edge => edge.input?.node as StoryNodeView)
                .Where(n => n != null)
                .ToList();
        }

        public List<int> GetGotoList()
        {
            List<StoryNodeView> nodeList = GetNextNodeList();
            List<int> retList = new List<int>();
            foreach (var it in nodeList)
                retList.Add(it.GUID);
            return retList;
        }

        public int ParseInt(string str, int def = 0){
            try{return int.Parse(str);}
            catch (Exception){ return def; }
        }
            public void GetIntMap(StoryComponent comp, List<IntIntPair> RetMap) {
            if (RetMap == null)
                return;

            foreach (StoryComponentItem it in comp.ItemList) {
                try {
                    int id = ParseInt(it.ItemID.value);
                    int count = ParseInt(it.Count.value);
                    int weight = ParseInt(it.Weight.value,100);
                    RetMap.Add(new IntIntPair(id, count, weight));
                }
                catch (Exception) { }
            }
        }

        public void GetIntList(StoryComponent comp, List<int> RetList) {
            if (RetList == null)
                return;

            foreach (StoryComponentItem it in comp.ItemList) {
                try{
                    int id = int.Parse(it.ItemID.value);
                    RetList.Add(id);
                }
                catch (Exception) { }
            }
        }
    }
}

