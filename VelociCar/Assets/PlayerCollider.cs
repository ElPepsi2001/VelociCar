using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public PlayerController controller;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "MapCollider")
            controller.alive = false;
    }
}
