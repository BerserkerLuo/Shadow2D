using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System.Collections.Generic;
using StoryEditor;
using System.IO;
using System.Linq;

public class StoryGraphView : GraphView
{
    public static StoryGraphView Singleton;

    public Dictionary<int, StoryNodeView> NodeMap = new();
    Dictionary<int, Edge> EdgeMap = new();
     
    public StoryGraphView() 
    {
        Singleton = this;

        this.style.flexGrow = 1; 

        Insert(0, new GridBackground());

        SetupZoom(0.05f, 2);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //背景网格
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Code/StoryEditor/Uss/GridBackGround.uss");
        styleSheets.Add(styleSheet);
         
        //绑定连接事件
        this.graphViewChanged += OnGraphViewChanged;

        //绑定剪切板
        this.RegisterCallback<KeyDownEvent>(Clipboard.Singleton.OnKeyDown);

        schedule.Execute(()=> {
            OnNewStory();
        }).ExecuteLater(10);
       
    }

    public void OnNewStory() { 
        RemoveAll(); 

        //初始化浮窗
        ColorSelectWindow.Singleton.ShowWindow(false);
        EventWindow.Singleton.AddNewEvent();
        EventWindow.Singleton.SetClampPos(1100, 0);

        AddElement(OnAddNode());
    }

    public Vector2 GetViewOffset() {
        return viewTransform.position;
    }

