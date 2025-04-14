using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerStats playerStats;


    private void Awake() {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        SaveManager.Load(playerStats);
        ResourceUIManager.Instance.RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
