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
    public GameObject Decal;

    private int frameCount = 0;
    bool hittingEnemy = false;

    private void Update()
    {
        ++frameCount;

        if (frameCount > 10 * 60) Destroy(gameObject);
    }

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
                BossAI bossScript = col.gameObject.GetComponent<BossAI>();
                bossScript.TryTakeBulletDamage(Damage);
                enemyHitExplosion = bossScript.CurrentBulletExplosion();
                hittingEnemy = true;
                StartCoroutine(ScheduleNewDeath());
            }
            else
            {
                hittingEnemy = false;

                if(col.contacts.Length > 0)
                    Instantiate(Decal, col.contacts[0].point, Quaternion.FromToRotation(Vector3.up, col.contacts[0].normal));

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
                PlayerHealth ph = col.gameObject.GetComponent<PlayerHealth>();
                ph.TakeDamage(Mathf.Clamp((Damage - col.gameObject.GetComponent<PlayerHealth>().armor), 1.0f, 10000.0f));
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

        // play sound
        if (hittingEnemy) { Instantiate(enemyHitExplosion, transform.position, Quaternion.Euler(Vector3.zero)); }
        else { Instantiate(otherHitExplosion, transform.position, Quaternion.Euler(Vector3.zero)); }

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
