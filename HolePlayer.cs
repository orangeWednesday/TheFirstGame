// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// //Unityの画面上で触れるキーボードを指定したらどの処理を実行するっていう機能を使いたい時にスクリプトに書くやつ
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;
// using UnityEngine.UI;

// public class HolePlayer : MonoBehaviour
// {
//     //使いたい変数の型を任意の変数の名前に変える作業、スクリプト上の道具生成所
    
//     //SerializeField
//     //「Unityの画面上で変数（数値やオブジェクトなど）を簡単に代入ができるもの
//     //下に「private/public 変数の型 任意の変数の名前」を入力する」
//     //Header
//     //「SerializeFieldの変数がなんの変数なのか名前をつけられるもの」
//     [SerializeField, Header("移動速度")]
//     private float _moveSpeed;
//     [SerializeField, Header("ジャンプ速度")]
//     private float _jumpSpeed;
//     [SerializeField, Header("体力")]
//     private int _hp;
//     [SerializeField, Header("無敵時間")]
//     private float _damageTime;
//     [SerializeField, Header("点滅時間")]
//     private float _flashTime;
//     [SerializeField, Header("ジャンプ")]
//     private GameObject _jumpSE;
//     [SerializeField, Header("ダメージ")]
//     private GameObject _damageSE;
//     [SerializeField, Header("看板の内容")]
//     private GameObject _signBoard;


//     //「public/private 変数の型（なんの種類を代入する？） 任意の変数の名前」を書いている
//     //まだ変数は代入していない
//     //Vector2
//     //「float（少数を代入できる型）が2つ代入できるもの。(x,y)座標や方向を扱うときに用いる。」
//     private Vector2 _inputDirection;
//     private Rigidbody2D _rigid;
//     private Animator _anim;
//     private SpriteRenderer _spriteRenderer;
//     private Spike _spike;
//     private GameObject _safeShoes;
//     private int _spikeAttackPower;
//     private float _originalMoveSpeed;
//     private float _originalJumpSpeed;
//     //bool
//     //「true or falseの値を設定できる変数、2パターンの処理が書ける
//     private bool _bJump;
//     private bool _signBoardHit;
//     // private bool _signBoardHit2;
//     private bool _read;


//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         //あるメソッドに限らずゲーム全体で使いたい変数を指定するところ？？
        
//         //GetComponent<>()
//         //「<>で指定したコンポーネントをオブジェクトからとってくる」
//         _originalMoveSpeed = _moveSpeed;
//     　　　_originalJumpSpeed = _jumpSpeed;
//         _rigid = GetComponent<Rigidbody2D>();
//         _anim = GetComponent<Animator>();
//         _spriteRenderer = GetComponent<SpriteRenderer>();
//         _bJump = false;
//     　　 _hp = PlayerPrefs.GetInt ("HP", _hp);


//         _spike = FindObjectOfType<Spike>();
//         _spikeAttackPower = _spike._attackPower;
//         _spikeAttackPower = PlayerPrefs.GetInt ("Spike", _spikeAttackPower);

//         _safeShoes = GameObject.Find("SafeShoes");

//         string currentSceneName = SceneManager.GetActiveScene().name;
//     }

//      // 削除時の処理
//     void OnDestroy()
//     {
//         // 他のシーンでも適応できるようにHPとSpikeの攻撃力を保存
//         PlayerPrefs.SetInt ("HP", _hp);
//         PlayerPrefs.Save ();
//         PlayerPrefs.SetInt ("Spike", _spikeAttackPower);
//         PlayerPrefs.Save ();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //updateメソッドの下に書いたメソッドの名前を書いてゲーム起動中に実行してもらう所
//         _Move();
//         Debug.Log(_hp);
//         _LookMoveDirec();
//         _HitFloor();
//     }

    
//     //何をして欲しいか書く所、メソッド生成所
    
//     //「public/private 型の種類　任意のメソッドの名前」とかいてメソッドの中身を書いていく
//     private void _Move()
//     {
//         if(_bJump)return;
//         //linearVelocity
//         //「オブジェクトに加えられる力の大きさと方向を入れる時に使う変数」
//         //inputDirection.x（プレイヤーが入力した横軸の方向キー）×（上記の移動速度で指定した数字）、_rigid.linearVelocity.y（今までと変わらない上下方向）
//         _rigid.linearVelocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.linearVelocity.y);
//         _anim.SetBool("Walk", _inputDirection.x != 0.0f);
//     }

//     private void _LookMoveDirec()
//     {
//         if(_inputDirection.x > 0.0f)
//         {
//             transform.eulerAngles = Vector3.zero;
//         }
//         else if(_inputDirection.x < 0.0f)
//         {
//             transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
//         }
//     }

    
//     //OnCollisionEnter2D
//     //当たり判定を持っている指定したオブジェクトに衝突した時に実行されるメソッド

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if(collision.gameObject.tag =="SafeShoes")
//         {
//             _SpikeInvaild(collision.gameObject);
//         }

//         else if (collision.gameObject.tag == "Goal")
//         {
//             FindObjectOfType<MainManager>()._ShowGameClearUI();
//             enabled = false;
//             GetComponent<PlayerInput>().enabled = false;
//         }

//         else if (collision.gameObject.CompareTag("Door"))
//         {
//              _HitDoor(collision.gameObject);
//         }
    

//     // 衝突したオブジェクトが "VanishFloor" タグを持っている場合
//     if (collision.gameObject.CompareTag("VanishFloor"))
// 　　 {
//     Destroy(collision.gameObject);
//     }
//      // 衝突したオブジェクトが "VanishFloor" タグを持っている場合
//     if (collision.gameObject.CompareTag("TrapFloor"))
//     {
//         // 衝突したオブジェクト (VanishFloor) を削除する
//         Destroy(collision.gameObject);
    
