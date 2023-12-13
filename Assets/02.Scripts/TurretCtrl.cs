using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretCtrl : MonoBehaviour {
    private Transform tr;
    private RaycastHit hit;
    public float rotSpeed = 5.0f;
    private PhotonView pv = null;
    private Quaternion currRot = Quaternion.identity;
	// Use this for initialization
	void Awake () {
        tr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        // Photon View의 Observed 속성을 이 스크립트로 저장
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        // offf : 실시간 데이터를 송수신 안함
        // RelibleDDeltaCompressed : 데이터를 정확히 송수신한다.(TCP)
        // Unreliable : 데이터의 정합성을 보장할 수 없지만 속도가 빠르다. (UDP)
        // UnreliableOnChange : Unrealiable 옵션과 같지만 변경사항이 발생했을 때만 전송
        currRot = tr.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
        if (pv.isMine)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100.0f, Color.green);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
            {
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                tr.Rotate(0, angle * Time.deltaTime * rotSpeed, 0);
            }
        }
        else
        {
            tr.localRotation = Quaternion.Slerp(tr.localRotation, currRot,Time.deltaTime* 3.5f);
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
