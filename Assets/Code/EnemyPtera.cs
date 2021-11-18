using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPtera : MonoBehaviour
{
    public float speed = .05f;
    public float radius = 4;
    public LayerMask playerLayer;

    SpriteRenderer _renderer;
    Collider2D _collider;
    Rigidbody2D _rigidbody;
    // Animator _animator;
    // GameObject player;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
        // _animator = GetComponent<Animator>();
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        Collider2D[] closeColliders = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        if (closeColliders.Length > 0)
        {
            Vector2 playerPos = closeColliders[0].transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, speed);
            
            if ((transform.position.x < playerPos.x && transform.localScale.x > 0) || (transform.position.x > playerPos.x && transform.localScale.x < 0))
            {
                transform.localScale *= new Vector2(-1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet")) 
        {
            Destroy(other.gameObject);
            _renderer.flipY = true;
            _collider.enabled = false;
            _rigidbody.gravityScale = 5;
            
            Destroy(gameObject, 2);
        }
    }
}
