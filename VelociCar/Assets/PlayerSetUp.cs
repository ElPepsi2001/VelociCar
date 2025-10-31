using UnityEngine;

public class PlayerSetUp : MonoBehaviour
{ 
    public PlayerController player;
    public GameObject playerObject;
    void Start()
    {
        RoundManager.instance.players.Add(player);
        RoundManager.instance.playerObjects.Add(playerObject);
        player.velocities.Add(Vector2.zero);
    }
}
