//#define DEBUG_LOG
using System;
using System.Collections.Generic;
using Client.UI;
using Client.UI.UICommon;
using UILib.Export;
using UnityEngine;
using UnityEngine.UI;

namespace UILib
{

    public class UIManager : SingletonBase<UIManager>
    {

        public bool AddDlg(IXUIDlg dlg){
            if (true == m_dicDlgs.ContainsKey(dlg.fileName)){
                Debug.LogError("true == m_dicDlgs.ContainsKey(dlg.fileName): " + dlg.fileName);
                return false;
            }

            m_dicDlgs.Add(dlg.fileName, dlg);
            m_listAllDlg.Add(dlg);

            return true;
        }

        public void OnDlgVisible(IXUIDlg uiDlg, bool bVisible) { }
        public void Compositor(IXUIDlg uiDlg) { }
        public virtual void OnDlgHide(IXUIDlg uiDlg) { }
        public void OnDlgShow(IXUIDlg uiDlg){ }

        public virtual void OnDlgUptate()
        {
            for (int i = 0; i < m_listAllDlg.Count; ++i)//待完善
            {
                IXUIDlg dlg = m_listAllDlg[i];
                dlg._Update();
            }
        }

        public void ShowTips(string str) {
            //DlgTips.singleton.ShowTips(str);
        }

        public void HideTips(){
            //DlgTips.singleton.SetVisible(false);
        }

        private List<IXUIDlg> m_listAllDlg = new List<IXUIDlg>();
        private Dictionary<string, IXUIDlg> m_dicDlgs = new Dictionary<string, IXUIDlg>();
    }
}
