using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public float moveSpeed;
    public float upDownSpeed;
    private float moveValue;
    public bool isUp;
    private Vector3 target;
    private Material material;

    private void Awake()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        material.color = Color.green;
        isUp = false;
    }

    public void OnMove(InputValue value)
    {
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            moveValue = value.Get<float>();
        }
        else
        {
            moveValue = 0f;
        }
    }

    public void OnJump()
    {
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            isUp = !isUp;
        }

        if (!isUp)
        {
            GameObject.Find("Main Camera").SendMessage("Down", SendMessageOptions.RequireReceiver);
        }
        else
        {
            GameObject.Find("Main Camera").SendMessage("Up", SendMessageOptions.RequireReceiver);
        }
    }

    public void OnPause()
    {
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            GameObject.Find("UIPanel").GetComponent<GameUI>().GamePause();
        }
    }

    public void FixedUpdate()
    {
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            transform.Translate(new Vector3(moveValue * moveSpeed, 0, 0) * Time.fixedDeltaTime, Space.World);
        }
    }

    private void Update()
    {
        var step = upDownSpeed * Time.deltaTime;
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            if (!isUp)
            {
                target = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), -1f, 5.0f);
            }
            else
            {
                target = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), 3f, 5.0f);
            }
            transform.position = Vector3.MoveTowards(
                new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), transform.position.y, transform.position.z),
                target, step);
        }
        UpdateColorForShield();
    }

    public void UpdateColorForShield()
    {
        if (MyGameManager.GetGameManagerInstance().playerShield > 0)
        {
            material.color = Color.blue;
        }
        else
        {
            material.color = Color.green;
        }
    }
}
