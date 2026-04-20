using System.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] ObstacleMovement[] obstacles;
    [SerializeField] float minSpeed, maxSpeed;
    [SerializeField] private Transform[] leftPts;
    [SerializeField] private Transform[] rightPts;
    [SerializeField] private Transform leftParent;
    [SerializeField] private SpawnPtMovement leftSc;
    [SerializeField] private Transform rightParent;
    [SerializeField] private SpawnPtMovement rightSc;

  //  float timeBwRelease = 2; //Decrease as game goes on?
    private WaitForSeconds TimeReleaseWait = new WaitForSeconds(4f);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimeRelease());
    }

    //Release cloud from object pool
    public void ReleaseFromPool()
    {
        //Chhose a random cloud, if inactive then release
        int toRelease = Random.Range(0, obstacles.Length);

        if (obstacles[toRelease].GetIsActive())
        {
            return;
        }

        //Set activity and speed
        ObstacleMovement thisObst = obstacles[toRelease];
        thisObst.gameObject.SetActive(true);
        thisObst.SetIsActive(true);
        thisObst.SetSpeed(Random.Range(minSpeed, maxSpeed));
        
        //Choose a side/lane
        bool side = (Random.Range(0, 2) == 0) ? false : true;
        Transform chosen;

        thisObst.transform.parent = (side) ? leftParent : rightParent;
        if (side) //Left
        {
            chosen = leftPts[Random.Range(0, leftPts.Length)];
            StartCoroutine(thisObst.SetCanDeactivate());
            leftSc.AddChild(thisObst);
        }
        else //Right
        {
            chosen = rightPts[Random.Range(0, rightPts.Length)];
            StartCoroutine(thisObst.SetCanDeactivate());
            rightSc.AddChild(thisObst);
        }

        thisObst.SetMoveDirectionAndStartPoint(side, chosen);

    }

    //Do consistent releases on timer
    IEnumerator TimeRelease()
    {
        while (true)
        {
            yield return TimeReleaseWait;
            ReleaseFromPool();
            StartCoroutine(TimeRelease());
        }
          
    }
}
