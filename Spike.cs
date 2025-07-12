using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField, Header("攻撃力")]
    public int _attackPower;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



public void PlayerDamage(Player player)
{
    player.Damage(_attackPower);
}
}
