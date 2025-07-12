using UnityEngine;

public class SE : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _playingEnd();
    }

    private  void _playingEnd()
    {
        if(_audioSource.isPlaying) return;
        Destroy(gameObject);
    }
}
