using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //[SerializeField] GameObject particles;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
