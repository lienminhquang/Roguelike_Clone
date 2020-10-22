using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject soundManager;

    // Start is called before the first frame update
    void Awake()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.instance == null)

            //Instantiate gameManager prefab
            Instantiate(gameManager);

        //Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
        if (SoundManager.instance == null)

            //Instantiate SoundManager prefab
            Instantiate(soundManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
