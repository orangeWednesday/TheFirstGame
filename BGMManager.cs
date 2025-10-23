using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    private AudioSource audioSource;

    // [SerializeField, Header("ゲームオーバーUI")]
    // private GameObject _gameOverUI;
    // [SerializeField, Header("ゲームクリアUI")]
    // private GameObject _gameClearUI;

    [SerializeField, Header("穴の中のボリューム")]
    private float quietVolume;

    private bool isBGMStopped;
    // private bool _bShowUI;
    private bool shouldStop;
    private GameObject _player;

    void Awake()
    {
        // まずインスタンスチェックして破棄
        if (instance != null)
        {
            Destroy(gameObject);
            return;
            Debug.Log("音楽壊します");
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        shouldStop = false;
        Debug.Log("音楽止めるべきではない");
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("音楽始めます1");
            isBGMStopped = false;

        }
    }
    

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        isBGMStopped = false;
    }

    void Update()
    {
        _BGMQuiet();
        _player = GameObject.FindWithTag("Player");

        if(SceneManager.GetActiveScene().name == "Second Realstage"&& !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("セカンドステージがロードされたので音楽流します");
        }
        
        if(_player == null || !audioSource.isPlaying)
        {
        shouldStop = false;
        Debug.Log("音楽止めるべきではない");
        }

        if(_player != null　|| !audioSource.isPlaying) return;
        shouldStop = true;
        Debug.Log("次音楽止めるべき");
        _BGMStopSituation();
        

    
    }

    private void _BGMStopSituation()
    {
        if (shouldStop)
    {
            audioSource.Stop();
            isBGMStopped = true;
            Debug.Log("音楽を停止しました");
            shouldStop = false;
        Debug.Log("音楽止めるべきではない");
    }
    }

    private void _BGMQuiet()
    {
        if (SceneManager.GetActiveScene().name == "Second blackstage-1"||
            SceneManager.GetActiveScene().name == "Second blackstage-2"||
            SceneManager.GetActiveScene().name == "Second blackstage-3"||
            SceneManager.GetActiveScene().name == "Second blackstage-4"||
            SceneManager.GetActiveScene().name == "Second blackstage-5"||
            SceneManager.GetActiveScene().name == "Second blackstage-6"||
            SceneManager.GetActiveScene().name == "Second blackstage-7"
            )
        {
            audioSource.volume = quietVolume;
            Debug.Log("静かにする");
        }
        else
        {
            audioSource.volume = 1f;
        }
    }


    // public void OnRestart(InputAction.CallbackContext context)
    // {
    // if(!context.performed ||audioSource.isPlaying) return;
    //     audioSource.Play();
    //     Debug.Log("音楽再会");
    //     isBGMStopped = false;
    // }


// 敵が電話をかけてくる時にBGMを止めるのに使うメソッド（Playerクラスで引用）
    public void _BGMStop()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            isBGMStopped = true;
        }
    }

// 敵の電話が終わった後にBGMを再度再生するのに使うメソッド（Playerクラスで引用）
    public void _BGMStart()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            isBGMStopped = false;
        }
    }

    public static BGMManager Instance => instance;
}
