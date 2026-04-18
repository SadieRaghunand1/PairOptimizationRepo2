using UnityEngine;

public class Healer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.IncreaseHealth();
            gameObject.SetActive(false);
        }
    }
}
