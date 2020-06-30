using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;

public class GameDataManager : MonoBehaviour {

    public static GameDataManager GDM;
    private bool FirstTimeOpeningGameLoadedData = false;
    public List<SquareInfo> squares;
    public int TotalPoints_AllTime;
    public int HighScore_AllTime;
    public int currentPoints;
    public int currentClears;
    public int currentClearCounter;
    public int currentSwitches;
    public int moveCounter;
    public List<int> savedNextSquares;
    public bool gameOver;

    private void Awake() {
        //Debug.Log(Application.persistentDataPath);
        PlayerPrefs.DeleteAll();

        int firstTime = PlayerPrefs.GetInt("firstTime", 0);
        if (firstTime == 0) {
            PlayerPrefs.SetInt("firstTime", 1);
            Debug.Log("First Time Opening Game!");
            ResetGameData();
            FirstTimeOpeningGameLoadedData = true;
        }

        if (GDM == null) {
            DontDestroyOnLoad(gameObject);
            GDM = this;
            if (!FirstTimeOpeningGameLoadedData) {
                LoadGameData();
            }
        }

        else if (GDM != this) {
            Destroy(gameObject);
        }

    }
    

    public void SaveGameData() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/NumberMatching.dat");
        GameData data = new GameData();

        data.TotalPoints_AllTime = TotalPoints_AllTime;
        data.HighScore_AllTime = HighScore_AllTime;
        data.currentPoints = currentPoints;
        data.currentClears = currentClears;
        data.currentClearCounter = currentClearCounter;
        data.currentSwitches = currentSwitches;
        data.moveCounter = moveCounter;
        data.squares = squares;
        data.savedNextSquares = savedNextSquares;
        data.gameOver = gameOver;


        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saved Game Data To Local File");
    }

    public void LoadGameData() {
        if (File.Exists(Application.persistentDataPath + "/NumberMatching.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/NumberMatching.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();

            TotalPoints_AllTime = data.TotalPoints_AllTime;
            HighScore_AllTime = data.HighScore_AllTime;
            currentPoints = data.currentPoints;
            currentClears = data.currentClears;
            currentClearCounter = data.currentClearCounter;
            currentSwitches = data.currentSwitches;
            moveCounter = data.moveCounter;
            squares = data.squares;
            savedNextSquares = data.savedNextSquares;
            gameOver = data.gameOver;

            Debug.Log("Loaded Game Data from Local File");
        }
        else {
            Debug.Log("No Game Data found re-creating Game Data");
            ResetGameData();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ResetGameData() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/NumberMatching.dat");
        GameData data = new GameData();

        data.TotalPoints_AllTime = 0;
        data.HighScore_AllTime = 0;
        data.currentPoints = 0;
        data.currentClears = 0;
        data.currentClearCounter = 1;
        data.currentSwitches = 3;
        data.moveCounter = 0;

        data.squares = squares;
        //reset all square info
        for (int i = 0; i < squares.Count; i++) {
            data.squares[i].number = 0;
            data.squares[i].completed = false;
            data.squares[i].blocker = false;
            data.squares[i].adjescentConnections = new List<bool> { false, false, false, false};
            data.squares[i].luckyCoin = false;
        }

        data.savedNextSquares = new List<int> { UnityEngine.Random.Range(1,4), UnityEngine.Random.Range(1, 4), UnityEngine.Random.Range(1, 4) };
        data.gameOver = false;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Reset Game Data in Local File");
        LoadGameData();
    }

}

[Serializable]
class GameData {
    public int TotalPoints_AllTime;
    public int HighScore_AllTime;
    public int currentPoints;
    public int currentClears;
    public int currentClearCounter;
    public int currentSwitches;
    public int moveCounter;
    public List<SquareInfo> squares;
    public List<int> savedNextSquares;
    public bool gameOver;
}

[Serializable]
public class SquareInfo {
    public int number;
    public bool completed;
    public bool blocker;
    public List<bool> adjescentConnections;
    public bool luckyCoin;
}