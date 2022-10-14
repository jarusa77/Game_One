using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;

    public Transform groundDetection;

    public float distanceDown = 2f;
    private float direction = 1;//determines direction based on  facing 
    private bool isFacingRight = true;

    void Update()
    {

        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distanceDown);
        if(!groundInfo.collider)
        {


        }

    }


    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        direction *= -1f;
    }



}
