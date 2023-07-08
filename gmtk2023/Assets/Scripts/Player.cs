using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    private Vector3 initialScale;
    public float speed = 5f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        initialScale = transform.localScale;

    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        moveDelta = new Vector3(x, y, 0);
        moveDelta*= speed;

        /*
                //swap sprite direction (flip)
                if (moveDelta.x > 0)
                    transform.localScale = Vector3.one;
                // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                else if (moveDelta.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);

                //transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        */
        if (moveDelta.x > 0)
            transform.localScale = initialScale;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);


        // make sure that we can move in this directon by casting a box there first if the box returns null we are free to move
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }


        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Make this thing move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }


    }
}
