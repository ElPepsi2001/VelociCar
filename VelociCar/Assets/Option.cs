using UnityEngine;

public class Option : MonoBehaviour
{
    [HideInInspector]
    public PlayerController player = null;

    private void OnMouseEnter()
    {
        player.option = this.gameObject;
    }
    private void OnMouseExit()
    {
        player.option = null;
    }
}
