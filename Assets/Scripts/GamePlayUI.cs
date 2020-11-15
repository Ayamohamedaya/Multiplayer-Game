using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
    static GamePlayUI instance;
    [SerializeField] Image win_panel;
    public static GamePlayUI Instance => instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayGameOver()
    {
        win_panel.gameObject.SetActive(true);
    }
}
