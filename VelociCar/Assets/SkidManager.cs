using UnityEngine;

public class SkidManager : MonoBehaviour, IUpdateObserver
{
    [HideInInspector]
    public bool skid = false;
    public TrailRenderer[] trails;
    public GameObject puff;
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

    public void ObservedUpdate()
    {
        foreach(TrailRenderer trail in trails)
        {
            trail.emitting = skid;
            if(skid)
            {
                int chance = (int)Mathf.Round(0.2f / Time.deltaTime);
                if (chance < 1)
                    chance = 1;
                if(Random.Range(0, chance) == 0)
                {
                    GameObject instPuff = Instantiate(puff, trail.gameObject.transform.position, Quaternion.identity);
                    Destroy(instPuff, 1f);
                }
            }
        }
    }
}
