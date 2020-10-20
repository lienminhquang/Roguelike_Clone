using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private BoardManager boardManager;
    public int level = 3;

    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        boardManager = GetComponent<BoardManager>();
        boardManager.SetupScene(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
