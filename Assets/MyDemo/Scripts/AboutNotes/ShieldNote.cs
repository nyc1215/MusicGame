using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldNote : MonoBehaviour
{
    private Rigidbody rb;

    public int noteSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
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
            MyGameManager.GetGameManagerInstance().playerShield = 5;
            GameObject.Find("Player").GetComponent<PlayerControler>().UpdateColorForShield();
            GameObject.Find("UIPanel").GetComponent<GameUI>().ShieldUpdate();
            DestoryMyself();
        }
    }

    private void DestoryMyself()
    {
        Destroy(gameObject);
    }
}
