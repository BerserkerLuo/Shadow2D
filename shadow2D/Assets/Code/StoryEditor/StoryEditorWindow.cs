using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using StoryEditor;
using Table;

public class StoryEditorMainWindow : EditorWindow
{ 
    private static StoryEditorMainWindow _singleton;
    public static StoryEditorMainWindow Singleton { 
        get {
            if (_singleton == null)
                _singleton = GetWindow<StoryEditorMainWindow>();
            return _singleton;
        } 
    }

    private string LastFilePath {
        set { PlayerPrefs.SetString("LastFilePath", value); }
        get { return PlayerPrefs.GetString("LastFilePath",""); }
    }

    private StoryGraphView _graphView;
    private Toolbar toolbar;
    private Label FileNameLabel;

    [MenuItem("Window/StoryEditor")]
    public static void Open() 
    {
        var window = GetWindow<StoryEditorMainWindow>();
        window.titleContent = new GUIContent("StoryEditor");
    }

    private void OnEnable()
    {
        _singleton = this;

        ConstructGraphView();
        GenerateToolbar();

        TableMgr.Singleton.LoadCfg();
         
        _graphView.schedule.Execute(()=> {
            Load(LastFilePath);
        }).ExecuteLater(20);
    }
     
    private void OnDisable()  
    {
        //if (Global.Dirty && !string.IsNullOrEmpty(Global.FilePath)) 
        //    Save();

        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new StoryGraphView{ name = "StoryGraph" };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);

        _graphView.RegisterCallback<KeyDownEvent>(OnKeyDown);

        _graphView.schedule
            .Execute(CheckDirty)
            .Every(500);  // 时间单位是毫秒
    }

    private void GenerateToolbar()
    {
        FileNameLabel = UITool.Label();
        FileNameLabel.style.unityTextAlign = TextAnchor.MiddleLeft;  // 文本居中
        FileNameLabel.text = "NewStory";

        var toolbar = new Toolbar();
        toolbar.Add(new Button(() => NewStory()) { text = "NewStory" });
        toolbar.Add(new Button(() => OnAddNode()) { text = "AddNode" });
        toolbar.Add(new Button(() => FoldAll()) { text = "FoldAll" });
        toolbar.Add(new Button(() => Export()) { text = "ExportJson" });
        toolbar.Add(new Button(() => Load()) { text = "LoadJson" });
        toolbar.Add(FileNameLabel);
        rootVisualElement.Add(toolbar);

      
    }

    private void CheckDirty() {
        FileNameLabel.style.color = Global.Dirty ? Color.red : Color.green;
    }

    private void OnKeyDown(KeyDownEvent evt)
    {
        if (evt.ctrlKey && evt.keyCode == KeyCode.S) { Save(); }
        if (evt.ctrlKey && evt.keyCode == KeyCode.A) { OnAddNode();  }
    }
      
    private void NewStory() {
        if (Global.Dirty) {
            bool choice = EditorUtility.DisplayDialog("警告", "当前剧情有修改未保存!是否继续?", "Yes", "No");
            if (!choice) return;
        }

        Global.Dirty = false;
        flodFlag = true;

        FileNameLabel.text = "NewStory";
        Global.FilePath = "";
        _graphView.OnNewStory();
    }

    private void OnAddNode() {
        Global.Dirty = true;
        _graphView.OnAddNode();
    }

    bool flodFlag = true;
    private void FoldAll() {
        flodFlag = !flodFlag;
        _graphView.FoldAll(flodFlag);
    }

    private void Save() {
        if (string.IsNullOrEmpty(Global.FilePath))
            Export();
        else
            Save(Global.FilePath);
    }

    private void Export() {
        string defaultName = "story";              // 默认文件名，不带扩展名
        string path = EditorUtility.SaveFilePanel(
            title: "导出剧情",
            directory: "Assets/StreamingAssets/Story",
            defaultName: defaultName,
            extension: "json"
        );

        if (!string.IsNullOrEmpty(path)){
            // path 里包含完整目录和文件名：比如 "C:/MyProject/Assets/ExportData/story.json"
            Save(path,true);
        }
    }

    private void Save(string path,bool enforce = false) {
        if (!Global.Dirty && !enforce)
            return;

        SetFileName(path);
        Global.FilePath = path;
        _graphView.ExportJson(path);
        Global.Dirty = false;

        LastFilePath = path;
    }

    private void Load() {

        if (Global.Dirty){
            bool choice = EditorUtility.DisplayDialog("警告", "当前剧情有修改未保存!是否继续?", "Yes", "No");
            if (!choice) return;
        }

        string path = EditorUtility.OpenFilePanel(
          title: "加载剧情",
          directory: "Assets/StreamingAssets/Story",
          extension: "json"
        );

        Load(path);
    }

    private void Load(string path) {

        if (string.IsNullOrEmpty(path))
            return;
        
        Global.FilePath = path;
        SetFileName(path);
        _graphView.LoadJson(path);

        _graphView.schedule.Execute(() => {
            Global.Dirty = false;
            CheckDirty();
        }).ExecuteLater(5); 

        flodFlag = true;
        Debug.Log("已加载：" + path);

        LastFilePath = path;
    }

    private void SetFileName(string path) {
        string[] list = path.Split("/");
        FileNameLabel.text = list[list.Length - 1];
    }
}

