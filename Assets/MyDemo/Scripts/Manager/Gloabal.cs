using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gloabal : MonoBehaviour
{
    //Global�ն�����������tagΪglobal��ʱ��ֻ��ȫ�̴���һ��
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
