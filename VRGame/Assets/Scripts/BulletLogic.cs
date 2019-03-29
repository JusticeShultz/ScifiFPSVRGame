using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public int Damage = 25;
    public bool IsEnemyShot = false;
    [Tooltip("Particle effect for hitting enemy")]
    public GameObject enemyHitExplosion;
    [Tooltip("Particle effect for hitting anything other than enemy")]
    public GameObject otherHitExplosion;

    bool hittingEnemy = false;

    void OnCollisionEnter(Collision col)
    {
        if (!IsEnemyShot)
        {
            if (col.gameObject.name == "Spitter")
            {
                //Do damage to it
                col.gameObject.GetComponent<SpitterAI>().Health -= Damage;
                hittingEnemy = true;
                StartCoroutine(ScheduleNewDeath());
            }
            else if (col.gameObject.name == "Leech")
            {
                //Do damage to it
                col.gameObject.GetComponent<LeechAI>().Health -= Damage;
                hittingEnemy = true;
                StartCoroutine(ScheduleNewDeath());
            }
            else if(col.gameObject.name == "RiggedThresher")
            {
                //Do damage to it
                col.gameObject.GetComponent<BossAI>().TryTakeBulletDamage(Damage);
                hittingEnemy = true;
                StartCoroutine(ScheduleNewDeath());
            }
            else
            {
                hittingEnemy = false;
                if (col.gameObject.GetComponent<DestroyableEntity>() != null)
                {
                    col.gameObject.GetComponent<DestroyableEntity>().Health -= Damage;
                    StartCoroutine(ScheduleNewDeath());
                }
                else StartCoroutine(ScheduleNewDeath());
            }
        }
        else
        {
            if (col.gameObject.name == "PlayerCollider")
            {
                //Do damage to it
                col.gameObject.GetComponent<PlayerHealth>().CurrentHealth -= Mathf.Clamp((Damage - col.gameObject.GetComponent<PlayerHealth>().Armor), 1.0f, 10000.0f);
                Destroy(gameObject);
            }
            else Destroy(gameObject);
        }
    }

    // delete bullet
    private IEnumerator ScheduleNewDeath()
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<LineRenderer>());
        Destroy(GetComponent<SphereCollider>());

        if (hittingEnemy) { Instantiate(enemyHitExplosion, transform.position, Quaternion.Euler(Vector3.zero)); }
        else { Instantiate(otherHitExplosion, transform.position, Quaternion.Euler(Vector3.zero)); }
        print("explosion");

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
