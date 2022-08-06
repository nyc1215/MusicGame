using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNote : MonoBehaviour
{
    private Material material;
    private Collider collider_this;
    private Rigidbody rb;

    public int noteSpeed;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
        collider_this = GetComponent<Collider>();
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

        if (transform.position.z < 1.5f - transform.localScale.z)
        {
            DestoryMyself();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            material.color = new Color(material.color.r * 0.5f, material.color.g * 0.5f, material.color.b * 0.5f, material.color.a * 0.5f);
            Physics.IgnoreCollision(other, collider_this, true);

            if (MyGameManager.GetGameManagerInstance().playerShield > 0)
            {
                MyGameManager.GetGameManagerInstance().playerShield = 0;
                GameObject.Find("Player").GetComponent<PlayerControler>().UpdateColorForShield();
                GameObject.Find("UIPanel").GetComponent<GameUI>().ShieldUpdate();
            }
            else
            {
                if (MyGameManager.GetGameManagerInstance().playerLife > 0)
                {
                    MyGameManager.GetGameManagerInstance().playerLife--;
                    if (MyGameManager.GetGameManagerInstance().playerLife == 0)
                    {
                        MyGameManager.GetGameManagerInstance().isWin = false;
                        MyGameManager.GetGameManagerInstance().EndGame();
                    }
                }
                GameObject.Find("UIPanel").GetComponent<GameUI>().LifeTextUpdate();
            }
        }
    }

    private void DestoryMyself()
    {
        Destroy(gameObject);
    }
}
