using UnityEngine;

public class SignBoard : MonoBehaviour
{
    [SerializeField, Header("看板の内容")]
    private GameObject _ui;

    public GameObject GetUI()
    {
        return _ui;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
