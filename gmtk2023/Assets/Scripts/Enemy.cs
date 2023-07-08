using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject player;
    Vector3 target;
    NavMeshAgent agent;
    LayerMask playerlayer;
    Vector3 originalLocation;

    public float visionDistance = 5f;
    public int number_of_rays = 40;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform.position;
        agent.SetDestination(target);
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        originalLocation = transform.position;
        target = originalLocation;
        playerlayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        setTarget();
        agent.SetDestination(target);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
    public void setTarget()
    {
        //Collider2D targetArea = Physics2D.overlapCircle(transform.position,5f,playerlayer);
        //if(targetArea!=null){
        //     target = targetArea.gameObject.transform.position;
        // }
        var direction = Quaternion.Euler(0, 0, 90) * transform.right;
        RaycastHit2D LOS = Physics2D.Raycast(transform.position, direction, 5f);
        if (LOS && LOS.collider.gameObject.CompareTag("Player"))
        {
            //target= LOS.collider.gameObject.transform.position;
            target = player.transform.position;
        }


    }
    /*public void setTarget()
    {
        float angle = 360f / number_of_rays;
        float cast_angle = 0f;

        for (int i = 0; i < number_of_rays; i++)
        {
            var dir = Quaternion.Euler(0f, 0f, cast_angle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, visionDistance);

            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                target = player.transform.position;
                return;
            }

            cast_angle += angle;
        }

        target = originalLocation;
    }*/

}
