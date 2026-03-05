using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Internal;
using UnityEngine.Events;

namespace UILib
{
    public class Widgets
    {


    }

    public class XObject : IXUIObject
    {
        public static XObject Error { get { return s_Object; } }
        public PointerEventData PointEventData { get; set; }
        public GameObject CachedGameObject { get { return s_gameObjectError; } }
        public Transform CachedTransform { get { return s_gameObjectError.transform; } }
        public RectTransform CachedRectTransform { get { return s_gameObjectError.GetComponent<RectTransform>(); } }
        public IXUIObject parent { get; set; }
        public object DlgBehaviour { get; set; }
        public CanvasGroup CanvasGroup { get; }
        public Vector2 RealSize { get { return Vector2.zero; } }
        public Vector2 RelativeSize { get { return Vector2.zero; } }
        public Bounds AbsoluteBounds { get { return new Bounds(); } }
        public Bounds RelativeBounds { get { return new Bounds(); } }
        public object ClickCustomData { get; set; }

        public bool IsError
        {
            get { return true; }
        }

        public float Alpha { get; set; }
        public float ClickCd { get; set; }
        public bool IsEnableOpen { get; set; }
        string IXUIObject.Tips { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        GraphicRaycaster IXUIObject.Raycaster => throw new NotImplementedException();

        public IXUIObject GetUIObject(string strPath) { return null; }
        public bool IsVisible() { return true; }
        public void SetVisible(bool bVisible) { }
        public void OnFocus() { }
        public void Highlight(bool bTrue) { }
        public void Init() { }

        public bool IsEnable() { return true; }
        public void SetEnable(bool bEnable) { }
        public void SetGray(bool bGray) { }
        public void RegisterClickEventHandler(UIEvent btnClickHandler) { }
        public void RegisterDownEventHandler(UIEvent btnClickHandler) { }
        public void RegisterUpEventHandler(UIEvent btnClickHandler) { }
        public void RegisterEnterEventHandler(UIEvent btnClickHandler) { }
        public void RegisterExitEventHandler(UIEvent btnClickHandler) { }
        public void RegisterDoubleClickEvent(UIEvent eventHandler, float triggerTime) { }
        public void RegisterPressEvent(UIEvent eventHandler, float triggerTime) { }


        public void RegisterDragEventHandler(UIEvent eventHandler){}
        public void RegisterEndDragEventHandler(UIEvent eventHandler) { }
        public void RegisterBeginDragEventHandler(UIEvent eventHandler) { }
        public void RegisterDropEventHandler(UIEvent eventHandler) { }
        public void SetBeginDragOnDown(bool b) { }
        public void SetAllowDrag(bool b) { }
        public void SetAllowDrop(bool b) { }
        public void SetDragConstraintRect(float xMin, float xMax, float yMin, float yMax) { }
        public void SetDragSensitivity(float f) { }

        public void RegisterShowTipsEvent(UIEvent eventHandler, float tipsTime)
        {
            throw new NotImplementedException();
        }

        public void RegisterHideTipsEvent(UIEvent eventHandler)
        {
            throw new NotImplementedException();
        }

        static XObject()
        {
            GameObject.DontDestroyOnLoad(s_gameObjectError);
        }

        private static XObject s_Object = new XObject();
        private static GameObject s_gameObjectError = new GameObject("UIError");
    }

    public class XLable : XObject, IXUILabel
    {
        public static new XLable Error { get { return s_label; } }
        public float AlphaVar { get; set; }
        public Color Color { get; set; }
        public Text Text { get { return null; } }
        public string GetText() { return string.Empty; }
        public void SetText(string strText) { }
        public void SetColor(string htmlString) { }
        public void StartTimer(int nLeftSeconds, string strDay) { }
        public void StartCounter(long startCount, long speed) { }
        public void TweenValue(float targetValue, float fTime, string strFormat, float delay = 0.0f){ }
        public void TweenValueFrom(float fStart, float targetValue, float fTime, string strFormat, float delay = 0.0f) { }
        public void TypeText(string text, float printDelay = -1, UnityAction finish = null){ }
        public void SetTypeTextSpeed(float printDelay){}

        public int fontSize { get; set; }
        public FontStyle fontStyle { get; set; }
        public TextAnchor alignment { get; set; }
        private static XLable s_label = new XLable();


        public float preferredHeight
        {
            get; set;
        }

        public float preferredWidth
        {
            get; set;
        }
    }

    public class XRichLable : XLable, IXUIRichLabel
    {
        public static new XRichLable Error { get { return s_Richlabel; } }

        private static XRichLable s_Richlabel = new XRichLable();
    }

    public class XButton : XObject, IXUIButton
    {

        public static new XButton Error { get { return s_button; } }
        public void SetCaption(string strText) { }
        public bool IsEnable() { return true; }
        public void SetEnable(bool bEnable) { }
        public void SetGray(bool bGray) { }
        public Image UiSpriteBG { get; set; }
        private static XButton s_button = new XButton();

