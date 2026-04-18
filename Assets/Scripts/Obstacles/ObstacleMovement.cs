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



    private void Move()
    {
        if(isActive)
        {
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

    public void ReturnToPool(Collider other = null)
    {
       
        if( other != null && other.gameObject.layer == 7 && canDeactivate)
        {
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
