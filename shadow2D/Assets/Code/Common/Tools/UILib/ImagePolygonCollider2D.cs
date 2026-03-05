using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(PolygonCollider2D))]
[AddComponentMenu("UI/ImagePolygonCollider2D")]
public class ImagePolygonCollider2D : Image
{
    PolygonCollider2D _polygonCollider;
    public PolygonCollider2D Polygon
    {
        get
        {
            if (_polygonCollider == null)
                _polygonCollider = GetComponent<PolygonCollider2D>();
            return _polygonCollider;
        }
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector3 point;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, eventCamera, out point);
        return Polygon.OverlapPoint(point);
    }
}
