using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;



public class CoinSpawnerTester : MonoBehaviour
{
    public static CoinSpawnerTester Instance { get; private set; }
   // private string _fileName = "C:\\Users\\Bassel B\\Idle Clicker\\Assets\\Scripts\\Debug\\DebugLog.txt";
    private string _fileName = Application.dataPath + "/DebugLog.csv";

    private void Awake() {
        Instance = this;
    }

    void Start()
    {

        //if (File.Exists(_fileName)) {
        //    Debug.Log(_fileName + " already exists.");
        //    return;
        //}
        
    }

    public void AddALine(string message) {

        //sr.WriteLine("This is my file.");
        //sr.WriteLine("I can write ints {0} or floats {1}, and so on.",
        //    1, 4.2);
        //sr.Close();
        // Open the file to read from.
        using var writer = new StreamWriter(_fileName, true);
        //writer.WriteLine("this is a message,and another");
        writer.WriteLine(message);
    }
}
