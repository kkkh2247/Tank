﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class TankMove : MonoBehaviour {

    public float moveSpeed = 20.0f;
    public float rotSpeed = 50.0f;
    private Rigidbody rbody;
    private Transform tr;
    private float h, v;
    private PhotonView pv = null;
    public Transform camPivot;// 메인카메라
    // 위치 정보를 송수신할 변수
    private Vector3 currPos = Vector3.zero;
    private Quaternion currRot = Quaternion.identity;

	// OnPhotonSerializeView가 호출되기 전에 tr변수에 transform 이 할당되기 위해
	void Awake () { 
        rbody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        // 데이터 전송 타입설정
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        pv.ObservedComponents[0] = this;

        if(pv.isMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
            rbody.centerOfMass = new Vector3(0.0f, -0.5f, 0.0f);
        }
        else
        {
            rbody.isKinematic = true;
            //원격 네트워크 플레이어의 탱크는 물리력을 사용안함
        }
        currPos = tr.position;
        currRot = tr.rotation;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting){
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (pv.isMine)
        {
            //자신이 아닌경우 키조작 안함
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            tr.Rotate(Vector3.up * rotSpeed * h * Time.deltaTime);
            tr.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position,currPos,Time.deltaTime * 3.5f);
            tr.rotation = Quaternion.Slerp(tr.rotation,currRot,Time.deltaTime * 3.5f);
        }


       
	}
}
