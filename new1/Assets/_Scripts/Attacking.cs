using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public static Attacking instance;
    private Transform target;
    private Vector3 dir;
    private float playerShipDamage;
    public GameObject basicLoot;
    public GameObject basicLootIdleParticle;
    public GameObject explosionEffect;
    public GameObject GhostExplosionEffect;
    public GameObject GhostLoot;
    public GameObject hitEffect;
    public GameObject fireHitEffect;
    Rigidbody rb;
    public float speed = 12f;
    public float minDistance = 0.09f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        playerShipDamage = PlayerAttackController.instance.PlayerShip.attack;
        rb = GetComponent<Rigidbody>();
        int random = Random.Range(4, 5);
        rb.velocity = transform.up * random;
    }
    private void FixedUpdate()
    {
        if (target == null && PlayerAttackController.instance.inRange)
        {
            PlayerAttackController.instance.inRange = false;
        }
        if (target == null)
        {
            target = PlayerAttackController.instance.target;
        }
        if (target != null)
        {
            dir = (target.position - transform.position).normalized;
        }
        if (target != null)
        {
            //Check if we need to follow object then do so 
            if (Vector3.Distance(target.transform.position, rb.transform.position) > minDistance)
            {
                transform.LookAt(target.transform.position);
                rb.MovePosition(rb.transform.position + dir * speed * Time.fixedDeltaTime);
            }
            else
            {
                HitTarget();
            }
        }

    }
    void HitTarget()
    {
        if (target != null)
        {
            PlayerAttackController.instance.AttackingEnemies(target.gameObject, playerShipDamage); // damage it
            target.gameObject.GetComponent<EnemyHealth>().HealthBar.SetActive(true);
            Destroy(gameObject);
            if (gameObject.name == "NormalArrow(Clone)")
            {
                if (target.GetComponent<EnemyHealth>().shipHealth > 0)
                {
                    Instantiate(hitEffect, target.position, Quaternion.identity); // hit particle for every hit
                    Destroy(GameObject.Find("HitParticle(Clone)"), 0.3f);
                }
                if (target.GetComponent<EnemyHealth>().shipHealth <= 0)
                {

                   
                    if (target.name == "GhostShip")
                    {
                        GameObject temp2 = Instantiate(GhostLoot, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity);
                        Instantiate(basicLootIdleParticle, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity, temp2.transform);

                        Instantiate(GhostExplosionEffect, target.position, Quaternion.identity);
                        Destroy(GameObject.Find("PatlamaParticleGhost(Clone)"), 1f);
                    }
                    else
                    {
                        Instantiate(explosionEffect, target.position, Quaternion.identity);
                        Destroy(GameObject.Find("PatlamaParticle(Clone)"), 1f);
                    }
                    GameObject temp = Instantiate(basicLoot, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity);
                    Instantiate(basicLootIdleParticle, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity, temp.transform);
                    Instantiate(explosionEffect, target.position, Quaternion.identity);
                    Destroy(GameObject.Find("PatlamaParticle(Clone)"), 1f);
                    PlayerAttackController.instance.inRange = false;
                }
            }
            else if (gameObject.name == "FireArrow(Clone)")
            {
                if (target.GetComponent<EnemyHealth>().shipHealth > 0)
                {
                    Instantiate(fireHitEffect, target.position, Quaternion.identity); // hit particle for every hit
                    Destroy(GameObject.Find("FireHit(Clone)"), 0.3f);
                    target.transform.GetChild(2).gameObject.SetActive(true);
                }
                if (target.GetComponent<EnemyHealth>().shipHealth <= 0)
                {
                    GameObject temp = Instantiate(basicLoot, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity);
                    Instantiate(basicLootIdleParticle, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity, temp.transform);
                    if (target.name=="GhostShip")
                    {
                        GameObject temp2 = Instantiate(GhostLoot, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity);
                        Instantiate(basicLootIdleParticle, new Vector3(target.position.x, basicLoot.transform.position.y, target.position.z), Quaternion.identity, temp2.transform);

                        Instantiate(GhostExplosionEffect, target.position, Quaternion.identity);
                        Destroy(GameObject.Find("PatlamaParticleGhost(Clone)"), 1f);
                    }
                    else
                    {
                        Instantiate(explosionEffect, target.position, Quaternion.identity);
                        Destroy(GameObject.Find("PatlamaParticle(Clone)"), 1f);
                    }
                    PlayerAttackController.instance.inRange = false;
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
