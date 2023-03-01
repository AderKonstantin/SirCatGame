using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairsTrigger : MonoBehaviour
{
    private Collider _collider;

    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (_collider.enabled)
        // {
        //      sugarcode
        // }
    }
}