        public bool SetSprite(Sprite newSprite)
        {
            return true;
        }
        public void SetSprite(string abName, string spriteName) { }
        public void SetSprite(string abName, string spriteName, bool bNativeSize = false, Action finish = null) { }

        public bool SetSpriteMaterial(Material newMaterial)
        {
            return true;
        }

        public void SetSelect(bool flag){}

        //public void RegisterPointerEnterHandler(ButtonEnterEventHandler btnEnterHandler)
        //{

        //}

        //public void RegisterPointerExitHandler(ButtonExitEventHandler btnExitHandler)
        //{

        //}

        //public void OnPointerEnter(PointerEventData eventData)
        //{

        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{

        //}

        public int id { get; set; }
        public long GUID { get; set; }
        public object Data { get; set; }

        public string additionalInfo { get; set; }
    }

    public class XInput : XObject, IXUIInput
    {
        public static new XInput Error { get { return s_input; } }
        public bool IsSelected { get; set; }
        public bool Interactable { get; set; }
        public string GetText() { return string.Empty; }
        public void SetText(string strText) { }
        public void Select() { }
        public void RegisterSubmitEventHandler(UIEvent eventHandler) { }
        public void RegisterOnValueChanged(UIEvent eventHandler) { }
        public void RegisterOnInputSelectEventHandler(UIEvent eventHandler) { }
        public void OnSelect(BaseEventData eventData) {}
        private static XInput s_input = new XInput();
      
    }

    public class XCheckBox : XObject, IXUICheckBox
    {
        public static new XCheckBox Error { get { return s_checkBox; } }
        public bool bChecked { get; set; }
        public object Data { get; set; }
        public void RegisterOnCheckEventHandler(UIEvent eventHandler) { }
        private static XCheckBox s_checkBox = new XCheckBox();
    }

    public class XGroup : XObject, IXUIObject
    {
        public void SetVisible(string strId, bool bVisible) { }
        public void SetLabelText(string strId, string strText) { }
        public static new XGroup Error { get { return s_group; } }
        private static XGroup s_group = new XGroup();
        public int id { get; set; }
    }

    public class XList : XObject, IXUIList
    {
        public static new XList Error { get { return s_list; } }
        public Int32 Count { get { return 0; } }
        public bool EnableMultiSelect { get; set; }
        public Transform ItemsRoot { get { return null; } }
        public void Reposition() { }
        public Vector2 GetNormalizedPosition() { return Vector2.zero; }
        public void SetVerticalLastPostion() { }

        public bool SetVerticalNormalizedPosition(float value) { return false; }
        public void Clear(){ }
        public void Refresh(){ }
        public IXUIListItem GetItemByGUID(ulong ulId) { return null; }
        public IXUIListItem GetItemById(UInt32 unId) { return null; }
        public IXUIListItem GetItemById(UInt32 unId, bool bVisible) { return null; }
        public IXUIListItem GetItemByIndex(Int32 nIndex) { return null; }
        public IXUIListItem GetItemByIndexOrAdd(Int32 nIndex) { return null; }
        public IXUIListItem[] GetAllItems() { return null; }
        public IXUIListItem AddListItem(GameObject obj) { return XListItem.Error; }
        public IXUIListItem AddListItem() { return XListItem.Error; }
        public bool DelItem(IXUIListItem iUIListItem) { return false; }
        public bool DelItemById(UInt32 unId) { return false; }
        public bool DelItemByIndex(Int32 nIndex) { return false; }
        public void Highlight(List<UInt32> listIds){ }
        public void SetSize(float cellWidth, float cellHeight){ }
        public void SetAllItemsVisible(bool isActive){}
        public void MoveToIndex(int index) { }
        public void MoveTo(RectTransform childTrans) { }
        public void MoveToHead() { }
        public void MoveToTail() { }

        void IXUIList.RegisterSelectStateChangeEventHandler(UIEvent btnClickHandler){ }

        public void SetItemVisibleCount(int count){ }

        private static XList s_list = new XList();
    }

    public class XListItem : XObject, IXUIListItem
    {
        public static new XListItem Error { get { return s_listItem; } }
        public UInt32 id { get; set; }
        public UInt64 GUID { get; set; }
        public Int32 Index { get { return 0; } }
        public object Data { get; set; }
        public bool IsSelected { get; set; }
        public Color HighlightColor { get; set; }
        public void SetVisible(string strId, bool bVisible) { }
        public void SetIconSprite(string strSprite) { }
        public void SetSprite(string strId, string strSprite) { }
        public void SetIconSprite(string strSprite, string strAtlas) { }
        public void SetIconTexture(string strTexture) { }
        public void SetColor(Color color) { }
        public void SetColor(string strId, Color color) { }
        public void SetColor(string strId, string htmlString) { }
        public void SetLabelText(string strId, string strText) { }
        public void SetLabelColor(string strId, Color color) { }
        public void SetSpriteColor(string strId, Color color) { }
        public void SetSpriteColor(string strId, string htmlString) { }
        public void SetSprite(string strId, string abName, string resName) { }
        public void SetFillAmount(string strId, float fValue) { }
        public void SetChildGray(string strId, bool bGray) { }
        public void RegisterObjectClickEventHandler(string strId, UIEvent btnClickHandler) { }
        public void SetEnable(bool bEnable) { }
        public void SetEnableSelect(bool bEnable) { }
        public void SetSize(float cellWidth, float cellHeight) { }
        public void Clear() { }
        public Dictionary<string, XUIObjectBase> GetAllXUIObj() { return s_dicAllXObj; }
        static private Dictionary<string, XUIObjectBase> s_dicAllXObj = new Dictionary<string,XUIObjectBase>();
        private static XListItem s_listItem = new XListItem();
    }

