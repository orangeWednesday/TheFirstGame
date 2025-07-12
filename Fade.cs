using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Events;


public class Fade : MonoBehaviour
{
    enum Mode
    {
        FadeIn,
        FadeOut,
    }

    [SerializeField, Header("フェードの時間")]
    private float _fadeTime;
    [SerializeField, Header("フェードの種類")]
    private Mode _mode;

    private bool _bFade;
    private float _fadeCount;
    private Image _image;
    //UnityEventは変数に他のクラスのメソッドを代入することができる。
    private UnityEvent _onFadeComplete = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image = GetComponent<Image>();
        //switch文
        //()内で指定した変数の値によって処理を分ける
        //switch (確認する変数)　case 条件(〜の場合): やってほしい内容
        //if分との違い　if文＝上から順番にスクリプトを判断する　switch文＝()内の変数を見て条件に合う方しか実行しない
        switch (_mode)
        {
            case Mode.FadeIn:_fadeCount = _fadeTime; break;
            case Mode.FadeOut: _fadeCount = 0; break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _Fade();
    }

    private void _Fade()
    {
        if(!_bFade) return;

        switch(_mode)
        {
            case Mode.FadeIn: _FadeIn();break;
            case Mode.FadeOut: _FadeOut();break;
        }
        //_fadeCountが減るのに呼応してalpha減っていく
        float alpha = _fadeCount / _fadeTime;
        //alphaは透明度を操る部分
        _image.color = new Color(_image.color.r,_image.color.g,_image.color.b,alpha);
    }

    private void _FadeIn()
    {
        //_fadeCountを徐々に減らしていく
        _fadeCount -= Time.deltaTime;
        if(_fadeCount <= 0)
        {
            _mode = Mode.FadeOut;
            _bFade = false;
            _onFadeComplete.Invoke();
        }
    }
    private void _FadeOut()
    {
        //_fadeCountを徐々に増やしていく
        _fadeCount += Time.deltaTime;
        if(_fadeCount >= _fadeTime)
        {
            _mode = Mode.FadeIn;
            _bFade = false;
            _onFadeComplete.Invoke();
        }
    }
        public void FadeStart(UnityAction listener)
        {
            if (_bFade) return;
            _bFade = true;
            _onFadeComplete.AddListener(listener);
        }

}
