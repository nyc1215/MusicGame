using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Vector3 targetPos;
    private float targetRotate;
    public float upDownSpeed;
    public float rotateSpeed;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Down();
    }

    public void Up()
    {
        targetPos = new Vector3(0f, 2.2f, 1.5f);
        targetRotate = -15.0f;
    }

    public void Down()
    {
        targetPos = new Vector3(0f, 0.2f, 1.5f);
        targetRotate = 15.0f;
    }

    private void Update()
    {
        var stepMove = upDownSpeed * Time.deltaTime;
        var stepRotate = rotateSpeed * Time.deltaTime;
        if (!MyGameManager.GetGameManagerInstance().isPause)
        {
            transform.SetPositionAndRotation(
              Vector3.MoveTowards(transform.position, targetPos, stepMove),
              Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotate, 0, 0), stepRotate)
          );
        }
    }
}