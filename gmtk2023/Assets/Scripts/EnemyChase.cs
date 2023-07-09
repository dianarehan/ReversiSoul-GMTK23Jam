using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyChase : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public float attackCooldown;
    public float damage;

    private float distance;
    private bool canAttack = true;
    private CharacterController playerCharacter;

    private void Start()
    {
        playerCharacter = player.GetComponent<CharacterController>();
    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (distance <= 1f && canAttack)
        {
            Attack();
        }
    }

    private void Attack()
    {
        canAttack = false;
        StartCoroutine(AttackCooldown());

        if (playerCharacter != null)
        {
            playerCharacter.ReceiveDamage(damage);
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}