//     }

//     }

//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         if (collision.gameObject.tag =="SignBoard")
//         {
//             _signBoardHit = false;
//             // _signBoardHit2 = false;
//         }
//     }


//     private void _HitFloor()
//     {
//             int layerMask = LayerMask.GetMask("Floor");
//             Vector3 rayPos = transform.position - new Vector3 (0.0f, transform.lossyScale.y / 2.0f + 0.35f);
//             Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f,0.1f);
//             RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
            
//             if(rayHit.transform == null)
//             {
//                 _bJump = true;
//                 _anim.SetBool("Jump", _bJump);
//                 return;
//             }

//             if(rayHit.transform.tag == "Floor" && _bJump)
//             {
//                 _bJump = false;
//                _anim.SetBool("Jump",_bJump);
//             }
//     }

//      private void _HitDoor(GameObject door)
// {
//     Destroy(door);

// }    
//     public void _SpikeInvaild(GameObject safeShoes)
//     {
//        if(_spikeAttackPower != 0) 
//         {
//         Debug.Log("安全靴ゲット");
//         _spikeAttackPower = 0;
//         Destroy(safeShoes);
//         Debug.Log("トゲの攻撃力は01");
//         }

//         else if(_spikeAttackPower == 0) 
//         {
//         Debug.Log("トゲの攻撃力は02");
//         }
//         else if(_spikeAttackPower == null)
//         {
//         Debug.Log("トゲの攻撃力は03");
//         }

//     }


//     public void OnRead(InputAction.CallbackContext context)
//     {
//         if(!context.performed || !_signBoardHit ) return;

//         else if(context.performed && !_read && _signBoardHit )
//         {
//         _signBoard.SetActive(true);
//         _read = true;
//         _moveSpeed = 0;
//         _jumpSpeed = 0;
//         //Instantiate(_signBoardSE);
//         }

//         else if(context.performed && _read && _signBoardHit)
//         {
//         _signBoard.SetActive(false);
//         _read = false;
//         _bJump = false;
//         _signBoardHit = false;
//         _moveSpeed = _originalMoveSpeed;
//         _jumpSpeed = _originalJumpSpeed;
//         //Instantiate(_signBoardSE);
//         }

//         // else if(context.performed && !_read && !_signBoardHit1 && _signBoardHit2 )
//         // {
//         // _signBoard2.SetActive(true);
//         // _read = true;
//         // _moveSpeed = 0;
//         // _jumpSpeed = 0;
//         // //Instantiate(_signBoardSE);
//         // }

//         // else if(context.performed && _read && !_signBoardHit1 && _signBoardHit2 )
//         // {
//         // _signBoard2.SetActive(false);
//         // _read = false;
//         // _bJump = false;
//         // _signBoardHit1 = false;
//         // _signBoardHit2 = false;
//         // _moveSpeed = _originalMoveSpeed;
//         // _jumpSpeed = _originalJumpSpeed;
//         // //Instantiate(_signBoardSE);
//         // }

//     }


//     IEnumerator _Damage()
//     {
//         Color color = _spriteRenderer.color;
//         for(int i = 0; i < _damageTime; i ++)
//         {
//             yield return new WaitForSeconds(_flashTime);
//             _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

//             yield return new WaitForSeconds(_flashTime);
//             _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
//         }
//         _spriteRenderer.color = color;
//         gameObject.layer = LayerMask.NameToLayer("Default");
//     }

//     private void _Dead()
//     { 
//         if (_hp <= 0)
//         {
//             Destroy(gameObject);
//         }
//     }

//     private void OnBecameInvisible()
//     {
//         Camera camera = Camera.main;
//         if(camera.name =="Main Camera" && camera.transform.position.y > transform.position.y)
//         {
//             Destroy(gameObject);
//         }
//     }

//     //プレイヤーが入力した情報をスクリプト上の変数に変換するメソッド
//     //カッコ内に書いた変数（引数）
//     //メソッドを実行する際に値を入れられる変数,メソッドないでしか使わない変数として扱うこともある。
//     //(（入れたい値）　変数の任意の名前)を入力する

//     //InputAction.CallbackContext
//     //「プレイヤーが入力した情報をとってくる変数」
//     public void _OnMove(InputAction.CallbackContext context)
//     {
//         //ReadValue<>()
//         //「InputAction.CallbackContextでとってきた変数を<>の型のに変えて代入することができる」
//         _inputDirection = context.ReadValue<Vector2>();
//     }

//     public void OnJump(InputAction.CallbackContext contect)
//     {
//         //if（条件）{どのような処理をするのか}
//         //!〜「〜ではない」
//         //||「または」
//         //return
//         //「if文の条件に当てはまるときは、return以降に書かれた処理は実行しない」
//         //performed
//         //「プレイヤーがゲームパッドを操作中」
//         if(!contect.performed || _bJump) return;

//         //AddForce（力を加えたい方向(x,y,zそれぞれ代入可),適用する力のタイプ）
//         //オブジェクトを加速させる処理
//         //Vector2.up「上方向」
//         //ForceMode2D.Force
//         //「初速遅い、徐々に加速　ex)車、電車」
//         //ForceMode2D.Impulse
//         //「初速速い、徐々に減速　ex)ボールを投げた時」
//         _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
//         Instantiate(_jumpSE);
//         //_bJump = true;
//         //_anim.SetBool("Jump",_bJump);
//     }

//     public void Damage(int damage)
//     {
//         _hp = Mathf.Max(_hp - damage, 0 );
//         _Dead();
//         Instantiate(_damageSE);
//     }

//     public int GetHP()
//     {
//         return _hp;
//     }

// }