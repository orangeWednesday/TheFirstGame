// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// //Unityの画面上で触れるキーボードを指定したらどの処理を実行するっていう機能を使いたい時にスクリプトに書くやつ
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class PlayerHPSaver : MonoBehaviour
// {
//     // [SerializeField, Header("体力")]
//     // private int _hp;
//     // [SerializeField, Header("ダメージ")]
//     // private GameObject _damageSE;

//     private int _hp;
//     public Player _player;
//     public Spike _spike;
//     public HolePlayer _holeplayer;
//     public int _spikeAttackPower;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         _player = FindObjectOfType<Player>();
//         _holeplayer = FindObjectOfType<HolePlayer>();

//         if(_player != null)
//         {
//         _hp = _player.GetHP();
//         }
//         if(_holeplayer != null)
//         {
//         _hp = _holeplayer.GetHP();;
//         }
//         _hp = PlayerPrefs.GetInt ("HP", _hp);

//         _spike = FindObjectOfType<Spike>();
//         _spikeAttackPower = _spike._attackPower;
//         _spikeAttackPower = PlayerPrefs.GetInt ("Spike", 1);
//     }

//       void OnDestroy()
//     {
//         // 他のシーンでも適応できるようにHPとSpikeの攻撃力を保存
//         PlayerPrefs.SetInt ("HP", _hp);
//         // PlayerPrefs.SetInt ("Spike", _spikeAttackPower);
//         // PlayerPrefs.Save ();
//     }

//     // public void Damage(int damage)
//     // {
//     //     _hp = Mathf.Max(_hp - damage, 0 );
//     //     _Dead();
//     //     Instantiate(_damageSE);
//     // }

//     public int GetHP()
//     {
//         return _hp;
//     }


//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
