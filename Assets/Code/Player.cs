using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int speed = 5;
    int jumpForce = 800;
    int bulletForce = 600;
    Rigidbody2D _rigidbody;
    Animator _animator;

    public Transform feet;
    public LayerMask ground;
    public bool isGrounded = false;

    public GameObject bulletPrefab;
    public Transform spawnPos;

    private bool hurt = false;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (hurt)
        {
            return;
        }
        float xSpeed = Input.GetAxis("Horizontal") * speed;
        _rigidbody.velocity = new Vector2(xSpeed, _rigidbody.velocity.y);
        _animator.SetFloat("speed", Mathf.Abs(xSpeed));

        if ((xSpeed < 0 && transform.localScale.x > 0) || (xSpeed > 0 && transform.localScale.x < 0))
        {
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    void Update()
    {
        if (hurt)
        {
            return;
        }
        isGrounded = Physics2D.OverlapCircle(feet.position, .3f, ground);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _rigidbody.AddForce(new Vector2(0, jumpForce));
        }
        // transform.position = new Vector3 (transform.position.x + 6, 0); // Camera follows the player but 6 to the right

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("shoot");
            GameObject newBullet = Instantiate(bulletPrefab, spawnPos.position, Quaternion.identity);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(new Vector2(transform.localScale.x * bulletForce, 0));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy")) 
        {
            hurt = true;
            _animator.SetTrigger("hurt");
            _rigidbody.velocity = new Vector2(-transform.localScale.x * 8, _rigidbody.velocity.y);
            StartCoroutine(NotHurt());
        }
    }

    IEnumerator NotHurt() {
        yield return new WaitForSeconds(.2f);
        hurt = false;
    }
}
