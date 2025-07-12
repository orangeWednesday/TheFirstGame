using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class HoleDoor : MonoBehaviour
{
    [SerializeField,Header("振動する時間")]
    private float _shakeTime;
    [SerializeField,Header("振動の大きさ")]
    private float _shakeMagnitude;
    [SerializeField,Header("揺れるドア")]
    private GameObject _shakeDoor;

    private Player _player;
    private Vector3 _initPos;
    private float _shakeCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _initPos = _shakeDoor.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if(collision.gameObject.CompareTag("Door"))
        {
            StartCoroutine(_Shake());
            Debug.Log("揺れる");
        }
    }
    IEnumerator _Shake()
    {
        Vector3 initPos = _shakeDoor.transform.position;

        while(_shakeCount < _shakeTime)
        {
            float x = initPos.x + Random.Range(-_shakeMagnitude,_shakeMagnitude);
            float y = initPos.y + Random.Range(-_shakeMagnitude,_shakeMagnitude);

            _shakeCount += Time.deltaTime;

            yield return null;
        }
        _shakeDoor.transform.position = initPos;
    }
}

