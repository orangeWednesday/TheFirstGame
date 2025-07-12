using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Unityの画面上で触れるキーボードを指定したらどの処理を実行するっていう機能を使いたい時にスクリプトに書くやつ
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DestroyDoor : MonoBehaviour
{
    [SerializeField, Header("ペアのドア")]
    private GameObject _pairDoor;
    [SerializeField, Header("揺れの大きさ")]
    public float shakeAmplitude;
    [SerializeField, Header("揺れの速さ")]
    public float shakeSpeed;
    [SerializeField, Header("揺れ時間")]
    public float _shakingLength;

    private Vector3 initialPosition;
    private bool hasShaken = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.localPosition;
        hasShaken = false;
    }

    // Update is called once per frame
    void Update()
    {
        // hasshakenのbool値をif関数に追加したら、バグは修正された）
        // バグの原因はtime.timeにありそう
        if(_pairDoor == null && !hasShaken)
        {
        StartCoroutine(BrokenDoor());
        hasShaken = true;
        }
    }

    private IEnumerator BrokenDoor()
    { 
        float timer = 0f;

    while (timer < _shakingLength)
    {
        float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmplitude;
        transform.localPosition = initialPosition + new Vector3(offsetX, 0f, 0f);

        timer += Time.deltaTime;
        yield return null; // 次のフレームまで待つ
    }
        // // 処理が終わった後、バグみたいに揺れる
        // float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmplitude;
        // transform.localPosition = initialPosition + new Vector3(offsetX, 0f, 0f);
        // yield return new WaitForSeconds(_shakingLength);
        // // // 元の位置に戻る
        transform.localPosition = initialPosition;

    }
}
