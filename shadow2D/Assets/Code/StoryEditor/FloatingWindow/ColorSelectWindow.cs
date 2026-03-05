using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace StoryEditor
{
    public class ColorSelectWindow : FloatingWindowBase<ColorSelectWindow>
    {
        HashSet<long> colotKey = new HashSet<long>();
        List<Color> colorList = new();

        Action<Color> CallBack = null;

        public ColorSelectWindow() {

            style.minWidth = 0;
            TitleLabel.style.display = DisplayStyle.None;

            InitColor();

            VisualElement contentBlock = UITool.Block();
            contentBlock.style.flexDirection = FlexDirection.Column;
            Add(contentBlock);

            int lenth = (int)Mathf.Sqrt(colorList.Count);
            if (lenth * lenth < colorList.Count) lenth += 1;

            VisualElement Line = new VisualElement();
            for (int i = 0; i < colorList.Count; ++i) {
                if (i % lenth == 0) {
                    Line = UITool.Block();
                    contentBlock.Add(Line);
                }

                Color color = colorList[i];

                VisualElement colorBlock = UITool.Block();
                colorBlock.style.height = 24;
                colorBlock.style.width = 24;
                colorBlock.style.backgroundColor = color;

                UITool.SetBorder(colorBlock,1,Color.black);
                UITool.SetMargin(colorBlock, 2);

                colorBlock.RegisterCallback<MouseEnterEvent>(evt => {
                    if (!IsShow) return;
                    UITool.SetSize(colorBlock,26,26);
                    UITool.SetMargin(colorBlock, 1);
                });
                colorBlock.RegisterCallback<MouseLeaveEvent>(evt => {
                    if (!IsShow) return;
                    UITool.SetSize(colorBlock, 24, 24);
                    UITool.SetMargin(colorBlock, 2);
                });

                colorBlock.RegisterCallback<PointerDownEvent>(evt => {
                    if (!IsShow) return;
                    SelectColor(color);
                });

                Line.Add(colorBlock);
            }

            parent.RegisterCallback<PointerDownEvent>(evt => {
                if (!IsShow) return;
                CallBack = null;
                ShowWindow(false);
            });
        }



        public void Show(PointerDownEvent evt, Action<Color> callback) {
            ShowWindow(true);
            CallBack = callback;

            Vector2 pos = new Vector2(evt.position.x, evt.position.y);
            schedule.Execute(() =>{
                pos.x -= resolvedStyle.width*0.5f;
                pos.y -= resolvedStyle.height*0.5f;
                SetPosByMousePos(pos);
            }).ExecuteLater(1);
            //SetPosByMousePos(pos);

            BringToFront();
        }

        public void SelectColor(Color color) {
            ShowWindow(false);
            if (CallBack == null)
                return;
            CallBack(color);
            CallBack = null;
        }

        public Color GetColorByIndex(int index) {
            index = index % colorList.Count;
            return colorList[index];
        }

        public void InitColor() {
            rgb(224, 122, 95);
            rgb(83, 59, 77);
            rgb(78, 113, 255);
            rgb(51, 52, 70);
            rgb(167, 101, 69);
            rgb(59, 59, 26);
            rgb(138, 120, 78);
            rgb(27, 77, 62);
            rgb(105, 11, 34);
            rgb(61, 54, 92);
            rgb(124, 69, 133);
            rgb(201, 87, 146);
            rgb(83, 59, 77);
            rgb(89, 65, 0);
            rgb(100, 74, 7);
            rgb(96, 70, 82);
            rgb(26, 26, 25);
        }
        public void rgb(int r, int g, int b)
        {
            long key = MakeKey(r,g,b);
            if (colotKey.Contains(key)){
                Debug.Log("duplicate :" + r +","+ g + "," + b);
                return;
            }
            colotKey.Add(key);
            colorList.Add(UITool.rgb(r, g, b));
        }
        public long MakeKey(int r, int g, int b) {
            return (long)r << 32 | (long)g << 16 | (long)b;
        }
        
    }
}