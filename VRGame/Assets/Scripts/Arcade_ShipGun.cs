using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class Arcade_ShipGun : MonoBehaviour
{
    [Tooltip("Which hand do we fire from?")]
    public SteamVR_Input_Sources HandType;
    [Tooltip("What event makes us shoot?")]
    public SteamVR_Action_Boolean GrabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    [Tooltip("Which bullet prefab should we use?")]
    public GameObject Bullet;
    [Tooltip("How fast should our bullet fly?")]
    public float BulletFlySpeed = 20;
    [Tooltip("How fast can we fire?")]
    public float FiringSpeed;
    [Tooltip("Where does our shot fire from?")]
    public GameObject GunBarrel;
    public SteamVR_Action_Vibration hapticFlash = SteamVR_Input.GetAction<SteamVR_Action_Vibration>("Haptic");
    public bool IsAI = false;

    // Shots per second that this weapon may fire.
    private float shotcooldown = 0.1f;
    private GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("LeftHand");
    }

    void Update ()
    {
        ++shotcooldown;

        if (IsAI)
        {
            if (shotcooldown > FiringSpeed * 60)
            {
                shotcooldown = 0;
                GameObject bullet = Instantiate(Bullet, GunBarrel.transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().velocity = GunBarrel.transform.forward * -BulletFlySpeed;
            }

            //float px = Player.transform.position.x;
            //float py = Player.transform.position.y;
            //float pz = Player.transform.position.z;

            //transform.LookAt(new Vector3(px, py, pz));
            transform.LookAt(Player.transform.position);

            GetComponent<Rigidbody>().velocity = transform.forward * 1.5f;
        }
        else
        {
            if (GrabPinchAction.GetState(HandType))
            {
                if (shotcooldown > FiringSpeed * 60)
                {
                    shotcooldown = 0;
                    GameObject bullet = Instantiate(Bullet, GunBarrel.transform.position, transform.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = GunBarrel.transform.forward * -BulletFlySpeed;
                    hapticFlash.Execute(0, 0.1f, 100.0f, 25, HandType);
                }
            }
        }
    }
}
