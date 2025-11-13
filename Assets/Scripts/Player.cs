using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int currentCoin = 0;
    public int maxHealth = 3;
    private bool isDead = false;
    public Text health;
    public Text coinText;
    public Animator animator;
    public Rigidbody2D rb;
    public float jumpHeight = 5f;
    private bool isGround = true;
    
    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    void Start()
    {
        
    }

    void Update()
    {
        if (isDead) return;
        movement = Input.GetAxis("Horizontal"); 
        transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * moveSpeed;
        health.text = maxHealth.ToString();
        coinText.text = currentCoin.ToString();
        
       if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
       else if(movement > 0f && facingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
       if (Input.GetKey(KeyCode.Space) && isGround)
        {
            Jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }
        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
        if (maxHealth <= 0 && isDead == false)
        {
            isDead = true;
            Die();
        }

    }

    void Jump()
    {
        Vector2 vel = rb.linearVelocity; 
        vel.y = jumpHeight; 
        rb.linearVelocity = vel;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            currentCoin++;
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected");
            Destroy(other.gameObject, 1f);
        }

        if (other.gameObject.tag == "VictoryPoint")
        {
            FindFirstObjectByType<SceneLoader>().LoadLevel();
        }
    }

    public void Attack()
    {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<PatrolEnemy>() != null)
            {
                collInfo.gameObject.GetComponent <PatrolEnemy>().TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;
        maxHealth -= damage;
        animator.SetTrigger("Hurt");
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;
        GameManager gm = Object.FindFirstObjectByType<GameManager>();
        gm.isGameActive = false;
    }   

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}



