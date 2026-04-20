using UnityEngine;

public class Healer : MonoBehaviour
{

    //On trigger with player, increase health
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Heal hit " + other);
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.IncreaseHealth();
            gameObject.SetActive(false);
        }
    }
}
