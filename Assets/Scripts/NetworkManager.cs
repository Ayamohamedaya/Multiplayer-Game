using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;
using Hashtable = System.Collections.Hashtable;
using System.Linq;
using UnityEngine.UI;



public class NetworkManager : MonoBehaviourPunCallbacks
{
    const string gameVersion = "v1.0";
    static NetworkManager instance;
    [SerializeField] TMP_Text txt_log;
    MainMenuUIManager mainMenuUIMan;
    public const string SceneName_mainMenu = "MainMenu";
    public const string SceneName_gameplay = "Gameplay";
    int count = 0;
    //Properties
    public static NetworkManager Instance => instance;

    private bool PlayerReady = false;
    private ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();


    // bool isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["rdy"];
    //  Hashtable hash = new Hashtable();
  

   
    private bool AllPlayersReady
    {
        get
        {
           foreach (var photonPlayer in PhotonNetwork.PlayerList)
           {
                
               if ((bool)photonPlayer.CustomProperties["rdy"] == false)
                    return false;
           }

           return true;
        }
    }

  

    public void readyPlayers()
    {
        PlayerReady = true;
        _playerCustomProperties["rdy"] = PlayerReady;
        PhotonNetwork.SetPlayerCustomProperties(_playerCustomProperties);

        Log(PhotonNetwork.NickName + " checked ready");
        mainMenuUIMan.DisplayPanel(MainMenuPanel.Lobby_WaitingForPlayers);

        // hash.Add("rdy", isReady);
        // PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
    {
        if (scene.name == SceneName_mainMenu)
        {
            mainMenuUIMan = FindObjectOfType<MainMenuUIManager>();
        }

        ClearLog();
    }

    #region PunMethods

    public void Connect()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    } 

    public void CreateRoom(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Log("Room Name must have a value");
            return;
        }

        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        roomOptions.BroadcastPropsChangeToAll = true;

    }

    public void JoinRoom(string roomName, bool joinRandomRoom = false, bool createRoomIfNotFound = false)
    {
        if (joinRandomRoom)
        {
            PhotonNetwork.JoinRandomRoom();
            return;
        }

        if (string.IsNullOrEmpty(roomName))
        {
            Log("Room Name must have a value");
            return;
        }

        if (createRoomIfNotFound)
        {
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4 };
            TypedLobby typedLobby = new TypedLobby { Type = LobbyType.Default };
            PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby);
            roomOptions.BroadcastPropsChangeToAll = true;
        }
        else
        {
            PhotonNetwork.JoinRoom(roomName); 
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (!AllPlayersReady)
        {
            return;
        }
            PhotonNetwork.LoadLevel(sceneName);

        
    }
   

   

    #endregion

    #region PunCallbacks
  
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        mainMenuUIMan.DisplayPanel(MainMenuPanel.Lobby);
        Log("Connected");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Log(PhotonNetwork.CurrentRoom.Name + " room is Created");
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Log(PhotonNetwork.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
        mainMenuUIMan.DisplayPanel(MainMenuPanel.ready);
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Log(newPlayer.NickName + " joined room");
        
    }


    #endregion

    public void Log(string message)
    {
        txt_log.text = message + "\n" + txt_log.text;
    }

    public void ClearLog()
    {
        txt_log.text = string.Empty;
    }

  
}
