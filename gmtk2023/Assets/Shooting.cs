using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform shootPoint;
    public GameObject arrowPrefab;

    public float arrowForce = 20f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            Shoot();
        }
    }
    void Shoot()
    {
        GameObject arrow =Instantiate(arrowPrefab,shootPoint.position,shootPoint.rotation);
        Rigidbody2D rb =arrow.GetComponent<Rigidbody2D>();
        rb.AddForce(shootPoint.up * arrowForce, ForceMode2D.Impulse);


    }
}
