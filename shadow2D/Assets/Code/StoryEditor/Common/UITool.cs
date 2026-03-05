using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

namespace StoryEditor
{
    public class UITool
    {
        //===============================================================================================================
        //颜色
        #region
        public static Color rgb(int v, int a = 1) {
            return rgb(v, v, v, a);
        }
        public static Color rgb(int r, int g, int b, int a = 1)
        {
            return new Color(r / 255f, g / 255f, b / 255f, a);
        }
        #endregion
        //===============================================================================================================
        //Margin
        #region
        public static void SetMargin(VisualElement e, int length)
        {
            e.style.marginTop = length;
            e.style.marginBottom = length;
            e.style.marginLeft = length;
            e.style.marginRight = length;
        }
        #endregion
        //===============================================================================================================
        //Padding
        #region
        public static void SetPadding(VisualElement e, int length)
        {
            e.style.paddingTop = length;
            e.style.paddingBottom = length;
            e.style.paddingLeft = length;
            e.style.paddingRight = length;
        }
        #endregion
        //===============================================================================================================
        //Border
        #region
        public static void SetBorder(VisualElement e, int width, Color color = default, int radius = -1) {
            if (color == null) color = rgb(128);
            SetBorderWidth(e, width);
            SetBorderColor(e, color);
            SetBorderRadius(e, radius);
        }
        public static void SetBorderWidth(VisualElement e, int width) {
            e.style.borderTopWidth = width;
            e.style.borderBottomWidth = width;
            e.style.borderLeftWidth = width;
            e.style.borderRightWidth = width;
        }

        public static void SetBorderColor(VisualElement e, Color color) {
            e.style.borderTopColor = color;
            e.style.borderBottomColor = color;
            e.style.borderLeftColor = color;
            e.style.borderRightColor = color;
        }

        public static void SetBorderRadius(VisualElement e, int length)
        {
            e.style.borderBottomLeftRadius = length;
            e.style.borderBottomRightRadius = length;
            e.style.borderTopLeftRadius = length;
            e.style.borderTopRightRadius = length;
        }
        #endregion
        //===============================================================================================================
        //Size
        #region
        public static void SetSize(VisualElement e, int width, int height)
        {
            e.style.width = width;
            e.style.height = height;
        }
        #endregion
        //===============================================================================================================
        //分割线
        #region
        public static void AddBottomBorder(VisualElement e) {
            e.style.borderBottomWidth = 1;
            e.style.borderBottomColor = rgb(74, 74, 74); ;
        }
        #endregion
        //===============================================================================================================
        //设置边缘 Margin Border Padding 尺寸
        public static void SetEdge(VisualElement e,int margin = 0,int broderWidth = 0,int padding=0) {
            SetMargin(e, margin);
            SetBorderWidth(e,broderWidth);
            SetPadding(e, padding);
        }

        //===============================================================================================================
        //监听数值变化
        #region
        public static void ListenDirty(TextField e) {
            e.RegisterValueChangedCallback(evt => { Global.Dirty = true;});
        }
        public static void ListenDirty(DropdownField e){
            e.RegisterValueChangedCallback(evt => { Global.Dirty = true; });
        }
        public static void ListenDirty(PopupField<EGotoType> e){
            e.RegisterValueChangedCallback(evt => { Global.Dirty = true; });
        }
        public static void ListenDirty(PopupField<EventType> e)
        {
            e.RegisterValueChangedCallback(evt => { Global.Dirty = true; });
        }
        public static void ListenDirty(PopupField<int> e)
        {
            e.RegisterValueChangedCallback(evt => { Global.Dirty = true; });
        }
        #endregion
        //===============================================================================================================
        //Block
        #region
        public static VisualElement Block()
        {
            VisualElement block = new VisualElement();
            block.style.flexDirection = FlexDirection.Row;
            block.style.alignItems = Align.Center;
            block.style.width = Length.Percent(100);
            return block;
        }
        #endregion 

        //===============================================================================================================

        public static TextField TextInput(string text = "") {
            TextField textField = new TextField();
            textField.multiline = true;
            textField.value = text; 
            textField.style.whiteSpace = WhiteSpace.Normal;
            textField.style.flexShrink = 0; // 禁止向上缩
            textField.style.maxWidth = 180; 
            textField.style.width = Length.Percent(100);
            textField.style.backgroundColor = rgb(42); 
            SetMargin(textField, 1); 
            SetBorderWidth(textField, 0);
            var input = textField.Q("unity-text-input");
            input.style.backgroundColor = rgb(0, 0);
            input.style.unityTextAlign = TextAnchor.MiddleCenter;
            SetPadding(input, 0);
            SetBorderWidth(input, 0);
            SetBorderRadius(input, 0);

            return textField;
        }

        public static void SetTextFieldFontSize(TextField text,int size) {
            List<VisualElement> eList = text.Query<VisualElement>().ToList();
            eList[2].style.fontSize = size;
        }

        //===============================================================================================================
        public static Label Label(string text = "") {
            var Label = new Label(text);
            Label.style.unityTextAlign = TextAnchor.MiddleCenter;  // 文本居中
            Label.style.justifyContent = Justify.Center;            // 水平居中（外层布局）
            Label.style.alignItems = Align.Center;                  // 垂直居中（外层布局）
            Label.style.width = Length.Percent(100);
            //Label.style.backgroundColor = rgb(95, 167, 249);  // 背景颜色
            return Label;
        }

        //===============================================================================================================
        public static Button Button() {
            Button button = new Button();
            button.style.height = Length.Percent(100);
            button.style.width = Length.Percent(100);
            button.text = "button";
            SetHoverColor(button, rgb(255, 155, 69), rgb(213, 69, 27));
            SetEdge(button);
            SetBorderRadius(button, 0);
            return button;
        }

