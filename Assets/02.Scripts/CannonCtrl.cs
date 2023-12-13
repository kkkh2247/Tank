using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonCtrl : MonoBehaviour {
    private Transform tr;
    private PhotonView pv = null;
    public float rotSpeed = 100.0f;
    private Quaternion currRot = Quaternion.identity;

	// Use this for initialization
	void Awake () {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        currRot = tr.localRotation;
	}

	void Update () {
        if (pv.isMine)
        {
            float angle = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * rotSpeed;
            tr.Rotate(angle, 0, 0);
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation,currRot,Time.deltaTime*3.0f);
        }
      
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(tr.localRotation);

        }
        else
        {
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

}
