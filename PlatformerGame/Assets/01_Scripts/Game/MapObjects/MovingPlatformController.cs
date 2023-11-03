using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    Vector3 dir = Vector3.left;
    [SerializeField]
    Vector3 initialPos;
    [SerializeField]
    float maxDistance;
    [SerializeField]
    float targetPosX;
    [SerializeField]
    float speed;
    [SerializeField]
    int waitFrameCount;
    bool playerStepUp;

    public Vector3 MoveDistancePerFrame { get; private set; }

    void Start()
    {
        transform.position = initialPos;
        targetPosX = transform.position.x + maxDistance * dir.x;

        StartCoroutine(Coroutine_Update());
    }

    IEnumerator Coroutine_Update()
    {
        while (true)
        {
            if ((targetPosX - transform.position.x) * dir.x <= 0f)
            {
                dir *= -1f;
                targetPosX = transform.position.x + maxDistance * dir.x;
                MoveDistancePerFrame = Vector3.zero;
                for (int i = 0; i < waitFrameCount; i++)
                {
                    yield return null;
                }
            }
            
            if (playerStepUp)
            {
                MoveDistancePerFrame = speed * Time.deltaTime * dir;
            }

            transform.Translate(speed * Time.deltaTime * dir);

            yield return null;
        }
    }

    public void OnPlayerGoUp()
    {
        MoveDistancePerFrame = Vector3.zero;
        playerStepUp = true;
    }

    public void OnPlayerGoDown()
    {
        playerStepUp = false;
        MoveDistancePerFrame = Vector3.zero;
    }
}
