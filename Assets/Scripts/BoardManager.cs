using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int col = 8;
    public int row = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTitles;
    public GameObject[] wallTitles;
    public GameObject[] foodTitles;
    public GameObject[] enemyTitles;
    public GameObject[] outerWallTitles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();
        for(int i = 1; i < col; i++)
        {
            for (int j = 1; j < row; j++)
            {
                gridPositions.Add(new Vector3(i, j, 0.0f));
            }
        }
    }

    void BoardSetup()
    {
        for (int i = -1; i < col + 1; i++)
        {
            for (int j = -1; j < row + 1; j++)
            {
                GameObject toInitiate = floorTitles[Random.Range(0, floorTitles.Length)];
                if(i == -1 || i == col || j == -1 || j == row)
                {
                    toInitiate = outerWallTitles[Random.Range(0, outerWallTitles.Length)];
                }

                GameObject instance = Instantiate(toInitiate, new Vector3(i, j, 0.0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPos = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPos;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int count = Random.Range(min, max);
        for (int i = 0; i < count; i++)
        {
            Vector3 pos = RandomPosition();
            GameObject toInstantiate = tileArray[Random.Range(0, tileArray.Length)];
            GameObject obj = Instantiate(toInstantiate, pos, Quaternion.identity);
            obj.transform.SetParent(boardHolder);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTitles, wallCount.maximum, wallCount.maximum);
        LayoutObjectAtRandom(foodTitles, foodCount.maximum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2.0f);
        LayoutObjectAtRandom(enemyTitles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(col - 1, row - 1.0f, 0.0f), Quaternion.identity);
    }
}
