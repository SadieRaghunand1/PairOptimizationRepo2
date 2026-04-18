using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, PlayerControls.IPlayerActions
{
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;

    private PlayerControls controls;

    private GameManager gm;


    //Player components 
    public Rigidbody rb;

    //Jump
    public float jumpForce;
    private bool isGrounded;

    //Movement
    Vector3 movement;
    [SerializeField] private float speed;

    //Other
    int health = 3;
    float immunityTime = 3;
    bool isImmune;
    [SerializeField] private GameUIManager gUIManager;
    [SerializeField] private SpawnPtMovement[] lanes;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindAnyObjectByType<GameManager>();
    }

    #region input
    private void OnEnable()
    {
        if (controls != null)
            return;

        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnDisable()
    {
        controls.Player.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();

       movement = new Vector3(MoveComposite.x, 0, 0);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGrounded)
            return;

        OnJumpPerformed?.Invoke();
        Debug.Log("Jump!");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region collision
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            ChangeIsGrounded();
        }

        //Collide w obstacle
        if(collision.gameObject.layer == 8)
        {
            ObstacleMovement ob = collision.gameObject.GetComponent<ObstacleMovement>();
            GetDamaged(ob.GetDamage(), ob.GetPts());
        }

        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            ChangeIsGrounded();
        }
    }

    private void ChangeIsGrounded()
    {
        isGrounded = !isGrounded;
    }
    #endregion

    void FixedUpdate()
    {
        moveCharacter(movement);
    }


    void moveCharacter(Vector3 direction)
    {
        rb.AddForce(direction * speed);
    }

    void GetDamaged(int damage, int ptsSub)
    {
        if(!isImmune)
        {
            if (health - damage <= 0)
            {
                StartCoroutine(DelayDeath());
                gUIManager.SetHealthImgs(true);
            }
            else
            {
                health -= damage;
                gUIManager.SetHealthImgs(true);
            }

            StartCoroutine(Immunity());
            gm.IncPts(-ptsSub);
        }

    }

    public void IncreaseHealth()
    {
        health++;
        gUIManager.SetHealthImgs(false);
    }

    void Die()
    {
        gm.EndGame();
    }

    IEnumerator DelayDeath()
    {
        GetComponent<Renderer>().material.color = Color.red;
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i].SetSpeed(0);
        }
        gUIManager.GetScoreText().color = Color.green;

        yield return new WaitForSeconds(3);
        Die();
    }

    IEnumerator Immunity()
    {
        isImmune = true;
        GetComponent<Renderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(immunityTime);
        isImmune = false;
        GetComponent<Renderer>().material.color = Color.green;
    }


}
