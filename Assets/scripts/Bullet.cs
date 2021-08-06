using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float damages;

    void Start()
    {
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null && (collision.collider.gameObject.tag is "Enemy"))
        {
            Enemy enemy = collision.collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.takeDamage(damages);
            }
        }
        Destroy(gameObject);
    }
    // Update is called once per frame
    private void Update()
    {
        transform.position += transform.up * .25f;
    }
}
