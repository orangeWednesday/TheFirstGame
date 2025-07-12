using UnityEngine;

public class ShakingPeople : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    [SerializeField, Header("震え始める距離")]
    public float triggerDistance;
    [SerializeField, Header("揺れの大きさ")]
    public float shakeAmplitude;
    [SerializeField, Header("揺れの速さ")]
    public float shakeSpeed;

    private Vector3 initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < triggerDistance)
        {
            // 震える動き（左右にSin波）
            float offsetX = Mathf.Sin(Time.time * shakeSpeed) * shakeAmplitude;
            transform.localPosition = initialPosition + new Vector3(offsetX, 0f, 0f);
        }
        else
        {
            // 元の位置に戻る
            transform.localPosition = initialPosition;
        }
    }
}
