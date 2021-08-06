using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float Health;
    [SerializeField]
    private float movmentSpeed;
    private int killRewardAmount { get; set; }
    private int damage;

    private MapGenerator.MapTile targetTile;
    private Animator animator;

    private void Awake()
    {
        Enemies.enemies.Add(gameObject);
    }

    void Start()
    {
        init();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        checkPosition();
        move();
    }

    public void takeDamage(float amount)
    {
        Health -= amount;
        if (Health <= 0)
            die();
    }

    private void init()
    {
        targetTile = MapGenerator.startTile;
    }

    private void die()
    {
        Destroy(transform.gameObject);
        Enemies.enemies.Remove(gameObject);
    }

    private void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetTile.position, movmentSpeed * Time.deltaTime);
        if (animator && animator.isInitialized)
            animator.Play("GiantBeetle");
    }

    private void checkPosition()
    {
        if (targetTile != null && targetTile != MapGenerator.endTile)
        {
            float distance = (transform.position - targetTile.Content[0].transform.position).magnitude;
            if (distance < 0.001f)
            {
                int currentindex = MapGenerator.EnemiesPath.IndexOf(targetTile.position);
                var next = MapGenerator.EnemiesPath[currentindex + 1];
                targetTile = MapGenerator.mapTiles[(int)next.x, (int)next.y];

                if (Mathf.Abs(transform.position.y - targetTile.position.y) < 0.001f &&
                    transform.rotation.eulerAngles == Vector3.zero)
                {
                    var r = transform.position.x > targetTile.position.x ? Vector3.forward : Vector3.back;
                    transform.Rotate(r * 90);
                }
                else if (Mathf.Abs(transform.position.x - targetTile.position.x) < 0.001f && transform.rotation.eulerAngles != Vector3.zero)
                {
                    var r = transform.rotation.eulerAngles.z > 0 ? Vector3.forward : Vector3.back;
                    transform.Rotate(90 * r);
                }
            }
        }
    }
}
