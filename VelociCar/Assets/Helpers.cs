using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null)
                _camera = Camera.main;
            return _camera;
        }
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait))
            return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition};
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject); 
    }

    public static float CompareDirection2D(Vector2 a, Vector2 b)
    {
        if (a == Vector2.zero || b == Vector2.zero)
            return 0f;

        Vector2 normA = a.normalized;
        Vector2 normB = b.normalized;

        float dot = Vector2.Dot(normA, normB);
        dot = Mathf.Clamp(dot, -1f, 1f);

        float similarity = (dot + 1f) * 0.5f;

        return similarity;
    }

    public static float CompareDirection(Vector3 a, Vector3 b)
    {
        if (a == Vector3.zero || b == Vector3.zero)
            return 0f;

        Vector3 normA = a.normalized;
        Vector3 normB = b.normalized;

        float dot = Vector3.Dot(normA, normB);
        dot = Mathf.Clamp(dot, -1f, 1f);

        float similarity = (dot + 1f) * 0.5f;

        return similarity;
    }
}
