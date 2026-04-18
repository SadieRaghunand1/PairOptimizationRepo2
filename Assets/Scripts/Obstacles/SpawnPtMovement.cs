using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPtMovement : MonoBehaviour
{
    GameManager gm;
    [SerializeField] private Player player;

    float speed = 10;
    [SerializeField] private float resetTrigger;
    [SerializeField] private float resetPos;

    [SerializeField] private List<ObstacleMovement> children = new List<ObstacleMovement>();

    [SerializeField] private GameObject healChk;

    //Increase difficulty over time
    float speedMod = 1.5f;
    int rounds = 0;
    int numResetsChk = 3; //number of resets needed for next checkpoint
    float chkInc = 3.5f; //Amount above num increases by, multiplied and rounded to int

    private void Start()
    {
        gm = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ResetPosition();
    }

    //Control movement
    void Movement()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
        
    }

    //Once reaches behind player, move back to start
    void ResetPosition()
    {
        if(transform.position.z <= resetTrigger)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetPos);

            for(int i = 0; i < children.Count; i++)
            {
                children[i].ReturnToPool();
            }

            //Add round to checkpoint count
            healChk.SetActive(false);
            rounds++;
            if(rounds >= numResetsChk)
            {
                CheckpointHandling();
            }
        }
    }

    //Add obstacle to list
    public void AddChild(ObstacleMovement obst)
    {
        Debug.Log("Adding " + obst);
        children.Add(obst);
    }

    //Remove obstacle when added back to pool
    public void RemoveChild(ObstacleMovement obst)
    {
        children.Remove(obst);
    }

    //Control checkpoint behavior
    void CheckpointHandling()
    {
        //Increase speed
        speed *= speedMod;

        numResetsChk = (int)Mathf.Ceil(chkInc * numResetsChk);
        gm.SetCheckPoint(numResetsChk);

        //Reset rounds, release healing object
        rounds = 0;
        healChk.SetActive(true);
        //player.IncreaseHealth();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
