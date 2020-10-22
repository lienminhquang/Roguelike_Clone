using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardManager;
    private int level = 0;
    public int Level { 
        get { return level; } 
        set 
        {
            level = value;
        } 
    }
    public int playerFoodPoint = 100;
    public bool playerTurn = true;
    public float turnDelay = 0.1f;
    public float startLevelDelay = 3.0f;
    private List<Enemy> enemyList;
    private bool enemyMoving;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup = true;
    bool initialized = false;

    
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            //return;
        }
        DontDestroyOnLoad(gameObject);

        boardManager = GetComponent<BoardManager>();
        

        enemyList = new List<Enemy>();
        level++;
        InitGame();
    }

    private void OnDestroy()
    {
        print("Destroy");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        print("load");

        instance.level++;
        instance.InitGame();
    }

    private void InitGame()
    {
        
            doingSetup = true;
            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Day " + level;
            levelImage.SetActive(true);
            Invoke("HideLevelImage", startLevelDelay);
            enemyList.Clear();
            boardManager.SetupScene(level);
           
    }

    void HideLevelImage()
    {
        doingSetup = false;
        levelImage.SetActive(false);
    }

    internal void AddEnemyToList(Enemy gameObject)
    {
        enemyList.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTurn || enemyMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    private IEnumerator MoveEnemies()
    {
        enemyMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if(enemyList.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach (var item in enemyList)
        {
            item.MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }
        playerTurn = true;
        enemyMoving = false;
    }

    public void DestroyUnits()
    {
        boardManager.DestroyAllUnit();
        print("destroy");
    }

    public void GameOver()
    {
        levelText.text = "You done :)";
        levelImage.SetActive(true);
        enabled = false;
        enemyList.Clear();
    }


}
