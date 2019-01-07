using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeedTest : MonoBehaviour
{
    private void Start()
    {
        Random.InitState(5);
        string s = "";
        for(int i = 0; i < 100; i++)
        {
            s += Random.Range(0, 10).ToString();
        }
        Debug.Log(s);

        Random.InitState(10);
        s = "";
        for (int i = 0; i < 100; i++)
        {
            s += Random.Range(0, 10).ToString();
        }
        Debug.Log(s);

        Random.InitState(5);
        s = "";
        for (int i = 0; i < 100; i++)
        {
            s += Random.Range(0, 10).ToString();
        }
        Debug.Log(s);
    }
}
