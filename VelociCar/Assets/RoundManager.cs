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
    int currentIndex = 0;
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
        bool arrived = true;

        for(int i = 0; i < players.Count; i++)
        {
            Vector2 pos1 = playerObjects[i].transform.position * 2f;
            Vector2 roundedPos1 = new Vector2(Mathf.Round(pos1.x), Mathf.Round(pos1.y)) / 2f;

            Vector2 pos2 = players[i].gameObject.transform.position * 2f;
            Vector2 roundedPos2 = new Vector2(Mathf.Round(pos2.x), Mathf.Round(pos2.y)) / 2f;
            if (roundedPos1 != roundedPos2)
            {
                arrived = false;
                playerObjects[i].transform.position = Vector2.Lerp(playerObjects[i].transform.position, (Vector2)players[i].gameObject.transform.position, Time.deltaTime);
            }
        }

        if(arrived)
            currentIndex = 0;
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
}