    //===============================================================================================================================================================
    //搜索可连接的节点
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter adapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) => {
            if (port != startPort && port.node != startPort.node)
                compatiblePorts.Add(port);
        });
        return compatiblePorts;
    }

    //===============================================================================================================================================================
    //图表变化
    private GraphViewChange OnGraphViewChanged(GraphViewChange change)
    {
        if (change.edgesToCreate != null)
            OnEdgeCreate(change.edgesToCreate);

        if (change.elementsToRemove != null){
            foreach (var elem in change.elementsToRemove){
                if (elem is Edge edge) OnEdgeRemove((Edge)elem);
                if (elem is StoryNodeView node) OnNodeRemove((StoryNodeView)elem);
            }
        } 
        return change;
    }

    private void OnEdgeCreate(List<Edge> edgeList) {
        foreach (var edge in edgeList)
            OnAddEdge(edge);
    }

    private void OnAddEdge(Edge edge) {
        int Index = MakeEdgeIndex(edge);
        if (EdgeMap.ContainsKey(Index))
        {
            Debug.Log("OnAddEdge Duplicate Index " + Index);
            return;
        }
        Debug.Log("OnAddEdge Id " + Index);
        EdgeMap.Add(Index, edge);

        StoryNodeView outputNode = edge.output?.node as StoryNodeView;
        if(outputNode != null) outputNode.OnGotoChange(1);

        StoryNodeView inputNode = edge.input?.node as StoryNodeView;
        if (outputNode != null) inputNode.OnEventIdChange(outputNode.EventId);

        Global.Dirty = true;
    }
     
    private int MakeEdgeIndex(Edge edge) {
        StoryNodeView outputNode = edge.output?.node as StoryNodeView;
        if (outputNode == null)
            return 0;
        StoryNodeView inputNode = edge.input?.node as StoryNodeView;
        if (inputNode == null)
            return 0;
        return MakeEdgeIndex(outputNode.GUID, inputNode.GUID);
    }
    private int MakeEdgeIndex(int l,int r) {
        if (l < r) {
            int t = l;
            l = r;
            r = t;
        }
        return l << 16 | r;
    }

    private void OnEdgeRemove(Edge edge) {
        int index = MakeEdgeIndex(edge);
        EdgeMap.Remove(index);
        RemoveElement(edge);

        StoryNodeView outputNode = edge.output?.node as StoryNodeView;
        if (outputNode != null) outputNode.OnGotoChange(-1);

        Debug.Log("OnEdgeRemove Id " + index);
    }
    private void OnNodeRemove(StoryNodeView node) {
        List<Edge> edgeList = new();
        edgeList.AddRange(node.inputPort.connections);
        edgeList.AddRange(node.outputPort.connections);

        foreach (Edge edge in edgeList)
            OnEdgeRemove(edge);
        NodeMap.Remove(node.GUID);

        Debug.Log("OnNodeRemove Id " + node.GUID);
    }

    private void RemoveAll() {

        List<Edge> allEdge = new List<Edge>();
        allEdge.AddRange(EdgeMap.Values);

        List<StoryNodeView> allNode = new List<StoryNodeView>();
        allNode.AddRange(NodeMap.Values);

        foreach (var it in allEdge)
        {
            Debug.Log("RemoveAll edge Id :" + MakeEdgeIndex(it));
            RemoveElement(it);
        }

        foreach (var it in allNode)
        {
            Debug.Log("RemoveAll Node Id :" + it.GUID);
            RemoveElement(it);
        }

        EdgeMap.Clear();
        NodeMap.Clear();

        StoryNodeView.MakeGuId = 0;

        EventWindow.Singleton.ClearAll();

        loopCount = 0;
        RetSetPos();
    }

    //===============================================================================================================================================================
    private int loopCount = 0;
    private Vector2 _createPos = new Vector2(50,50);
    public void RetSetPos() {
        _createPos = new Vector2(50 + loopCount * 10, 50);
    }
    public Vector2 GetCretePos() {
         
        _createPos.x += 10 * viewTransform.scale.x;
        _createPos.y += 10 * viewTransform.scale.x;

        var window = StoryEditorMainWindow.Singleton;
        if (window == null)
            return _createPos;
        
        Vector2 winSize = window.position.size;
        Vector2 stepPos = winSize - _createPos;
        if (stepPos.x > 50 && stepPos.y > 50)
            return _createPos;

        loopCount += 1;
        RetSetPos();
        if (winSize.x - _createPos.x < 50)
            RetSetPos();

        return _createPos;
    }
    public StoryNodeView OnAddNode()
    {
        Debug.Log("OnAddNode");

        Vector2 createPos = (GetCretePos() - GetViewOffset()) / viewTransform.scale.x;


        var node = CreateNode();
       
        node.SetPosition(new Rect(createPos.x, createPos.y, 0, 0));
        NodeMap.Add(node.GUID, node);
        return node;
    }

    public void FoldAll(bool flag) {
        foreach (StoryNodeView node in NodeMap.Values)
            node.OnFold(flag ? 1 : 2);
    }

    //===============================================================================================================================================================
    //创建节点
    public StoryNodeView CreateNode()
    {
        Debug.Log("CreateNode");

        var node = new StoryNodeView();
        AddElement(node);
       
        return node;
    }


    //===============================================================================================================================================================

    //导出Json
    public void ExportJson(string path) {

        Debug.Log("ExportJson");

        StoryInfo storyInfo = new StoryInfo();

        storyInfo.EventList = EventWindow.Singleton.GetEventInfoList();
        foreach (StoryNodeView node in NodeMap.Values)
            storyInfo.NodeList.Add(node.GetStoryNodeInfo());

        ExcuteStory(storyInfo);

        string json = JsonUtility.ToJson(storyInfo, true);
        File.WriteAllText(path, json);

        Debug.Log("JsonData:\n" + json);
    }

    public void ExcuteStory(StoryInfo storyInfo) {
        Dictionary<int,Dictionary<int, int>> nodeFatherMap = new();
        foreach (var info in storyInfo.NodeList) {
            Dictionary<int, int> tempMap = nodeFatherMap.GetValueOrDefault(info.eventId,null);
            if (tempMap == null) {
                tempMap = new();
                nodeFatherMap.Add(info.eventId, tempMap);
            }

            foreach (int sonId in info.GoList) {
                if (tempMap.ContainsKey(sonId)) continue;
                tempMap.Add(sonId, info.Id);
            }
        }

        foreach (var eventInfo in storyInfo.EventList) {
            Dictionary<int, int> tempMap = nodeFatherMap.GetValueOrDefault(eventInfo.Id, null);
            if (tempMap == null) continue;
            int fatherId = tempMap.First().Value;
            int nextId = 0;
            for (int i = 0; i < tempMap.Count; ++i) {
                nextId = tempMap.GetValueOrDefault(fatherId, -1);
                if (nextId == -1)
                    break;
                fatherId = nextId;
            }
            eventInfo.StartId = fatherId;
        }
    }

    //加载Json
    public void LoadJson(string path)
    {
        Debug.Log("LoadJson");

        string json = File.ReadAllText(path);
        StoryInfo storyInfo = JsonUtility.FromJson<StoryInfo>(json);

        Debug.Log("JsonData:\n" + json);
         
        RemoveAll();

        EventWindow.Singleton.SetEventList(storyInfo.EventList);

        foreach (var it in storyInfo.NodeList) {
            StoryNodeView node = CreateNode();
            node.GUID = it.Id;
            node.SetStoryNodeInfo(it);
            node.OnFold(1);
            NodeMap.Add(node.GUID,node);
        }

        CreateEdge(storyInfo.NodeList, NodeMap);
    }

    public void CreateEdge(List<StoryNodeInfo> nodeList, Dictionary<int, StoryNodeView> nodeMap)
    {
        foreach (var it in nodeList)
        {
            var mainNode = nodeMap.GetValueOrDefault(it.Id);
            if (mainNode == null)
                continue;

            List<int> gotoList = it.GoList;
            foreach (int id in gotoList)
            {
                var nextNode = nodeMap.GetValueOrDefault(id);
                if (nextNode == null)
                    continue;
                ConnectNode(mainNode, nextNode);
            }

            mainNode.OnGotoChange();
        }
    }

    public void ConnectNode(StoryNodeView mainNode, StoryNodeView nextNode)
    {
        Edge edge = mainNode.outputPort.ConnectTo(nextNode.inputPort);
        AddElement(edge);
        OnAddEdge(edge);
    }

}


