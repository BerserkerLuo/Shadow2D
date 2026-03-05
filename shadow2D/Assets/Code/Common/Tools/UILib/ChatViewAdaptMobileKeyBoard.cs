using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 移动设备输入框的自适应组件
/// </summary>
public class ChatViewAdaptMobileKeyBoard : MonoBehaviour
{
    public InputField _inputField;

    /// <summary>
    /// 自适应（弹出输入框后整体抬高）的面板的初始位置
    /// </summary>
    public Vector2 _adaptPanelOriginPos;
    public RectTransform _adaptPanelRt;
    private float RESOULUTION_HEIGHT = 1280F;

    //public static ChatViewAdaptMobileKeyBoard Create(GameObject attachRoot, InputField inputField)
    //{
    //    ChatViewAdaptMobileKeyBoard instance = null;
    //    instance = attachRoot.AddComponent<ChatViewAdaptMobileKeyBoard>();
    //    instance._inputField = inputField;
    //    return instance;
    //}
    private void Awake()
    {
        _inputField = GetComponent<InputField>();
    }

    private void Start()
    {
        Debug.Log("ChatViewAdaptMobileKeyBoard.start()");
        _inputField.onEndEdit.AddListener(OnEndEdit);
        _inputField.onValueChanged.AddListener(OnValueChanged);
        _adaptPanelOriginPos = _adaptPanelRt.anchoredPosition;
        //_adaptPanelRt = transform.Find("TabControl/Panels").GetComponent<RectTransform>();
        //_adaptPanelOriginPos = _adaptPanelRt.anchoredPosition;
    }

    private void LateUpdate()
    {
        if (_inputField.isFocused)
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                float keyboardHeight = AndroidGetKeyboardHeight() * RESOULUTION_HEIGHT / Screen.height;
                Debug.LogFormat("安卓平台检测到InputField.isFocused为真，获取键盘高度：{0}, Screen.height：{1}", keyboardHeight, Screen.height);
                _adaptPanelRt.anchoredPosition = Vector3.up * (keyboardHeight);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                float keyboardHeight = IOSGetKeyboardHeight() * RESOULUTION_HEIGHT / Screen.height;
                Debug.LogFormat("IOS平台检测到键盘高度：{0},Screen.height: {1}", keyboardHeight, Screen.height);
                _adaptPanelRt.anchoredPosition = Vector3.up * keyboardHeight;
            }
            else
            {
                //Editor或其他平台，测试用！
                _adaptPanelRt.anchoredPosition = Vector3.up * 300f;
            }
        }
        else
        {
            _adaptPanelRt.anchoredPosition = _adaptPanelOriginPos;
        }
    }

    private void OnValueChanged(string arg0) { }


    /// <summary>
    /// 结束编辑事件，TouchScreenKeyboard.isFocused为false的时候
    /// </summary>
    /// <param name="currentInputString"></param>
    private void OnEndEdit(string currentInputString)
    {
        //Debuger.LogFormat("OnEndEdit, 输入内容：{0}, 结束时间：{1}", currentInputString, Time.realtimeSinceStartup);
        //_adaptPanelRt.anchoredPosition = _adaptPanelOriginPos;
    }

    /// <summary>
    /// 获取安卓平台上键盘的高度
    /// </summary>
    /// <returns></returns>
    public int AndroidGetKeyboardHeight()
    {
#if UNITY_ANDROID
        using (AndroidJavaClass UnityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject View = UnityClass.GetStatic<AndroidJavaObject>("currentActivity").
                Get<AndroidJavaObject>("mUnityPlayer").Call<AndroidJavaObject>("getView");

            using (AndroidJavaObject Rct = new AndroidJavaObject("android.graphics.Rect"))
            {
                View.Call("getWindowVisibleDisplayFrame", Rct);
                return Screen.height - Rct.Call<int>("height");
            }
        }
#else
        return 0;
#endif
    }


    public float IOSGetKeyboardHeight()
    {
#if UNITY_IPHONE && !UNITY_EDITOR
        return TouchScreenKeyboard.area.height;
#else
        return 0;
#endif
    }
}