    public class XLoopList : XObject, IXUILoopList
    {
        public int TotalCount { get { return 0; } }
        public Transform ItemsRoot { get { return null; } }
        public Vector2 NormalizedPosition { get { return Vector2.zero; }set { } }

        public int StartIndex { get { return 0; } }

        public int EndIndex { get { return 0; } }

        public void Init(int dataCount, UIEvent updateCellCB, float cellPadding) { }
        public void AddListItem(int nCount = 1) { }
        public void Reposition(int index = 0) { }
        public void ResetSize(int dataCount) { }
        public void RegisterScrollValueChange(UIEvent uiEvent) { }
        public void RegisterScrollDragEnd(UIEvent uiEvent) { }
        public void MoveToIndex(int index) { }
        public void UpdateList(int index = -1) { }
        public float GetDragBottomPixel() { return 0; }
        public void StopMovement() { }
    }

    public class XPicture : XObject, IXUIPicture
    {
        public static new XPicture Error { get { return s_picture; } }
        public Rect uvRect { get; set; }
        public Color Color { get; set; }
        public RawImage CachedImage { get; }
        public Texture MainTexture { get; set; }
        public void SetTexture(string strAbName, string strTextureFile, bool bNativeSize = false, Action finish = null) { }
        public void SetTexture(Texture tex) { }
        private static XPicture s_picture = new XPicture();
        public int CalculateOpaquePixel() { return 0; }
        public int ModifyPixels(Vector3 screenPos, int nBurshSize) { return 0; }
    }

    public class XPopupList : XObject, IXUIPopupList
    {
        public static new XPopupList Error { get { return s_popupList; } }
        public int SelectedIndex { get; set; }
        public string Selection { get; set; }

        public bool AddItem(string strItem) { return false; }
        public bool AddItem(string strItem, object data) { return false; }
        public void Clear() { }
        public object GetDataByIndex(int index) { return null; }
        public void RegisterPopupListSelectEventHandler(UIEvent eventHandler) { }
        private static XPopupList s_popupList = new XPopupList();
    }

    public class XProgress : XObject, IXUIProgress
    {
        public static new XProgress Error { get { return s_progress; } }
        public float MaxValue { get; set; }
        public float Value { get; set; }
        public Color Color { get; set; }
        public void TweenValue(float targetValue, float fTime, float fDelay = 0.0f){ }
        private static XProgress s_progress = new XProgress();
    }

    public class XScrollBar : XObject, IXUIScrollBar
    {
        static public new XScrollBar Error { get { return s_scrollBar; } }
        public float Value { get; set; }
        public float Size { get; set; }
        public void RegisterScrollBarChangeEventHandler(UIEvent eventHandler) { }
        static private XScrollBar s_scrollBar = new XScrollBar();
    }

    public class XSlider : XObject, IXUISlider
    {
        static public new XSlider Error { get { return s_slider; } }
        public float Value { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public bool Interactable { get; set; }
        public void RegisterValueChangeEventHandler(UIEvent eventHandler) { }
        static private XSlider s_slider = new XSlider();
    }

    public class XSprite : XObject, IXUISprite
    {
        static public new XSprite Error { get { return s_sprite; } }
        // Properties
        public Image CachedImage { get; }
        public int Id { get; set; }
        public object Data { get; set; }
        public Vector2 InitPos { get; }
        public Color Color { get; set; }
        public string spriteName { get; set; }
        public Sprite sprite { set { } }
        public Image image { get; set; }
        // Methods
        public void SetColor(string htmlString) { }
        public bool SetSprite(Sprite newSprite) { return false; }
        public void SetSprite(string path, string spriteName) { }
        public void SetSprite(string path, string spriteName, bool bNativeSize = false, Action finish = null){  }
        public void SetFillAmount(float amount) { }
        public bool SetNativeSize() { return false; }
        public void SetSize(Vector2 size) { }


        static private XSprite s_sprite = new XSprite();
    }

    public class XTextList : XObject, IXUITextList
    {
        static public new XTextList Error
        {
            get { return s_textlist; }
        }
        public int OffsetLine { get; set; }
        public int TotalLine { get { return 0; } }
        public int MaxShowLine { get { return 0; } }
        public void Clear() { }
        public void Add(string text) { }
        static private XTextList s_textlist = new XTextList();
    }

}
