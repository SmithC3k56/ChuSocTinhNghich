using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private GameObject _footGameObject;


    private PlayerModel _player = null;

    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private BoxCollider2D _footColl;
    private Animator anim;
    private SpriteRenderer sprite;
    private float dirX = 0f;
    private float currentSpeed = 0f;
    private bool isMove = true;
    [SerializeField] private GameObject[] Hearts;
    [SerializeField] private Text gemLb;
    private int totalHearts = 2;
    private float totalGem = 0f;

    private enum MovementState
    {
        idle,
        crouching,
        running,
        jumping,
        falling,
        clibing,
        hurting
    }

    [SerializeField] private AudioSource jumpSoundEffect;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float forceMagnitude = 20.0f; // Magnitude của lực

    private MovementState state = MovementState.idle;
    private bool isHurt = false;
    private bool isFacingRight = true;
    // Start is called before the first frame update
    private void Start()
    {
        this._footColl = this._footGameObject.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        coll = this.GetComponent<BoxCollider2D>();
        jumpSoundEffect = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        totalGem = PlayerPrefs.GetFloat("Coin");
        gemLb.text = totalGem.ToString();
    }


    // Update is called once per frame
    private void Update()
    {
        if (isDashing)
        {
            return;
        }

      
        dirX = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ? 
            Input.GetAxisRaw("Horizontal") : 0f;
     
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyUp("space") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        UpdateAnimationState();
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (totalHearts > 0)
            {
                isHurt = true;
                Hearts[totalHearts - 1].SetActive(false);
                totalHearts--;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
           
            Apply45DegreeForce();
            
        }
        else if (other.gameObject.CompareTag("End"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }
        else if (other.gameObject.CompareTag("item"))
        {
            StartCoroutine(DestroyWithDelay(other.gameObject));
            if (totalHearts < 3)
            {
                totalHearts++;
                Hearts[totalHearts - 1].SetActive(true);
            }
        }
        else if (other.gameObject.CompareTag("gem"))
        {
            StartCoroutine(DestroyWithDelay(other.gameObject));
            totalGem++;
            gemLb.text = totalGem.ToString();
            PlayerPrefs.SetFloat("Coin", totalGem);
        }
    }

    IEnumerator DestroyWithDelay(GameObject e)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(e);
    }

    void Apply45DegreeForce()
    {
        Vector2 forceDirection = new Vector2(3.0f, 1f).normalized;
        Vector2 force = forceDirection * forceMagnitude;
        
        rb.AddForce(force, ForceMode2D.Impulse);
    }

   
    
    private void UpdateAnimationState()
    {
        anim.SetFloat("Blend", float.Parse(PlayerPrefs.GetString("TypeChar")));
    
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            state = MovementState.crouching;
        }
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && IsGrounded())
        {
            state = MovementState.running;
        }
        else if (rb.velocity.y > 0.1f && !IsGrounded())
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f &&! IsGrounded())
        {
            state = MovementState.falling;
        }
        else if (IsGrounded() && dirX == 0)
        {
            state = MovementState.idle;
        }
        else if (isHurt)
        {
            state = MovementState.hurting;
            isHurt = false;
        }

        anim.SetInteger("state", (int)state);
    }
    private void Flip()
    {
        if (isFacingRight && dirX < 0f || !isFacingRight && dirX > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private bool IsGrounded()
    {
        // return Physics2D.BoxCast(_footColl.bounds.center, _footColl.bounds.size, 0, Vector2.down, 0.1f, jumpableGround);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpableGround);

    }
}