using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackAnim : MonoBehaviour {
    private float scrollSpeed = 3.0f;
    private Renderer _renderer;
	// Use this for initialization
	void Start () {
        _renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        var offset = Time.time * scrollSpeed * Input.GetAxisRaw("Vertical");

        _renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        _renderer.material.SetTextureOffset("_BumpMap", new Vector2(0, offset));
        //_MainTex, Diffuse, _BumpMap, Normal_Map, _Cube, Cubemap
	}
}
