using Client.UI.UICommon;
using System.Collections;
using System.Collections.Generic;
using UILib;
using UILib.Export;
using UnityEngine;
using Table;
using Tools;

namespace Game
{
    public enum TriggerType{
        Frie = 1,//开火
        Hit = 2,//命中
        Kill = 3,//击杀
        Crit = 4,//暴击
        Reload = 5,//装弹
    }

    public class SkillNode{
        public int skillId1 = 0;
        public int skillId2 = 0;
    }

    public class WeaponSkillList{
        public int castType = 0;    // 0同时触发 1循环触发
        public int typeParam = 0;   //
        public List<int> SkillList = new List<int>();
    }

    public class TriggerData {
        public TriggerType triggerType;
        public int index;
    }

    public class WeaponSetting : MonoBehaviour
    {
        private Dictionary<TriggerType, XUIList> SkillLists = new Dictionary<TriggerType, XUIList>();

        private XUISprite DragSkillIcon;

        private Dictionary<string, XUIObjectBase> m_dicId2UIObject = new Dictionary<string, XUIObjectBase>();

        void Awake()
        {
            WidgetFactory.FindAllUIObjects(transform, null, ref m_dicId2UIObject, this);

            RegistXUIList(TriggerType.Frie, "FireSkillList");
            RegistXUIList(TriggerType.Hit, "HitList");
            RegistXUIList(TriggerType.Kill, "KillList");
            RegistXUIList(TriggerType.Crit, "CritList");
            RegistXUIList(TriggerType.Reload, "ReloadList");

            DragSkillIcon = (XUISprite)GetUIObject("DragSkillIcon");
        }
        public IXUIObject GetUIObject(string strName)
        {
            return m_dicId2UIObject.GetValueOrDefault(strName, null);
        }

        public void RegistXUIList(TriggerType type, string name) {
            XUIList list = (XUIList)GetUIObject(name);
            if (list == null) {
                Debug.Log($"RegistXUIList {name} Error !");
                return;
            }

            SkillLists.Add(type, list);

            for (int index = 0; index < list.Count; ++index) {
                IXUIListItem item = list.GetItemByIndex(index);
                if (item == null)
                    continue;

                XUIButton button = (XUIButton)item.GetUIObject("Button");
                if (button == null)
                    continue;


                RegistButtonEvent(button, type, index);

                SetButtonIconByImg(button, null);
            }

        }

        public void RegistButtonEvent(XUIButton button, TriggerType type, int index)
        {
            TriggerData data = new TriggerData();
            data.triggerType = type;
            data.index = index;
            button.Data = data;

            button.RegisterEnterEventHandler(OnButtonEnter);
            button.RegisterExitEventHandler(OnButtonExit);
            button.RegisterDownEventHandler(OnButtonDown);
            button.RegisterUpEventHandler(OnButtonUp);
        }

        //=========================================================================

        public void SetButtonIconByImg(XUIButton button, Sprite srcImg) {
            XUISprite iconObj = (XUISprite)button.GetUIObject("Icon");
            if (iconObj == null) return;
            iconObj.sprite = srcImg;
            iconObj.SetVisible(iconObj.sprite != null);
        }
        public void SetButtonIcon(XUIButton button, string path) {
            XUISprite iconObj = (XUISprite)button.GetUIObject("Icon");
            if (iconObj == null) return;
            iconObj.SetVisible(path != "");
            iconObj.SetSprite(path);
        }

        public void SetButtonIconBySkillId(XUIButton button, int skillId) {
            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return;
            SetButtonIcon(button, skillCfg.Icon);
        }

        public Sprite GetButtonIcon(XUIButton button) {
            XUISprite iconObj = (XUISprite)button.GetUIObject("Icon");
            if (iconObj == null) return null;
            return iconObj.sprite;
        }

        //=========================================================================
        public void Update()
        {
            UpdateDragSkillIcon();
        }

        bool InDragIcon = false;
        public void UpdateDragSkillIcon() {
            if (!InDragIcon) return;
            Vector2 localPos;
            // 把屏幕坐标转换到 UI 坐标
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                DragSkillIcon.CachedRectTransform.parent as RectTransform,
                Input.mousePosition, UIGlobal.UICamera, out localPos)) {
                DragSkillIcon.CachedRectTransform.localPosition = localPos;
            }
        }
        public void StartDragIcon(XUISprite srcImg) {
            DragSkillIcon.SetVisible(true);
            DragSkillIcon.SetSprite(srcImg.sprite);
            InDragIcon = true;

            UpdateDragSkillIcon();
        }
        public void EndDragIcon() {
            InDragIcon = false;
            DragSkillIcon.SetVisible(false);
        }
        //=========================================================================

        XUIButton SelectButton;
        public bool OnButtonEnter(IXUIObject obj) {
            SelectButton = (XUIButton)obj;
            return true;
        }

        public bool OnButtonExit(IXUIObject obj) {
            SelectButton = null;
            return true;
        }

        public bool HaveSelectButton() {
            return SelectButton != null;
        }

        public TriggerData GetSelectTriggerData() {
            if (SelectButton == null)
                return null;
            return (TriggerData)SelectButton.Data;
        }

        public void SetSelectButtonIcon(Sprite srcImg) {
            SetButtonIconByImg(SelectButton, srcImg);
        }

        //=========================================================================

        XUIButton DragButton;
        public bool OnButtonDown(IXUIObject obj) {
            DragButton = (XUIButton)obj;

            XUISprite iconObj = (XUISprite)DragButton.GetUIObject("Icon");
            if (iconObj == null || !iconObj.IsVisible()) {
                DragButton = null;
                return true;
            }
 
            StartDragIcon(iconObj);
            return true;
        }

        public delegate void ChangeWeaponCallBack(TriggerType sType,int sIndex, TriggerType tType, int tIndex);
        public ChangeWeaponCallBack changeWeaponCallBack;
        public bool OnButtonUp(IXUIObject obj){
            if (DragButton == null)return true;
            EndDragIcon();
            if (SelectButton == null) return true;

            TriggerData sData = (TriggerData)DragButton.Data;
            TriggerData tData = (TriggerData)SelectButton.Data;

            changeWeaponCallBack?.Invoke(sData.triggerType, sData.index, tData.triggerType, tData.index);

            OnChangeIcon();

            return true;
        }

        public void OnChangeIcon() {
            if (SelectButton == null) return;
            if (DragButton == null) return;

            SetButtonIconByImg(DragButton, GetButtonIcon(SelectButton));
            SetButtonIconByImg(SelectButton, DragSkillIcon.sprite);

        }
    }

}


