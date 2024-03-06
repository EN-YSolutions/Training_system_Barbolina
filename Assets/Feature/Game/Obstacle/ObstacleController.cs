using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public event Action EndMove = delegate{ };

    [SerializeField] private Transform rightRoad;
    [SerializeField] private Transform centralRoad;
    [SerializeField] private Transform leftRoad;

    [HideInInspector] public int Speed = 1;

    private float endZ = 5f;
    private float endRightZ = -3f;

    Vector3 startPos;
    Vector3 endPos;

    private Transform _changeRoad;

    private void Awake()
    {
        startPos = transform.position;
    }
    public void Init( int time)
    {
        transform.position = startPos;
        endPos = transform.position;
        endPos.z = endZ;
        StartCoroutine(MoveCoroutine(time));
    }

    private IEnumerator MoveCoroutine(float time)
    {
        float t = 0f;

        while (transform.position.z > endZ)
        { 
            t += Time.deltaTime * Speed;
            transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return null;
        }

        EndMove.Invoke();
    }

    private Vector3 endPositionForRightEnd;

    public IEnumerator RightEnd(float time)
    {
        float t = 0f;

        endPos = transform.position;
        endPos.z = endRightZ;
        endPositionForRightEnd = transform.position;

        while (transform.position.z > endRightZ)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(endPositionForRightEnd, endPos, t / time);
            yield return null;
        }
        _changeRoad.Rotate(0, 0, 90);
    }

    public void OpenFence(Roads road)
    {
        switch (road)
        {
            case Roads.RightRoad:
                _changeRoad = rightRoad;
                break;
            case Roads.CentralRoad:
                _changeRoad = centralRoad;
                break;
            case Roads.LeftRoad:
                _changeRoad = leftRoad;
                break;
        }
        _changeRoad.Rotate(0, 0, -90);
    }
}
