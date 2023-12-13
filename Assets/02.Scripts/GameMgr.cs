using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMgr : MonoBehaviour
{
    public Text txtConnect;
    private void Awake()
    {
        CreateTank();
        PhotonNetwork.isMessageQueueRunning = true;
        GetConnectPlayerCount(); // 룸 입장하면 기존 접속자 정보 출력
    }
    void GetConnectPlayerCount()
    {
        Room currRoom = PhotonNetwork.room;
        txtConnect.text = currRoom.playerCount.ToString() + "/" + currRoom.maxPlayers.ToString();
    }
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        print("a");
        GetConnectPlayerCount();
    }
    void OnPhotonPlayerDisconnected(PhotonPlayer outPlayer)
    {
        print("b");
        GetConnectPlayerCount();
    }
    public void OnClickExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        //GetConnectPlayerCount();
    }
    void OnLeftRoom()
    {
        Application.LoadLevel("scLobby");
    }
    void CreateTank(){
    float pos = Random.Range(-50f,50f);
    PhotonNetwork.Instantiate("Tank", new Vector3(pos, 10, pos), Quaternion.identity,0);
	}
}
