using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 通用鼠标指针管理器
/// 支持硬件光标 & UI模拟光标
/// </summary>
public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    public bool useUICursor = false;
    Image uiCurser;
    public Image BattleUiCurser;
    public Image NormalCurser;

    private void Awake()
    {
        Instance = this;

        ChangToNormalCurser();

        if (useUICursor && uiCurser != null)
            Cursor.visible = false; // 隐藏系统指针
    }

    private void Update(){
        UpdateUICursor();
        UpdateCursorHide();
    }

    public void UpdateCursorHide() {
        Vector3 mp = Input.mousePosition;
        // 检查是否在窗口内
        if (mp.x < 0 || mp.y < 0 || mp.x > Screen.width || mp.y > Screen.height){
            // 鼠标移出窗口 → 显示系统鼠标，隐藏UI鼠标
            if (useUICursor){
                Cursor.visible = true;
                uiCurser.gameObject.SetActive(false);
                useUICursor = false;
            }
            return;
        }

        // 在窗口内 → 隐藏系统鼠标，显示UI鼠标
        if (!useUICursor){
            Cursor.visible = false;
            uiCurser.gameObject.SetActive(true);
            useUICursor = true;
        }
    }

    public void UpdateUICursor() {
        Vector2 localPos;
        // 把屏幕坐标转换到 UI 坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCurser.transform.parent as RectTransform,
            Input.mousePosition,
            UnityGameEntry.Instance.UICamera,
            out localPos))
        {
            uiCurser.rectTransform.localPosition = localPos;
        }
    }

    public void ChangToNormalCurser() {
        uiCurser = NormalCurser;
        NormalCurser.gameObject.SetActive(true);
        BattleUiCurser.gameObject.SetActive(false);
    }

    public void ChangeToBattleCurser(){
        uiCurser = BattleUiCurser;
        NormalCurser.gameObject.SetActive(false);
        BattleUiCurser.gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏指针
    /// </summary>
    public void HideCursor(){
        if (useUICursor)
            if (uiCurser != null) uiCurser.enabled = false;
        else
            Cursor.visible = false;
    }

    /// <summary>
    /// 显示指针
    /// </summary>
    public void ShowCursor(){
        if (useUICursor)
            if (uiCurser != null) uiCurser.enabled = true;
        else
            Cursor.visible = true;
    }

    /// <summary>
    /// Sprite 转换成独立 Texture2D (从图集裁剪出来)
    /// </summary>
    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width || sprite.rect.height != sprite.texture.height)
        {
            // 从图集中裁切
            var newTex = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            var pixels = sprite.texture.GetPixels(
                (int)sprite.textureRect.x,
                (int)sprite.textureRect.y,
                (int)sprite.textureRect.width,
                (int)sprite.textureRect.height
            );
            newTex.SetPixels(pixels);
            newTex.Apply();
            return newTex;
        }
        else
        {
            // 本来就是独立贴图
            return sprite.texture;
        }
    }
}
