using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float scaleY = 1f;
    [SerializeField] float Xfactor = 1f;
    [SerializeField] float VFXTime = 0.7f;
    [SerializeField] float destroyTime = 5f;
    [SerializeField] GameObject VFX;
    [SerializeField] AudioClip deathSFX;

    Rigidbody2D enemyRigidbody;
    Animator enemyAnimator;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFacingRight())
        {
            enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }
    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("DamageHiter"))
        {
            VFX.transform.position = this.transform.position;
            StartCoroutine(Dismantle());
        }
    }
    IEnumerator Dismantle()
    {
        VFX.SetActive(true);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        moveSpeed = 0f;
        enemyAnimator.SetTrigger("Death");
        yield return new WaitForSeconds(VFXTime);
        VFX.SetActive(false);
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(enemyRigidbody.velocity.x)) * Xfactor, scaleY);
    }
}
