using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace StoryEditor
{
    public class Clipboard : SingletonBase<Clipboard>
    {
        public List<StoryNodeView> copyList = new List<StoryNodeView>();

        public Dictionary<int,StoryNodeView> CopyRelationMap = new();

        public void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.ctrlKey && evt.keyCode == KeyCode.C) { CopySelectedNodes(); }
            if (evt.ctrlKey && evt.keyCode == KeyCode.V) { PasteSelectedNodes(); }
        }

        public void CopySelectedNodes() {
            copyList.Clear();
            copyList.AddRange(StoryGraphView.Singleton.selection.OfType<StoryNodeView>().ToList());
        }

        public void PasteSelectedNodes() {
            if (copyList.Count == 0) return;
            CopyRelationMap.Clear();

            StoryGraphView.Singleton.ClearSelection();

            List<StoryNodeInfo> infoList = new List<StoryNodeInfo>();
            foreach (var it in copyList) {
                StoryNodeInfo info = it.GetStoryNodeInfo();
                StoryNodeView newNode = StoryGraphView.Singleton.CreateNode();
                newNode.SetStoryNodeInfo(info);
                newNode.SetPosition(new Rect(info.pos.x + 10, info.pos.y,0,0));
                newNode.OnFold(it.FoldFlag ? 1:2);
                StoryGraphView.Singleton.NodeMap.Add(newNode.GUID,newNode);

                CopyRelationMap.Add(info.Id, newNode);
                infoList.Add(info);

                StoryGraphView.Singleton.AddToSelection(newNode);

                if (copyList.Count == 1) {
                    StoryGraphView.Singleton.ConnectNode(it, newNode);
                    it.OnGotoChange();

                    float offsetY = it.layout.height + 20;
                    newNode.SetPosition(new Rect(info.pos.x + 10* info.GoList.Count, info.pos.y + offsetY, 0, 0));
                }
            }

            StoryGraphView.Singleton.CreateEdge(infoList, CopyRelationMap);

            CopyRelationMap.Clear();
        }
    }
}