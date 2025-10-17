using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    List<GameObject> options = new List<GameObject>();
    public Vector2 currentVelocity = Vector2.zero;
    public Vector2 protoVelocity = Vector2.zero;
    int acceleration = 1;
    int deceleration = 1;
    int steering = 1;
    public GameObject optionPrefab;
    [HideInInspector]
    public GameObject option = null;
    private bool optionsSpawned = false;

    void SetUpRound()
    {
        optionsSpawned = true;

        int newAcc = acceleration;
        int newDec = deceleration;
        int newSteer = steering;
        //Change Depending On Circumstances

        for (int x = -newSteer; x <= newSteer; x++)
        {
            for (int y = -newDec; y <= newAcc; y++)
            {
                Vector2 possVector = currentVelocity + new Vector2(x, y);
                SpawnOption((Vector2)transform.position + possVector);
            }
        }

        GameObject rot = new GameObject(); rot.transform.position = (Vector2)transform.position + currentVelocity;
        foreach (GameObject option in options)
        {
            option.transform.SetParent(rot.transform);
        }
        rot.transform.rotation = transform.rotation;
        foreach (GameObject option in options)
        {
            option.transform.SetParent(null);
        }
        Destroy(rot);
    }

    void SpawnOption(Vector2 pos)
    {
        GameObject newOption = Instantiate(optionPrefab, pos, Quaternion.identity);
        options.Add(newOption);
        newOption.GetComponent<Option>().player = this;
    }

    public void ChangeVelocity(Vector2 pos)
    {
        Vector2 newVelocity = pos - (Vector2)transform.position;
        protoVelocity = newVelocity;
    }

    void ClearAllOptions()
    {
        optionsSpawned = false;

        foreach (GameObject option in options)
        {
            if(option != null)
                Destroy(option);
        }

        options.Clear();
    }
    void SubmitVelocity()
    {
        transform.position = (Vector2)transform.position + protoVelocity;
        currentVelocity = protoVelocity;
        ClearAllOptions();
    }
    public void CheckForInput(RoundManager manager)
    {
        if(!optionsSpawned)
            SetUpRound();

        if (Input.GetKeyDown(KeyCode.Mouse0) && option != null)
        {
            ChangeVelocity(option.transform.position);
            SubmitVelocity();
            manager.AddToIndex();
        }
    }
}
