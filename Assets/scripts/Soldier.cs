using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [SerializeField]
    private float range;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float timeBetweenShots;
    private float nextTick;

    public GameObject bullet;
    public GameObject target;
    private Animator animator;

    void Start()
    {
        nextTick = Time.time;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        updateNearestEnemy();
        if (Time.time >= nextTick && target != null)
            shoot();
    }

    private void updateNearestEnemy()
    {
        GameObject nearestEnemy = null;

        float distance = float.PositiveInfinity;

        foreach (GameObject enemy in Enemies.enemies)
        {
            float _distance = (transform.position - enemy.transform.position).magnitude;
            if (_distance < distance)
            {
                distance = _distance;
                nearestEnemy = enemy;
            }

        }
        if (distance <= range)
        {
            target = nearestEnemy;
        }
        else target = null;
        if (target)
            rotate(target.transform.position);
    }

    private void shoot()
    {
        if (animator && animator.isInitialized)
        {
            animator.Play("Character1_Shoot");
        }
        //Enemy enemyScript = target.GetComponent<Enemy>();
        //enemyScript.takeDamage(damage);

        Instantiate(bullet, transform.position, transform.rotation);
        nextTick = Time.time + timeBetweenShots;
    }

    private void rotate(Vector2 position)
    {
        Vector2 point = position - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(point.y, point.x) * Mathf.Rad2Deg;

        Vector3 rotation = new Vector3(0, 0, angle - 90);
        transform.localRotation = Quaternion.Euler(rotation);
    }
}
