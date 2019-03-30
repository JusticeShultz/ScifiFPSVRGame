using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjectOnCall : MonoBehaviour
{
    public GameObject _object;
    public Vector3 Rotation;

    public void Create()
    {
        Instantiate(_object, transform.position, Quaternion.Euler(Rotation * Mathf.Deg2Rad));
    }
}
