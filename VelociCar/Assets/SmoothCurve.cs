using UnityEngine;

public class SmoothCurve : MonoBehaviour, IUpdateObserver
{
    #region EnablingFunctions
    void OnEnable()
    {
        UpdateManager.RegisterObserver(this);
    }

    void OnDisable()
    {
        UpdateManager.UnregisterObserver(this);
    }
    #endregion EnablingFunctions

    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 startVelocity;

    private float _timer = 0f;
    private float timePosition = 0.1f;

    public GameObject circle;
    void Start()
    {
        Instantiate(circle, startPos, Quaternion.identity).transform.localScale = new Vector3(1,1,1);
        Instantiate(circle, endPos, Quaternion.identity).transform.localScale = new Vector3(1, 1, 1);
    }

    public void ObservedUpdate()
    {
        if(_timer < 1f)
        {
            if(_timer >= timePosition)
            {
                float x = CalculatePosition(startPos.x, endPos.x, startVelocity.x, _timer);
                float y = CalculatePosition(startPos.y, endPos.y, startVelocity.y, _timer);

                Instantiate(circle, new Vector2(x, y), Quaternion.identity);

                timePosition += 0.1f;
            }
            _timer += Time.deltaTime;
        }
    }

    private float CalculatePosition(float x0, float x1, float m, float t)
    {
        float F1 = x1 - x0;

        float c = m;
        float b = F1 - c;

        return b * Mathf.Pow(t, 2f) + c * t + x0;
    }
}
