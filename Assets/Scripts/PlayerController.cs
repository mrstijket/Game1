using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float boostTimer = 0f;
    [SerializeField] float cooldownBoostTime = 20f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 5f);
    [SerializeField] AudioClip deathSFX;
    private float inputX;
    [SerializeField] Transform projectile;
    public GameObject ninjaStar;
    public LayerMask whatIsGround;

    [Header("Buttons")]
    float attackStart = 0f;
    [SerializeField] float attackColddown = 1f;
    float powerStart = 0f;
    bool isGrounded;
    bool isAlive = true;
    bool isAttack = false;
    bool isBoosted = false;

    [Header("Check Ground")]
    public Transform groundPoint;
    bool canDoubleJump;

    [Header("Projectile")]
    [SerializeField] float projectileSpeed = 10f;
    float projectileStart = 0f;
    [SerializeField] float projectileColddown = 1f;

    Rigidbody2D myRigidbody;
    Animator anim;
    CapsuleCollider2D myBodyCollider;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive)
        { 
            myRigidbody.velocity = new Vector3(0f, myRigidbody.velocity.y);
            return; 
        }
        myRigidbody.velocity = new Vector2(inputX * moveSpeed, myRigidbody.velocity.y);
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        anim.SetBool("Running", Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon);

        BoostProcess();
        Die();
        FlipSprite();
    }
    public void Move(InputAction.CallbackContext context)
    {
        inputX = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed)
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
            canDoubleJump = true;
            anim.SetTrigger("Jump");
        }
        else
        {
            if (canDoubleJump && context.performed)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, jumpForce);
                canDoubleJump = false;
                anim.SetTrigger("Jump");
            }
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if(!isAlive) { return; }
        if (context.performed && Time.time > attackStart + attackColddown)
        {
            attackStart = Time.time;
            isAttack = true;
            anim.SetTrigger("Attack");
        }
    }
    public void Boost(InputAction.CallbackContext context)
    {
        if (!isAlive) { return; }
        if (context.performed)
            isBoosted = true;
    }
    public void BoostProcess()
    {
        if (isBoosted)
        {
            boostTimer += Time.deltaTime;
            anim.SetBool("Boosting", true);
            if (boostTimer >= 5)
            {
                isBoosted = false;
                anim.SetBool("Boosting", false);
                StartCoroutine(ZeroBoostTimer());
            }
        }
    }
    IEnumerator ZeroBoostTimer()
    {
        yield return new WaitForSeconds(cooldownBoostTime);
        boostTimer = 0;
    }
    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            isAlive = false;
            anim.SetTrigger("Death");
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            GetComponent<Rigidbody2D>().velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    public void Throw(InputAction.CallbackContext context)
    {
        if (!isAlive) { return; }
        if (context.performed && Time.time > projectileStart + projectileColddown)
        {
            projectileStart = Time.time;
            StartCoroutine(FireContinuously());
        }
    }
    IEnumerator FireContinuously()
    {
        GameObject star = Instantiate(ninjaStar, projectile.position, Quaternion.identity) as GameObject;
        if (transform.localScale.x > 0f)
            star.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        if(transform.localScale.x < 0f)
            star.GetComponent<Rigidbody2D>().velocity = new Vector2(-projectileSpeed, 0);
        yield return new WaitForSeconds(1f);
        Destroy(star);
    }
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
}
