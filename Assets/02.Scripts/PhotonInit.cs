using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PhotonInit : MonoBehaviour
{
    public string version = "v1.0";
    public InputField userId;
    public InputField roomName;

    public GameObject scrollContens;
    public GameObject roomItem;

    void Awake()
    {
        if (!PhotonNetwork.connected)
        PhotonNetwork.ConnectUsingSettings(version);

        userId.text = GetUserId();
        roomName.text = "Room_" + Random.Range(0,999).ToString("000");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnJoinedLobby()
    {
        Debug.Log("Entered Lobby");
        userId.text = GetUserId();
        //PhotonNetwork.JoinRandomRoom(); //무작위 방에 입장
        //JoinRandomRoom(Hashtable 룸속성, byte 최대 접속자 수)
        //Hashtable roomProperties = new Hashtable() {{"map",1},"minLevel 10"};
        //PhotonNetwork.JoinRandomRoom(roomProperties.4);
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if(string.IsNullOrEmpty(userId)){
            userId = "User_" + Random.RandomRange(0,999).ToString("000");
        }
        return userId;
    } 

    void OnPhotonRandomJoinFailed()//무작위 접속에 실패시
    {
        Debug.Log("No rooms");
        PhotonNetwork.CreateRoom( "My Room");
    }
    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID",userId.text);
        PhotonNetwork.JoinRandomRoom();
    }
    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;
        if(string.IsNullOrEmpty(roomName.text)){
            _roomName = "Room_" + Random.Range(0, 999).ToString("000");
        }
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID",userId.text);

        PhotonNetwork.CreateRoom(_roomName);
    }
    void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Create Room Failed = " + codeAndMsg[1]);
    }
    void OnJoinedRoom()
    {

        Debug.Log("Enter Room");
        StartCoroutine(this.LoadBattleField());
     //   CreateTank();
    }


    IEnumerator LoadBattleField()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        AsyncOperation ao = Application.LoadLevelAsync("scBattleField");
        yield return ao;
    }

    /*
    void CreateTank()
    {
        float pos = Random.Range(-100.0f, 100.0f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 20.0f, pos), Quaternion.identity, 0);
    }
     * */
    void OnReceivedRoomListUpdate() // 생성된 룸 목록이 변경됐을 때 호출되는 콜백함수
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ROOM_ITEM"))
        {
            Destroy(obj);
        }
        int rowCount = 0;
        scrollContens.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            Debug.Log(_room.name);
            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContens.transform, false);
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.name;
            roomData.connectPlayer = _room.playerCount;
            roomData.maxPlayers = _room.maxPlayers;
            roomData.DispRoomData();

            // RoomItem의 Button 컴포넌트에 클릭 이벤트를 동적 연결
            roomData.GetComponent<UnityEngine.UI.Button>().
                onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });

            scrollContens.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContens.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }
    }
    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.player.name = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName);
    }
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
