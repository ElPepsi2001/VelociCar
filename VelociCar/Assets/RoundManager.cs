using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour, IUpdateObserver
{
    #region Singleton
    public static RoundManager instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("More than one RoundManager!");
            Destroy(gameObject);
        }
    }
    #endregion Singleton
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
    public List<PlayerController> players = new List<PlayerController>();
    public List<GameObject> playerObjects = new List<GameObject>();
    private float[] lastTurnSpeed;
    int currentIndex = 0;
    float _timer = 0f;

    public float turnAcceleration;
    public float maxTurnSpeed;
    public float damp;
    public float driftingThreshold;

    public void ObservedUpdate()
    {
        if (currentIndex < players.Count)
            PlayState();
        else
            MoveState();
    }

    void PlayState()
    {
        players[currentIndex].CheckForInput(this);
    }

    void MoveState()
    {
        if (lastTurnSpeed == null)
        {
            lastTurnSpeed = new float[players.Count];
            for (int i = 0; i < players.Count; i++)
                lastTurnSpeed[i] = 0f;
        }

        _timer += Time.deltaTime;

        if (_timer < 1f)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].alive)
                {
                    Vector2 endPos = (Vector2)players[i].gameObject.transform.position;
                    Vector2 startPos = endPos - players[i].velocities[players[i].velocities.Count - 1];
                    Vector2 startVelocity = players[i].velocities[players[i].velocities.Count - 2];

                    float x = CalculatePosition(startPos.x, endPos.x, startVelocity.x, _timer);
                    float y = CalculatePosition(startPos.y, endPos.y, startVelocity.y, _timer);

                    playerObjects[i].transform.position = new Vector2(x, y);

                    x = CalculatePosition(startPos.x, endPos.x, startVelocity.x, _timer + Time.deltaTime);
                    y = CalculatePosition(startPos.y, endPos.y, startVelocity.y, _timer + Time.deltaTime);

                    Vector2 dir = endPos - (Vector2)playerObjects[i].transform.position;

                    lastTurnSpeed[i] = CalculateTurnSpeed(dir, playerObjects[i].transform.rotation.eulerAngles.z, lastTurnSpeed[i]);
                    playerObjects[i].transform.Rotate(0f, 0f, lastTurnSpeed[i] * Time.deltaTime);

                    Vector2 idealDir = new Vector2(x, y) - (Vector2)playerObjects[i].transform.position;
                    float speed = (endPos - startPos).magnitude;
                    float similarity = Helpers.CompareDirection(idealDir, playerObjects[i].transform.up);
                    if (similarity < driftingThreshold && speed > 1f)
                        playerObjects[i].GetComponent<SkidManager>().skid = true;
                    else
                        playerObjects[i].GetComponent<SkidManager>().skid = false;
                }
                else
                    Debug.LogWarning("Player is Dead!");
            }
        }
        else
        {
            foreach (GameObject playerObject in playerObjects)
                playerObject.GetComponent<SkidManager>().skid = false;

            _timer = 0f;
            currentIndex = 0;
        }
    }

    public void AddToIndex()
    {
        currentIndex++;
    }

    public void CheckCameraPosition(CameraController camera)
    {
        if (currentIndex < players.Count)
            camera.PlayState(players[currentIndex]);
        else
            camera.MoveState();
    }

    private float CalculatePosition(float x0, float x1, float m, float t)
    {
        float F1 = x1 - x0;

        float c = m;
        float b = F1 - c;

        return b * Mathf.Pow(t, 2f) + c * t + x0;
    }

    private float CalculateTurnSpeed(Vector2 direction, float currentAngle, float turnSpeed)
    {
        float idealAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (idealAngle > 180f)
            idealAngle -= 360f;
        idealAngle *= -1f;
        if (currentAngle > 180f)
            currentAngle -= 360f;
        float difference = idealAngle - currentAngle;

        if (difference >= 0f)
            turnSpeed += turnAcceleration * Time.deltaTime;
        else
            turnSpeed -= turnAcceleration * Time.deltaTime;

        turnSpeed *= Mathf.Exp(-damp * Time.deltaTime);
        turnSpeed = Mathf.Clamp(turnSpeed, -maxTurnSpeed, maxTurnSpeed);

        return turnSpeed;
    }

    public void ReportDeath()
    {
        bool allDead = true;
        foreach(PlayerController player in players)
        {
            if (player.alive)
                allDead = false;
        }

        if (allDead)
            Debug.LogError("All Players Dead!!!");

        AddToIndex();
    }
}
