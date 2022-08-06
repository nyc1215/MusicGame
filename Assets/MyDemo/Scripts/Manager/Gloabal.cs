using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloabal : MonoBehaviour
{
    //Global空对象的子物体的tag为global的时候只会全程存在一个
    private void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Global").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
