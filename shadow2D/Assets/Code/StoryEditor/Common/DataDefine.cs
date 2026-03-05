using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace StoryEditor {
    //============================================================================================================================


    //============================================================================================================================
    //通用容器
    public class StoryContainer {
        public VisualElement MainBlock;
        public VisualElement ContentBlock;
        public VisualElement ButtonBlock;
        public Button Button;
    }

    //剧情组件
    public class StoryComponent : StoryContainer
    {
        public StoryContainer Top;
        public VisualElement ItemContent;

        public PopupField<ECompType> Option;
        public List<StoryComponentItem> ItemList = new List<StoryComponentItem>();
    }

    //组件Item
    public class StoryComponentItem : StoryContainer {
        public TextField ItemID;
        public Label Name;
        public TextField Count;
        public TextField Weight;
    }
    //============================================================================================================================
    public class EventItem : StoryContainer {
        public int EventId;
        public Color Color;

        //public E
        public Label EventIDLabel;
        public TextField EventName;
        public TextField EventDescribe;
        public VisualElement EventColor;

        public bool FoldFlag = false;
        public Button FoldButton;
    }
}
