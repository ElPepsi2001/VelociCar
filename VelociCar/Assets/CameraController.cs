using UnityEngine;

public class CameraController : MonoBehaviour, IUpdateObserver
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

    public void ObservedUpdate()
    {
        RoundManager.instance.CheckCameraPosition(this);
    }
    public void PlayState(PlayerController playerController)
    {
        Vector3 optionPosition = playerController.transform.position + (Vector3)playerController.currentVelocity;
        Vector3 idealCameraPosition = (playerController.transform.position + optionPosition) / 2f;
        idealCameraPosition = new Vector3(idealCameraPosition.x, idealCameraPosition.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, idealCameraPosition, Time.deltaTime);
    }
    public void MoveState()
    {

    }
}
