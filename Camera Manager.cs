using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField,Header("振動する時間")]
    private float _shakeTime;
    [SerializeField,Header("振動の大きさ")]
    private float _shakeMagnitude;

    private Player _player;
    private Vector3 _initPos;
    private float _shakeCount;
    private int _currentPlayerHP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _currentPlayerHP = _player.GetHP();
        _initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _ShakeCheck();
        _FollowPlayer();
    }
    private void _ShakeCheck()
    {
        if(_currentPlayerHP != _player.GetHP())
        {
            _currentPlayerHP = _player.GetHP();
            StartCoroutine(_Shake());
        }
    }
    IEnumerator _Shake()
    {
        Vector3 initPos = transform.position;

        while(_shakeCount < _shakeTime)
        {
            float x = initPos.x + Random.Range(-_shakeMagnitude,_shakeMagnitude);
            float y = initPos.y + Random.Range(-_shakeMagnitude,_shakeMagnitude);

            _shakeCount += Time.deltaTime;

            yield return null;
        }
        transform.position = initPos;
    }
    private void _FollowPlayer()
    {
        if(_player ==null) return;
    　　 float x = _player.transform.position.x; // ← プレイヤーのX位置を追いかける
        float y = _initPos.y;                   // ← カメラの初期Y位置で固定
        float z = transform.position.z;         // ← Zは通常カメラの固定距離

    transform.position = new Vector3(x, y, z);
        // float x = _player.transform.position.x;
        // x = Mathf.Clamp(x, _initPos.x, Mathf.Infinity);
        // transform.position = new Vector3 (x,transform.position.y, transform.position.z);
    }
}
