using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
	public static DataManager DM;

	public int highScore;

    // Start is called before the first frame update
    void Start()
    {
    	if(DM == null) {
    		DontDestroyOnLoad(gameObject);
    		DM = this;
    	}
    	else if(DM != this) {
    		Destroy(gameObject);
    	}
    }

    public void SaveData() {
    	BinaryFormatter binForm = new BinaryFormatter(); // for encryption
    	FileStream file = File.Create(Application.persistentDataPath + "/highScore.dat"); // creates save file
    	gameData data = new gameData(); // container
    	data.highScore = this.highScore;
    	binForm.Serialize(file, data);
    	file.Close();
    }

    public void LoadData() {
    	if(File.Exists(Application.persistentDataPath + "/highScore.dat")) {
    		BinaryFormatter binForm = new BinaryFormatter();
    		FileStream file = File.Open(Application.persistentDataPath + "/highScore.dat", FileMode.Open);
    		gameData data = (gameData) binForm.Deserialize(file);
    		file.Close();
    		this.highScore = data.highScore;
    	}
    }
}

[Serializable]
class gameData {
	public int highScore;
}
