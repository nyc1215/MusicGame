using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeNote : MonoBehaviour
{
    private Rigidbody rb;

    public int noteSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            rb.velocity = new Vector3(0, 0, -noteSpeed);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        if (transform.position.z > 50)
        {
            DestoryMyself();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MyGameManager.GetGameManagerInstance().playerLife < 10)
            {
                MyGameManager.GetGameManagerInstance().playerLife++;
            }
            GameObject.Find("UIPanel").GetComponent<GameUI>().LifeTextUpdate();
            DestoryMyself();
        }
    }

    private void DestoryMyself()
    {
        Destroy(gameObject);
    }
}
