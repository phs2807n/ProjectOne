using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shovel : MonoBehaviour
{
    //float speed = -150.0f;

    //int index = 1;
    int count = 1;

    Vector3 rotVec;

    private void Awake()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        rotVec = Vector3.forward * 360 / count;

        //transform.Rotate(rotVec);

        transform.Translate(Vector3.up * 1.5f, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotVec);
    }
}
