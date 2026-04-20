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
    private Renderer rend; // CACHED: Renderer

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
    
    // CACHED: WaitForSeconds to prevent GC allocation on every call
    private WaitForSeconds immunityWait;
    private WaitForSeconds deathWait;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponent<Renderer>();
        gm = FindAnyObjectByType<GameManager>();
        
        // Initialize cached wait times
        immunityWait = new WaitForSeconds(immunityTime);
        deathWait = new WaitForSeconds(3f);
    }

    #region input
    private void OnEnable()
    {
        //Set all input
        if (controls != null)
            return;

        controls = new PlayerControls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    //De-couple controls
    public void OnDisable()
    {
        controls.Player.Disable();
    }

    //Control movement using input system
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();

        //Calculate movement vector
        movement = new Vector3(MoveComposite.x, 0, 0);
    }

    //Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !isGrounded)
            return;

        //Perform jump with rigidbody force physics
        OnJumpPerformed?.Invoke();
        Debug.Log("Jump!");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    #endregion

    #region collision
    private void OnCollisionEnter(Collision collision)
    {
        //Control grounded to prevent double jumps
        if(collision.gameObject.layer == 6)
        {
            ChangeIsGrounded();
        }

        //Collide w obstacle, do damage
        if(collision.gameObject.layer == 8)
        {
            // OPTIMIZATION: TryGetComponent is faster and safer
            if (collision.gameObject.TryGetComponent(out ObstacleMovement ob))
            {
                GetDamaged(ob.GetDamage(), ob.GetPts());
            }
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

    //Apply damage
    void GetDamaged(int damage, int ptsSub)
    {
        //Check immunity
        if(!isImmune)
        {
            //Check if this would kill player
            if (health - damage <= 0)
            {
                //Wait to load the main menu
                StartCoroutine(DelayDeath());
                gUIManager.SetHealthImgs(true);
            }
            else
            {
                //Reduce health
                health -= damage;
                gUIManager.SetHealthImgs(true);
            }

            //Make immune for period of time after being damaged
            StartCoroutine(Immunity());
            gm.IncPts(-ptsSub);
        }
    }

    //Add health
    public void IncreaseHealth()
    {
        health++;
        gUIManager.SetHealthImgs(false);
    }

    //End game
    void Die()
    {
        gm.EndGame();
    }

    //Delay death 
    IEnumerator DelayDeath()
    {
        //Change player material
        rend.material.color = Color.red;
        
        //Stop cloud movements
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i].SetSpeed(0);
        }
        
        //Set score text color
        gUIManager.GetScoreText().color = Color.green;

        //Die
        yield return deathWait;
        Die();
    }

    //Set immunity 
    IEnumerator Immunity()
    {
        //Prevent player from being damaged multiple times in a couple frames
        isImmune = true;
        rend.material.color = Color.yellow;
        
        yield return immunityWait;
        
        isImmune = false;
        rend.material.color = Color.green;
    }
}
