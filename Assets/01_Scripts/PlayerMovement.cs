using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float velocity = 5;
    public Animator animator;

    public float jumpforce = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask floorMask;

    private bool touchFloor;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float velocityX = Input.GetAxis("Horizontal")*Time.deltaTime;
        animator.SetFloat("movement",velocityX * velocity);

        if (velocityX < 0)
        {
            transform.localScale = new Vector3 (-1,1,1);
        }
        if (velocityX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        Vector3 posicion =transform.position;
        transform.position = new Vector3(velocityX+posicion.x,posicion.y,posicion.z);

        RaycastHit2D hit= Physics2D.Raycast(transform.position,Vector2.down,longitudRaycast,floorMask);
        touchFloor=hit.collider!=null;

        Jump();
    }

    void Jump()
    {
        if (touchFloor && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpforce), ForceMode2D.Impulse);
            touchFloor = false;
        }
        animator.SetBool("touchingFloor", touchFloor);

    }


    void OnCollisinEnter2D (Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            touchFloor = true;
        }
    }
    void onDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
