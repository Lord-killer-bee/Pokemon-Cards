using UnityEngine;
using System.Collections;

public class RoomMan : Photon.MonoBehaviour {

    public string verNum = "0.1";
    public string roomName = "room01";

    // Use this for initialization
    void Start () {
        PhotonNetwork.ConnectUsingSettings(verNum);
        Debug.Log("Starting Connection!");
    }
	
    public void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(roomName, null, null);
        Debug.Log("Starting Server!");
    }

}
