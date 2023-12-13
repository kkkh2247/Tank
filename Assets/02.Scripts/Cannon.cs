﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
    public float speed = 6000.0f;
    public GameObject expEffect;
    private CapsuleCollider _collider;
    private Rigidbody _rigidbody;
	// Use this for initialization
	void Start () {
        _collider = GetComponent<CapsuleCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * speed);
        StartCoroutine(this.ExplosionCannon(3.0f));
	}
	
    private void OnTriggerEnter()
    {
        StartCoroutine(this.ExplosionCannon(0.0f));
    }

    IEnumerator ExplosionCannon(float tm)
    {
        yield return new WaitForSeconds(tm);
        _collider.enabled = false;
        _rigidbody.isKinematic = true;
        GameObject obj = (GameObject)Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(obj, 1.0f);
        Destroy(this.gameObject, 1.0f);
    }
}
