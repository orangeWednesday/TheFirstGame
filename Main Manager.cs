using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{

    [SerializeField,Header("ゲームオーバーUI")]
    private GameObject _gameOverUI;
    [SerializeField,Header("ゲームクリアUI")]
    private GameObject _gameClearUI;
    [SerializeField,Header("BGM")]
　　 public AudioSource _bgm;
    [SerializeField,Header("ゲームクリアSE")]
    private GameObject _gameClearSE;
    [SerializeField,Header("ゲームオーバーSE")]
    private GameObject _gameOverSE;
    [SerializeField,Header("ゲームオーバー 笑いのSE")]
    private GameObject _gameOverLaughingSE;
    [SerializeField, Header("決定音")]
    private GameObject _submitSE;

    private GameObject _player;
    private bool _bShowUI;
    private bool _bShowUIClear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
        _bShowUI = false;
        _bShowUIClear = false;
        FindObjectOfType<Fade>().FadeStart(_MainStart);
        //Fadeが終わった後にplayerやenemyが動くようにする処理(ゲーム開始後は動かない)
        _player.GetComponent<Player>().enabled = false;
        _player.GetComponent<PlayerInput>().enabled = false;
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        { 
        enemySpawner.enabled = false;
        }
    }

    //Fadeが終わった後にplayerやenemyが動くようにする処理(fade終了後動作可能)
    private void _MainStart()
    {
        _player.GetComponent<Player>().enabled = true;
        _player.GetComponent<PlayerInput>().enabled = true;
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        { 
        enemySpawner.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _ShowGameOverUI();
    }

    private void _ShowGameOverUI()
    {
        if (_player != null || _gameOverUI.activeSelf) return;
        _gameOverUI.SetActive(true);
        _bShowUI = true;
        _bShowUIClear = false;
        _bgm.Stop();
        Instantiate(_gameOverSE);

        Debug.Log("TimeScale: " + Time.timeScale);
        
        if(_gameOverLaughingSE == null) return;
        Instantiate(_gameOverLaughingSE);



    }

    public void _ShowGameClearUI()
    {
        if (_gameClearUI.activeSelf) return;
        {
        _gameClearUI.SetActive(true);
        _bShowUI = false;
        _bShowUIClear = true;
        _bgm.Stop();
        Instantiate(_gameClearSE);

         }
    }

    //unity上2DActionGame→UI→Actionsに任意の名前を追加→三角押して<no binding>のPathに任意のボタンを追加
    //スクリプト作成後、Mainmanager→コンポーネントのplayerinput→Events→UIの中に先ほど設定したボタンの任意の名前が追加されているところにいく
    //このスクリプトを追加したゲームオブジェクトとno functionのところにこのメソッドの名前を追加
    public void OnRestart(InputAction.CallbackContext context)
    {
        if (_bShowUIClear && context.performed)
        {
        SceneManager.LoadScene("Second Realstage");
        Instantiate(_submitSE);
        PlayerPrefs.DeleteKey("HP");
        }
        else if(_bShowUI && context.performed)
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Instantiate(_submitSE);
        PlayerPrefs.DeleteKey("HP");
        }

    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if(!context.performed)return;
        Application.Quit();
    }
    public void _BGMStop()
    {
        _bgm.Stop();
    }
    public void _BGMStart()
    {
        _bgm.Play();
    }


}
