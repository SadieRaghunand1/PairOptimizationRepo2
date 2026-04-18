using System;
using System.Collections;
using UnityEditor.UI;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    bool moveLeft;
    private Rigidbody rb;
    private float speed = 5;
    private Vector3 moveDir;
    bool isActive;
    bool canDeactivate;

    PoolManager pool;

    [SerializeField] private int damage;
    [SerializeField] private int points;

    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pool = FindAnyObjectByType<PoolManager>();
        
    }

    private void FixedUpdate()
    {
        Move();
    }


    //Control movement on clouds
    private void Move()
    {
        //Check if this cloud is active
        if(isActive)
        {
            //Move based on lane direction
            if (moveLeft)
            {
                moveDir = Vector3.right * speed;
            }
            else
            {
                moveDir = Vector3.left * speed;
            }

            rb.linearVelocity = moveDir;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        ReturnToPool(other);
    }

    //When collide w/boundaries, return to object pool
    public void ReturnToPool(Collider other = null)
    {
       //Check for double collisions and make sure collided with correct barrier
        if( other != null && other.gameObject.layer == 7 && canDeactivate)
        {
            //Set inactive, de-parent from lane
            isActive = false;
            Debug.Log(transform.parent + ", " + this.gameObject.name);
            if(transform.parent != null)
            {
                transform.parent.GetComponent<SpawnPtMovement>().RemoveChild(this);
                transform.parent = null;
                gameObject.SetActive(false);
            }
                
        }
    }

    //Setters and getters
    public void SetMoveDirectionAndStartPoint(bool left, Transform startPos)
    {
        moveLeft = left;
        transform.position = startPos.transform.position;
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetIsActive(bool active)
    {
        isActive = active;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetPts()
    {
        return points;
    }

    public IEnumerator SetCanDeactivate()
    {
        yield return new WaitForSeconds(2);
        canDeactivate = true;
    }
}
