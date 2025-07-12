using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Unityの画面上で触れるキーボードを指定したらどの処理を実行するっていう機能を使いたい時にスクリプトに書くやつ
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //使いたい変数の型を任意の変数の名前に変える作業、スクリプト上の道具生成所
    
    //SerializeField
    //「Unityの画面上で変数（数値やオブジェクトなど）を簡単に代入ができるもの
    //下に「private/public 変数の型 任意の変数の名前」を入力する」
    //Header
    //「SerializeFieldの変数がなんの変数なのか名前をつけられるもの」
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
     private float _jumpSpeed;
     [SerializeField, Header("体力")]
     private int _hp;
     [SerializeField, Header("体力初期値")]
     private int _starthp;
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;
    [SerializeField, Header("ジャンプ")]
    private GameObject _jumpSE;
    [SerializeField, Header("ダメージ")]
    private GameObject _damageSE;
    [SerializeField, Header("ダメージ笑い声")]
    private GameObject _damageLaughingSE;
    [SerializeField, Header("電話音")]
    private GameObject _phoneCall;
    [SerializeField, Header("ジョンウィックのセリフ")]
    private GameObject _phoneCallScipt;
    [SerializeField, Header("ジョンウィックのスクリプト時間")]
    private float uiDisplayTime; 
    [SerializeField, Header("ジョンウィックの電話時間")]
    private float _phoneCallLength;
    [SerializeField, Header("ジョンウィックの銃弾")]
    private GameObject _bulletObj;
    [SerializeField, Header("当たり判定の微調整")]
    private float _adjustHitBox;
    [SerializeField, Header("安全靴のメッセージ")]
    private GameObject _SafeMessage;

    

    
   
    //「public/private 変数の型（なんの種類を代入する？） 任意の変数の名前」を書いている
    //まだ変数は代入していない
    //Vector2
    //「float（少数を代入できる型）が2つ代入できるもの。(x,y)座標や方向を扱うときに用いる。」
    private Vector2 _inputDirection;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    private Spike _spike;
    private GameObject _safeShoes;
    private int _spikeAttackPower;
    private float _originalMoveSpeed;
    private float _originalJumpSpeed;
    private bool SafeMessage;
    //bool
    //「true or falseの値を設定できる変数、2パターンの処理が書ける
    private bool _bJump;
    private Bullet _bullet;


    private bool _signBoardHit;
    private bool _read;
    private bool JohnCalled;
    private GameObject _signBoard;
    private GameObject _currentSignBoardUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //あるメソッドに限らずゲーム全体で使いたい変数を指定するところ？？
        
        //GetComponent<>()
        //「<>で指定したコンポーネントをオブジェクトからとってくる」

    　　　_originalMoveSpeed = _moveSpeed;
    　　　_originalJumpSpeed = _jumpSpeed;
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _signBoardHit = false;
        JohnCalled = false;
        SafeMessage = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bJump = false;
    　　 _hp = PlayerPrefs.GetInt ("HP", _starthp);
       Debug.Log("HPのかず"　+ _hp);



        _spike = FindObjectOfType<Spike>();
        _spikeAttackPower = _spike._attackPower;
        _spikeAttackPower = PlayerPrefs.GetInt ("Spike", _spikeAttackPower);

        _safeShoes = GameObject.Find("SafeShoes");

        string currentSceneName = SceneManager.GetActiveScene().name;
    }

    //  削除時の処理
    void OnDestroy()
    {
        // 他のシーンでも適応できるようにHPとSpikeの攻撃力を保存
        PlayerPrefs.SetInt ("HP", _hp);
        PlayerPrefs.Save ();
        PlayerPrefs.SetInt ("Spike", _spikeAttackPower);
        PlayerPrefs.Save ();
    }

    // Update is called once per frame
    void Update()
    {
        //updateメソッドの下に書いたメソッドの名前を書いてゲーム起動中に実行してもらう所
        _Move();
        Debug.Log(_hp);
        _LookMoveDirec();
        _HitFloor();
    }

    
    //何をして欲しいか書く所、メソッド生成所
    
    //「public/private 型の種類　任意のメソッドの名前」とかいてメソッドの中身を書いていく
    private void _Move()
    {
        if(_bJump)return;
        //linearVelocity
        //「オブジェクトに加えられる力の大きさと方向を入れる時に使う変数」
        //inputDirection.x（プレイヤーが入力した横軸の方向キー）×（上記の移動速度で指定した数字）、_rigid.linearVelocity.y（今までと変わらない上下方向）
        _rigid.linearVelocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.linearVelocity.y);
        _anim.SetBool("Walk", _inputDirection.x != 0.0f);
    }

    private void _LookMoveDirec()
    {
        if(_inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if(_inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }

    
    //OnCollisionEnter2D
    //当たり判定を持っている指定したオブジェクトに衝突した時に実行されるメソッド

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject);
        }

        if(collision.gameObject.tag == "CounterEnemy")
        {
            _HitCounterEnemy(collision.gameObject);
        }

         // 衝突したオブジェクトが "VanishFloor" タグを持っている場合
    if (collision.gameObject.CompareTag("VanishFloor"))
　　 {
        Debug.Log("VanishFloorに衝突！");
    Destroy(collision.gameObject);
    }
     // 衝突したオブジェクトが "VanishFloor" タグを持っている場合
    if (collision.gameObject.CompareTag("TrapFloor"))
    {
        // 衝突したオブジェクト (VanishFloor) を削除する
        Destroy(collision.gameObject);
    }

        else if(collision.gameObject.tag == "Spike")
        {
            _HitSpike(collision.gameObject);
            _bJump = false;
            _anim.SetBool("Jump",_bJump);
        }

        else if(collision.gameObject.tag =="EnterDoor1")
        {
            _HitDoor1(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor2")
        {
            _HitDoor2(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor3")
        {
            _HitDoor3(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor4")
        {
            _HitDoor4(collision.gameObject);
        }
        else if(collision.gameObject.tag =="EnterDoor5")
        {
            _HitDoor5(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor6")
        {
            _HitDoor6(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor7")
        {
            _HitDoor7(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor8")
        {
            _HitDoor8(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor9")
        {
            _HitDoor9(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor10")
        {
            _HitDoor10(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor11")
        {
            _HitDoor11(collision.gameObject);
        }

        else if(collision.gameObject.tag =="EnterDoor12")
        {
            _HitDoor12(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother1")
        {
            
           _SwitchAnotherWorld1(collision.gameObject);
        }

         else if(collision.gameObject.tag == "HoleToAnother2")
        {
            
           _SwitchAnotherWorld2(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother3")
        {
            
           _SwitchAnotherWorld3(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother4")
        {
            
           _SwitchAnotherWorld4(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother5")
        {
            
           _SwitchAnotherWorld5(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother6")
        {
            
           _SwitchAnotherWorld6(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToAnother7")
        {
            
           _SwitchAnotherWorld7(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal1")
        {
            
           _SwitchRealWorld1(collision.gameObject);
        }

         else if(collision.gameObject.tag == "HoleToReal2")
        {
            
           _SwitchRealWorld2(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal3")
        {
            
           _SwitchRealWorld3(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal4")
        {
            
           _SwitchRealWorld4(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal5")
        {
            
           _SwitchRealWorld5(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal6")
        {
            
           _SwitchRealWorld6(collision.gameObject);
        }

        else if(collision.gameObject.tag == "HoleToReal7")
        {
            
           _SwitchRealWorld7(collision.gameObject);
        }

        else if(collision.gameObject.tag =="SafeShoes")
        {
            _SpikeInvaild(collision.gameObject);
        }

        else if(collision.gameObject.tag =="Bullet")
        {
            _HitBullet(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("SignBoard"))
        {
            _signBoardHit = true;
            _bJump = false;
            var _signBoard = collision.gameObject.GetComponent<SignBoard>();
            _currentSignBoardUI = _signBoard.GetUI();
            Debug.Log("当たってる");
        }

        else if (collision.gameObject.tag == "Goal")
        {
            var main = FindObjectOfType<MainManager>();
            if(main != null)
            {
            main._ShowGameClearUI();
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            }
            else if(main == null)
            
            {
            var main2 = FindObjectOfType<SecondManager1>();
            main2._ShowGameClearUI();
            enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            }
        }

        else if (collision.gameObject.CompareTag("Door"))
        {
             _HitDoor(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SignBoard") && _currentSignBoardUI != null)
        {
            _currentSignBoardUI.SetActive(false);
            _signBoardHit = false;
            Debug.Log("離れてる");
        }
    }


    private void _HitFloor()
    {
           int layerMask = LayerMask.GetMask("Floor");
            Vector3 rayPos = transform.position - new Vector3 (0.0f, transform.lossyScale.y / 2.0f + _adjustHitBox);
            Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f,0.1f);
            RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
            
            if(rayHit.transform == null)
            {
                _bJump = true;
                _anim.SetBool("Jump", _bJump);
                return;
            }

            if(rayHit.transform.tag == "Floor" && _bJump)
            {
                _bJump = false;
               _anim.SetBool("Jump",_bJump);
            }
    }

    private void _HitSpike(GameObject spike)
    {
            if(_spikeAttackPower == 0)return;
            spike.GetComponent<Spike>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
    }
    
    
    public void _SpikeInvaild(GameObject safeShoes)
    {
       if(_spikeAttackPower != 0) 
        {
        Debug.Log("安全靴ゲット");
        _spikeAttackPower = 0;
        Destroy(safeShoes);
        Debug.Log("トゲの攻撃力は01");
        _SafeMessage.SetActive(true);
        _moveSpeed = 0;
        _jumpSpeed = 0;
        SafeMessage = true;

        }

        else if(_spikeAttackPower == 0) 
        {
        Debug.Log("トゲの攻撃力は02");
        }
        else if(_spikeAttackPower == null)
        {
        Debug.Log("トゲの攻撃力は03");
        }

    }

    private void _HitDoor(GameObject door)
    {
    Destroy(door);
    }    



    public void _SwitchAnotherWorld1(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-1");  
    }

    public void _SwitchAnotherWorld2(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-2");
        
    }

    public void _SwitchAnotherWorld3(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-3");
        
    }

    public void _SwitchAnotherWorld4(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-4");
        
    }

    public void _SwitchAnotherWorld5(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-5");
        
    }

    public void _SwitchAnotherWorld6(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-6");
        
    }

    public void _SwitchAnotherWorld7(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second blackstage-7");
        
    }

    public void _SwitchRealWorld1(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-1");  
    }

    public void _SwitchRealWorld2(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-2");
        
    }

    public void _SwitchRealWorld3(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-3");
        
    }

    public void _SwitchRealWorld4(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-4");
        
    }

    public void _SwitchRealWorld5(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-5");
        
    }

    public void _SwitchRealWorld6(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-6");
        
    }

    public void _SwitchRealWorld7(GameObject hole)
    {
        Debug.Log("Switching to second stage...");
        SceneManager.LoadScene("Second Realstage-7");
        
    }

    private void _HitEnemy(GameObject enemy)

    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
　　　　　float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if ( transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            Instantiate(_jumpSE);
        }
        else

        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
        }
 
    }

    private void _HitCounterEnemy(GameObject enemy)

    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
　　　　　float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        if ( transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy);
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            Instantiate(_jumpSE);
            enemy.GetComponent<Enemy>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
        }
        else

        {
            enemy.GetComponent<Enemy>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
        }
    }


    private void _HitBullet(GameObject bullet)
    {
            bullet.GetComponent<Bullet>().PlayerDamage(this);
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
            StartCoroutine(_Damage());
    }


    private void _HitDoor1(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor1");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor2(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor2");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor3(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor3");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor4(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor4");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor5(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor5");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor6(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor6");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor7(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor7");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor8(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor8");

        if (exitDoor != null && !JohnCalled)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;

    }
    else if (exitDoor != null || JohnCalled)
    {
        transform.position = exitDoor.transform.position;
        Debug.Log("2");
    }
}

    private void _HitDoor9(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor9");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor10(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor10");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
    else
    {
        Debug.LogWarning("ExitDoor not found!");
    }
}

    private void _HitDoor11(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor11");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
        StartCoroutine(DoorEventSequence());
        Debug.Log("1");
    }
    
    else if (exitDoor != null || JohnCalled)
    {
        transform.position = exitDoor.transform.position;
        Debug.Log("2");
    }
}

    private void _HitDoor12(GameObject door)
{
    // "EnterDoor" に触れたときに "ExitDoor" に移動
    GameObject exitDoor = GameObject.FindGameObjectWithTag("ExitDoor12");

    // ExitDoorが存在する場合にのみ処理
    if (exitDoor != null)
    {
        // ExitDoorの位置にプレイヤーを移動させる
        transform.position = exitDoor.transform.position;
    }
}
private 


    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for(int i = 0; i < _damageTime; i ++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f);

            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);
        }
        _spriteRenderer.color = color;
        gameObject.layer = LayerMask.NameToLayer("Default");

        yield return new WaitForSeconds(_damageTime);
        _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f);

    }

    private void _Dead()
    { 
        if (_hp <= 0)
        {
            Destroy(gameObject);
            Debug.Log("プレイヤーはHPがなくなったので消しました");
        }
    }

    private void OnBecameInvisible()
    {
        Camera camera = Camera.main;
        if(camera.name =="Main Camera" && camera.transform.position.y > transform.position.y)
        {
            Destroy(gameObject);
            Debug.Log("プレイヤーは画面外に出たので消しました");
        }
    }

    //プレイヤーが入力した情報をスクリプト上の変数に変換するメソッド
    //カッコ内に書いた変数（引数）
    //メソッドを実行する際に値を入れられる変数,メソッドないでしか使わない変数として扱うこともある。
    //(（入れたい値）　変数の任意の名前)を入力する

    //InputAction.CallbackContext
    //「プレイヤーが入力した情報をとってくる変数」
    public void _OnMove(InputAction.CallbackContext context)
    {
        //ReadValue<>()
        //「InputAction.CallbackContextでとってきた変数を<>の型のに変えて代入することができる」
        if(_read) return;
        _inputDirection = context.ReadValue<Vector2>();
    }

    public void OnRead(InputAction.CallbackContext context)
    {
        if(!context.performed || !_signBoardHit) return;

        else if(context.performed && !_read && _signBoardHit)
        {
        Debug.Log("読んでる");
        _currentSignBoardUI.SetActive(true);
        _read = true;
        _moveSpeed = 0;
        _jumpSpeed = 0;
        //Instantiate(_signBoardSE);
        }

        else if(context.performed && _read && _signBoardHit)
        {
        _currentSignBoardUI.SetActive(false);
        _read = false;
        _bJump = false;
        _moveSpeed = _originalMoveSpeed;
        _jumpSpeed = _originalJumpSpeed;
        //Instantiate(_signBoardSE);
        }
    }

    public void OnSafeRead(InputAction.CallbackContext context)
    {
        if(!context.performed || !SafeMessage) return;
        Debug.Log("安全靴読み終えた");
        _SafeMessage.SetActive(false);
        _moveSpeed = _originalMoveSpeed;
        _jumpSpeed = _originalJumpSpeed;
        SafeMessage = false;
        //Instantiate(_signBoardSE);
    }

    public void OnJump(InputAction.CallbackContext contect)
    {
        //if（条件）{どのような処理をするのか}
        //!〜「〜ではない」
        //||「または」
        //return
        //「if文の条件に当てはまるときは、return以降に書かれた処理は実行しない」
        //performed
        //「プレイヤーがゲームパッドを操作中」
        if(!contect.performed || _bJump || _read) return;

        //AddForce（力を加えたい方向(x,y,zそれぞれ代入可),適用する力のタイプ）
        //オブジェクトを加速させる処理
        //Vector2.up「上方向」
        //ForceMode2D.Force
        //「初速遅い、徐々に加速　ex)車、電車」
        //ForceMode2D.Impulse
        //「初速速い、徐々に減速　ex)ボールを投げた時」
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        Instantiate(_jumpSE);
        //_bJump = true;
        //_anim.SetBool("Jump",_bJump);
    }

    public void Damage(int damage)
    {
        _hp = Mathf.Max(_hp - damage, 0 );
        _Dead();
        Instantiate(_damageSE);

        if(_damageLaughingSE == null) return;
        Instantiate(_damageLaughingSE);
    }

    public int GetHP()
    {
        return _hp;
    }

    public void MoveControl()
    {
        if(_read)
        {
        _moveSpeed = 0;
        _jumpSpeed = 0;
        }

        else if(_read!)
        {
        _moveSpeed = _originalMoveSpeed;
        _jumpSpeed = _originalJumpSpeed;
        }
    }

    private IEnumerator DoorEventSequence()
    {
        // ① プレイヤー停止
        GetComponent<PlayerInput>().enabled = false;
        BGMManager _manager = BGMManager.Instance;
　　　　　_manager._BGMStop();
// 　　　　　_mainmanager.GetComponent<BGMManager>()._BGMStop();

        

        // ② 電話音を鳴らす
        // Instantiate(_phoneCall);
        (_phoneCall).SetActive(true);

        // ③ 音が終わるのを待つ
        yield return new WaitForSeconds(_phoneCallLength);

        (_phoneCall).SetActive(false); //音きる
        _phoneCallScipt.SetActive(true);// ④ UI表示

          // ⑤ 一定時間待ってからUIを非表示
        yield return new WaitForSeconds(uiDisplayTime);
        _phoneCallScipt.SetActive(false);

        // ⑥ プレイヤー再び動けるようにする（必要なら）
        GetComponent<PlayerInput>().enabled = true;
　　　　　_manager._BGMStart();
        JohnCalled=true;
        _bulletObj.SetActive(true);
    }
}
