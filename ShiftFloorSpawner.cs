using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShiftFloorSpawner : MonoBehaviour
{
    [SerializeField, Header("フロアオブジェクト")]
    private  GameObject _shiftFloor;

    [SerializeField, Header("消えるフロア")]
    private GameObject _trapFloor;

    private GameObject _shiftFloorObj;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shiftFloorObj = this.gameObject;
　　　　　// 破壊対象のオブジェクトを監視
        if (_trapFloor != null)
        {
            // 破壊対象のオブジェクトが破壊されたら、GenerateObjectメソッドを呼び出す
            StartCoroutine(WatchForDestruction());
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }


　 IEnumerator WatchForDestruction()
    {
        // objectToDestroy が破壊されるまで待つ
        while (_trapFloor != null)
        {
            yield return null; // 1フレーム待機
        }

        // objectToDestroy が破壊されたら、objectToSpawn を生成
        _SpawShiftFloor();
    }
private void _SpawShiftFloor()

    {
    _shiftFloorObj = Instantiate(_shiftFloor);
    _shiftFloorObj.transform.position = transform.position;
    }

}