        public static void SetHoverColor(VisualElement e, Color normal, Color hower) {
            e.style.backgroundColor = normal;
            e.RegisterCallback<MouseEnterEvent>(evt => { e.style.backgroundColor = hower; });
            e.RegisterCallback<MouseLeaveEvent>(evt => { e.style.backgroundColor = normal; });
        }

        public static void SetTransparentHover(VisualElement e, Color broderColor)
        {
            e.style.backgroundColor = rgb(0, 0);
            SetPadding(e, 1);
            SetBorder(e, 0, broderColor);

            e.RegisterCallback<MouseEnterEvent>(evt => {
                SetPadding(e, 0);
                SetBorder(e, 1, broderColor);
                e.style.backgroundColor = rgb(0, 0);
            });
            e.RegisterCallback<MouseLeaveEvent>(evt => {
                SetPadding(e, 1);
                SetBorder(e, 0);
                e.style.backgroundColor = rgb(0, 0);
            });
        }

        //===============================================================================================================
        public static PopupField<T> PopupField<T>(List<T> op) {
            PopupField<T> popupField = new PopupField<T>(default, op, 0);
            SetMargin(popupField, 0);
            popupField.style.width = Length.Percent(100);
            List<VisualElement> eList = popupField.Query<VisualElement>().ToList();
            VisualElement ebutton = eList[1];
            SetBorder(ebutton, 0,Color.black,0);
            ebutton.style.backgroundColor = rgb(0, 0);
            ebutton.style.minWidth = 10;
            SetPadding(ebutton, 0);
            eList[2].style.unityTextAlign = TextAnchor.MiddleCenter;
            eList[3].style.display = DisplayStyle.None;

            return popupField;
        }

        public static DropdownField DropdownField(List<string> op)
        {
            DropdownField popupField = new DropdownField(default, op, 0);
            SetEdge(popupField, 0);
            popupField.style.width = Length.Percent(100);
            List<VisualElement> eList = popupField.Query<VisualElement>().ToList();
            VisualElement ebutton = eList[1];
            SetBorder(ebutton, 0, Color.black, 0);
            ebutton.style.backgroundColor = rgb(0, 0);
            ebutton.style.minWidth = 10;
            SetPadding(ebutton, 0);
            eList[2].style.unityTextAlign = TextAnchor.MiddleCenter;
            eList[3].style.display = DisplayStyle.None;

            return popupField;
        }

        public static T CreateGeneralContainer<T> () where T : StoryContainer, new()
        {
            T ret = new T();
            ret.MainBlock = Block();
            ret.ContentBlock = Block();
            ret.ButtonBlock = Block();
            ret.Button = Button();

            ret.Button.text = "X";
            SetBorderWidth(ret.Button,1);

            ret.MainBlock.style.justifyContent = Justify.Center;
            ret.ContentBlock.style.justifyContent = Justify.Center;

            ret.ButtonBlock.style.width = 20;
            ret.ButtonBlock.style.height = Length.Percent(100);
            ret.ButtonBlock.Add(ret.Button);

            ret.MainBlock.Add(ret.ContentBlock);
            ret.MainBlock.Add(ret.ButtonBlock);
            return ret;
        }

        public static StoryComponent CreateStoryComponent()
        {
            StoryComponent ret = CreateGeneralContainer<StoryComponent>();

            ret.Top = CreateGeneralContainer<StoryContainer>();
            ret.ItemContent = Block();
            ret.ContentBlock.style.flexDirection = FlexDirection.Column;
            ret.ContentBlock.Add(ret.Top.MainBlock);
            ret.ContentBlock.Add(ret.ItemContent);

            ret.Option = PopupField(Global.ComponentType);
            SetHoverColor(ret.Option, rgb(32), rgb(64));
            ret.Top.ContentBlock.Add(ret.Option);

            ret.Top.Button.text = "+";
            SetHoverColor(ret.Top.Button, rgb(65, 109, 25), rgb(155, 207, 83));

            AddBottomBorder(ret.MainBlock); 

            return ret;
        }

        public static StoryComponentItem AddStoryComponentItem(StoryComponent storyComponent) {
            StoryComponentItem item = CreateGeneralContainer<StoryComponentItem>();
            item.ItemID = TextInput();
            item.Name = Label(); 
            item.Count = TextInput();
            item.Weight = TextInput();

            item.Name.style.backgroundColor = rgb(42);

            item.ItemID.multiline = false;
            item.Count.multiline = false;
            item.Weight.multiline = false;

            item.ItemID.style.maxWidth = Length.Percent(25);
            item.Name.style.width = Length.Percent(38);
            item.Count.style.maxWidth = Length.Percent(20);
            item.Weight.style.maxWidth = Length.Percent(15);


            SetTextFieldFontSize(item.ItemID, 7);
            SetTextFieldFontSize(item.Count, 7);
            SetTextFieldFontSize(item.Weight, 7);

            item.ItemID.style.height = 13;
            item.Count.style.height = 13; 
            item.Weight.style.height = 13;
            item.Name.style.height = 15;

            SetBorderWidth(item.ItemID, 1);
            SetBorder(item.Name, 1, rgb(32)); 
            SetBorderWidth(item.Count, 1);
            SetBorderWidth(item.Weight, 1);

            SetPadding(item.Name, 0);
            item.Name.text = "精英进化丧尸";
            item.Name.style.fontSize = 9;

            item.ContentBlock.style.alignItems = Align.FlexEnd;

            item.ContentBlock.Add(item.ItemID);
            item.ContentBlock.Add(item.Count);
            item.ContentBlock.Add(item.Weight);
            item.ContentBlock.Add(item.Name);
            storyComponent.ContentBlock.Add(item.MainBlock);
            return item;
        }
    }
}
