using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomData : MonoBehaviour
{
    [HideInInspector]
    public string roomName = "";
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayers = 0;

    public Text textRoomName;
    public Text textConnectInfo;
    // Start is called before the first frame update
    public void DispRoomData()
    {
        textRoomName.text = roomName;
        textConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
