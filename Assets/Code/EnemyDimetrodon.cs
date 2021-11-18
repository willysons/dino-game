using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDimetrodon : MonoBehaviour
{
    public float speed = .05f;
    public float radius = 5;
    public LayerMask playerLayer;
   
    public float mMovementSpeed = 0.05f;
    public bool bIsGoingRight = true;
    private SpriteRenderer _mSpriteRenderer;
    public float mRaycastingDistance = 1f;

    SpriteRenderer _renderer;
    Collider2D _collider;
    Rigidbody2D _rigidbody;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();

        _mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _mSpriteRenderer.flipX = bIsGoingRight;
    }

    void Update()
    {
        // if the ennemy is going right, get the vector pointing to its right
        Vector3 directionTranslation = (bIsGoingRight) ? transform.right : -transform.right; 
        directionTranslation *= Time.deltaTime * mMovementSpeed;

        transform.Translate(directionTranslation);


        CheckForWalls();
    }

    private void CheckForWalls()
    {
        Vector3 raycastDirection = (bIsGoingRight) ? Vector3.right : Vector3.left;

        // Raycasting takes as parameters a Vector3 which is the point of origin, another Vector3 which gives the direction, and finally a float for the maximum distance of the raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position + raycastDirection * mRaycastingDistance - new Vector3(0f, 0.25f, 0f), raycastDirection, 0.075f);

        // if we hit something, check its tag and act accordingly
        if (hit.collider != null)
        {
            if (hit.transform.tag == "Ground")
            {
                bIsGoingRight = !bIsGoingRight;
                _mSpriteRenderer.flipX = bIsGoingRight;

            }
        }
    }

    // void FixedUpdate()
    // {
    //     Collider2D[] closeColliders = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
    //     if (closeColliders.Length > 0)
    //     {
    //         Vector2 playerPos = closeColliders[0].transform.position;
    //         transform.position = Vector2.MoveTowards(transform.position, playerPos, speed);
            
    //         if ((transform.position.x < playerPos.x && transform.localScale.x > 0) || (transform.position.x > playerPos.x && transform.localScale.x < 0))
    //         {
    //             transform.localScale *= new Vector2(-1, 1);
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Bullet")) 
        {
            _renderer.flipY = true;
            _collider.enabled = false;
            mMovementSpeed = 0;
            Destroy(other.gameObject);
            Destroy(gameObject, 2);
        }
    }
}
