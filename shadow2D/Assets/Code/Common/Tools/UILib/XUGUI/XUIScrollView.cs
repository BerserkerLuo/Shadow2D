/// <summary>
/// XUI button.
/// Copyright © 2012-2014 lybns
/// Any bugs/comments/suggestions, please contact to xuguangxiao@gmail.com
/// </summary>
using Client.UI.UICommon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using ProjectX;

namespace UILib
{
    public class XUIScrollView : XUIObject,IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform scrollView;

        float startValue;
        float startPointValue;

        float targetValue;
        bool IsOk = true;

        public override void Init() {
            base.Init();
            if (scrollView != null) {
                scrollView.anchorMin = new Vector2(0, 1);
                scrollView.anchorMax = new Vector2(0, 1);
            }
        }

        void SetTargetValue(float value) {
            targetValue = value;
            IsOk = false;
        }
        private void Update()
        {
            if (IsOk) return;

            Vector2 pos = scrollView.anchoredPosition;
            pos.y = Mathf.Lerp(pos.y, targetValue, 0.1f);
            scrollView.anchoredPosition = pos;

            if (Mathf.Abs(pos.y - targetValue) < 0.001f)
                IsOk = true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsOk = true;
            startValue = scrollView.anchoredPosition.y;
            startPointValue = eventData.position.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float stepValue = startPointValue - eventData.position.y;
            Vector2 pos = scrollView.anchoredPosition;
            pos.y = startValue - stepValue;
            scrollView.anchoredPosition = pos; 
        }
         
        public void OnEndDrag(PointerEventData eventData) {
            Vector2 pos = scrollView.anchoredPosition;
            if (pos.y < 0){
                SetTargetValue(0);
                return;
            }

            float height = CachedRectTransform.rect.height;
            float viewHeight = CalcViewHeight();
            if (pos.y > viewHeight - height) 
                SetTargetValue(viewHeight - height);
        }

        public XUIList uiList = null;

        int _OldRowCount = 0;
        float heigt = 0;
        private float CalcViewHeight() {
            if (uiList == null)
                return scrollView.rect.height;

            if(heigt == 0)
                heigt = scrollView.rect.height;

            int rowCount = uiList.GetRowCount();
            if (rowCount == _OldRowCount)
                return heigt;

            _OldRowCount = rowCount;

            float cellHeight = uiList.GetCellSize().y;
            float cellPaddingY = uiList.GetCellPadding().y;

            heigt = cellHeight * rowCount + cellPaddingY * (rowCount - 1);
            if(heigt < scrollView.rect.height)
                heigt = scrollView.rect.height;

            return heigt;
        }

        
    }
}

