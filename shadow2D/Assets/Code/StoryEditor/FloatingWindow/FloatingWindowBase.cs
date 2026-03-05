using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using ECS;

namespace StoryEditor
{
    public class FloatingWindowBase<T> : VisualElement where T : new()
    {
        private static T s_singleton = default(T);
        public static T Singleton{
            get{
                if (null == s_singleton)
                    s_singleton = new T();
                return s_singleton;
            }
        }

        protected VisualElement TopBlock;
        protected Label TitleLabel;

        private bool dragging = false;
        private Vector2 pointerOffset = Vector2.zero;
        public FloatingWindowBase() {

            pickingMode = PickingMode.Position;
            style.position = Position.Absolute;
            style.left = 800;
            style.top = 50;
            style.minWidth = 180;
            style.backgroundColor = UITool.rgb(32, 32, 32);
            UITool.SetBorderColor(this,UITool.rgb(0,0,0));
            UITool.SetBorderWidth(this,1);
            UITool.SetBorderRadius(this, 4);

            TopBlock = UITool.Block();
            TopBlock.style.backgroundColor = UITool.rgb(10, 10, 10);
            Add(TopBlock);

            TitleLabel = UITool.Label();
            TitleLabel.style.backgroundColor = UITool.rgb(0, 0);
            TitleLabel.style.unityTextAlign = TextAnchor.MiddleLeft;
            TitleLabel.style.height = 24;
            TopBlock.Add(TitleLabel);

            TitleLabel.RegisterCallback<PointerDownEvent>((evt => { OnStartMove(evt); }));
            TitleLabel.RegisterCallback<PointerMoveEvent>((evt => { OnMove(evt); }));
            TitleLabel.RegisterCallback<PointerUpEvent>((evt => { OnMoveEnd(evt); }));

            StoryGraphView.Singleton.Add(this);

            StoryGraphView.Singleton.RegisterCallback<GeometryChangedEvent>(evt => {
                schedule.Execute(() => { 
                    SetClampPos(style.left.value.value, style.top.value.value);
                }).ExecuteLater(10); 
            });
        }

        private void OnStartMove(PointerDownEvent evt) {

            evt.PreventDefault();
            evt.StopImmediatePropagation();

            TitleLabel.CaptureMouse();
            dragging = true;
            pointerOffset = evt.localPosition;
        }

        private void OnMove(PointerMoveEvent evt) {

            if (!dragging || !TitleLabel.HasMouseCapture()) return;

            evt.PreventDefault();
            evt.StopImmediatePropagation();
            var parentPos = parent.WorldToLocal(evt.position);
            style.left = parentPos.x - pointerOffset.x;
            style.top = parentPos.y - pointerOffset.y; 

            SetClampPos(parentPos.x - pointerOffset.x, parentPos.y - pointerOffset.y);
        }

        private void OnMoveEnd(PointerUpEvent evt) {
            if (!dragging) return;
            dragging = false;

            if (TitleLabel.HasMouseCapture())
                TitleLabel.ReleaseMouse();

            evt.PreventDefault();
            evt.StopImmediatePropagation();
        }

        public void SetPosByMousePos(Vector2 pos) {
            var localPos = parent.WorldToLocal(pos);
            SetClampPos(localPos.x,localPos.y);
        }

        public void SetClampPos(float x, float y) {
            ClampPos(ref x, ref y);
            SetPos(x,y);
        }

        public void SetPos(float x,float y) {
            style.left = x;
            style.top = y;
        }

        private void ClampPos(ref float x,ref float y){
            var window = StoryEditorMainWindow.Singleton;
            if (window == null) return;

            // 当前窗口可用区域
            Vector2 winSize = window.position.size;

            // 获取浮窗自身尺寸
            float fw = resolvedStyle.width;
            float fh = resolvedStyle.height;

            // 计算最大允许的位置
            float maxX = Mathf.Max(0, winSize.x - fw);
            float maxY = Mathf.Max(0, winSize.y - fh);

            //DebugUtils.Log("pos:[{},{}] wSize[{},{}] fSize[{},{}] MaxPos[{},{}]",x,y,winSize.x,winSize.y,fw,fh,maxX,maxY);


            // 钳制
            x = Mathf.Clamp(x, 0, maxX);
            y = Mathf.Clamp(y,20, maxY);
        }


        protected bool IsShow = false;
        public void ShowWindow(bool isShow)
        {
            IsShow = isShow;
            style.display = isShow ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}