using UnityEngine;

public class TriggerPts : MonoBehaviour
{
    [SerializeField] ObstacleMovement parent;
    private GameManager gm;

    private void Start()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
        {
            
            gm.IncPts(parent.GetPts());
        }
    }
}
