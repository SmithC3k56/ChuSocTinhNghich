using System;
using System.Collections;
using System.Linq;
using DefaultNamespace;
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


    [SerializeField] private float forceMagnitude = 20.0f; // Magnitude của lực

    private MovementState state = MovementState.idle;

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
        dirX = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetKey("space") && IsGrounded())
        {
            // jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (totalHearts > 0)
            {
                
                Hearts[totalHearts - 1].SetActive(false);
                totalHearts--;
            }
            else
            {
                SceneManager.LoadScene("Lv1");
            }

            anim.SetInteger("state", (int)MovementState.hurting);
            Apply45DegreeForce();
        }
        else if (other.gameObject.CompareTag("End"))
        {
            SceneManager.LoadScene("Lv1");
        }
        else if (other.gameObject.CompareTag("item"))
        {
            Debug.Log("item");
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
        // Tính vector lực với góc 45 độ
        Vector2 forceDirection = new Vector2(3.0f, 1f).normalized; // Vector đơn vị 45 độ
        Vector2 force = forceDirection * forceMagnitude;


        // Thêm lực vào đối tượng
        rb.AddForce(force, ForceMode2D.Impulse); // Sử dụng ForceMode2D.Impulse để tạo ra lực ngắn hạn
    }

    private void UpdateAnimationState()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            state = MovementState.crouching;
        }

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }
        else
        {
            state = MovementState.idle;
        }

        anim.SetInteger("state", (int)state);
        anim.SetFloat("Blend", float.Parse(PlayerPrefs.GetString("TypeChar")));
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_footColl.bounds.center, _footColl.bounds.size, 0, Vector2.down, 0.1f, jumpableGround);
    }
}