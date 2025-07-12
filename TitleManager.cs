using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField, Header("決定音")]
    private GameObject _submitSE;

    private bool _bStart;
    private Fade _fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bStart = false;
        _fade = FindObjectOfType<Fade>();
        //FadeStartが完了後に()内の引数のメソッド(_TitleStart)を実行。
        _fade.FadeStart(_TitleStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _TitleStart()
    {
        _bStart =true;
    }

    private void _ChangeScene()
    {
        SceneManager.LoadScene("First stage");
    }

    public void OnSpaceClick(InputAction.CallbackContext contex)
    {
        if(!contex.performed &&_bStart)
        {
           _fade.FadeStart(_ChangeScene);
           _bStart = false;
           Instantiate(_submitSE);
        }

    }
    
    public void OnEscape(InputAction.CallbackContext contex)
    {
        if(!contex.performed)return;
        Application.Quit();
    }
}
