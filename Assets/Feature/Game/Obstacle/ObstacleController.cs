using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public event Action EndMove = delegate{ };
    private float endZ = 5f;

    Vector3 startPos;
    Vector3 endPos;

    private void Awake()
    {
        startPos = transform.position;
        endPos = transform.position;
        endPos.z = endZ;
    }
    public void Init( int time)
    {
        transform.position = startPos;
        StartCoroutine(MoveCoroutine(time));
    }

    private IEnumerator MoveCoroutine(float time)
    {
        float t = 0f;

        while (transform.position.z > endZ)
        { 
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return null;
        }

        EndMove.Invoke();
    }
}
