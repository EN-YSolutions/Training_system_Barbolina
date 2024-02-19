using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 rightTransform;
    [SerializeField] private Vector3 leftTransform;
    [SerializeField] private int speed;

    public Roads NowRoad => _nowRoad;

    private Roads _nowRoad;

    private void Start()
    {
        _nowRoad = Roads.LeftRoad;
        transform.position = leftTransform;
    }

    private void Update()
    {
        Input();
        Move();
    }

    private void Input()
    {
        if (UnityEngine.Input.GetAxis("Horizontal") != 0)
        {
            if (UnityEngine.Input.GetAxis("Horizontal") < 0 && _nowRoad != Roads.LeftRoad)
                _nowRoad = Roads.LeftRoad;
            else if (UnityEngine.Input.GetAxis("Horizontal") > 0 && _nowRoad != Roads.RightRoad)
                _nowRoad = Roads.RightRoad;
        }
    }

    private void Move()
    {
        if(_nowRoad == Roads.LeftRoad && transform.position.x >= leftTransform.x)
            transform.position += leftTransform * Time.deltaTime * speed;
        if (_nowRoad == Roads.RightRoad && transform.position.x <= rightTransform.x)
            transform.position += rightTransform * Time.deltaTime * speed;
    }

}

public enum Roads
{
    LeftRoad,
    RightRoad
}
