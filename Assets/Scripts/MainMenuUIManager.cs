using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum MainMenuPanel
{
    Connection,
    Lobby,
    ready,
    Lobby_WaitingForPlayers,
    
}

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject Panel_connection;
    [SerializeField] GameObject Panel_lobby;
    [SerializeField] GameObject Panel_lobby_waitingForPlayers;
    [SerializeField] TMP_InputField _playerName;
    [SerializeField] TMP_InputField _roomName;
    [SerializeField] GameObject panel_ready;
    static MainMenuUIManager instance;
   public string playerName;
    public static MainMenuUIManager Instance=>instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DisplayPanel(MainMenuPanel.Connection);
    }

    public void DisplayPanel(MainMenuPanel panel)
    {
        Panel_connection.SetActive(false);
        Panel_lobby.SetActive(false);
        Panel_lobby_waitingForPlayers.SetActive(false);
        panel_ready.SetActive(false);
        switch (panel)
        {
            case MainMenuPanel.Connection:
                Panel_connection.SetActive(true);
                break;
            case MainMenuPanel.Lobby:
                Panel_lobby.SetActive(true);
                break;
            
            case MainMenuPanel.ready:
                panel_ready.SetActive(true);
                break;
                case MainMenuPanel.Lobby_WaitingForPlayers:
                  //Panel_lobby.SetActive(true);
                  Panel_lobby_waitingForPlayers.SetActive(true);
                  break;
        }
    }

    public void Connect()
    {
        NetworkManager.Instance.Connect();
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_playerName.text))
        {
            return;
        }
        Photon.Pun.PhotonNetwork.NickName = _playerName.text;
        playerName = _playerName.text;

        NetworkManager.Instance.CreateRoom(_roomName.text);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(_playerName.text))
        {
            return;
        }
        Photon.Pun.PhotonNetwork.NickName = _playerName.text;
       playerName= _playerName.text;
        NetworkManager.Instance.JoinRoom(_roomName.text);
    }

    public void ClickReady()
    {
        NetworkManager.Instance.readyPlayers();
        
    }

    public void StartGame()
    {
        
        NetworkManager.Instance.LoadScene(NetworkManager.SceneName_gameplay);

    }
  
}
