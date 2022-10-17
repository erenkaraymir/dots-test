using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1.5f;
            Debug.Log("Time 2");
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            Time.timeScale = 1;
            Debug.Log("Time 1");
        }
    }
}
