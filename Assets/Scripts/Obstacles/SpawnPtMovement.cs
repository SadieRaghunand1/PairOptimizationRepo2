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

    void Movement()
    {
        transform.Translate(Vector3.back * Time.deltaTime * speed);
        
    }

    void ResetPosition()
    {
        if(transform.position.z <= resetTrigger)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, resetPos);

            for(int i = 0; i < children.Count; i++)
            {
                children[i].ReturnToPool();
            }

            healChk.SetActive(false);
            rounds++;
            if(rounds >= numResetsChk)
            {
                CheckpointHandling();
            }
        }
    }

    public void AddChild(ObstacleMovement obst)
    {
        Debug.Log("Adding " + obst);
        children.Add(obst);
    }

    public void RemoveChild(ObstacleMovement obst)
    {
        children.Remove(obst);
    }

    void CheckpointHandling()
    {
        speed *= speedMod;

        numResetsChk = (int)Mathf.Ceil(chkInc * numResetsChk);
        gm.SetCheckPoint(numResetsChk);

        rounds = 0;
        healChk.SetActive(true);
        //player.IncreaseHealth();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
