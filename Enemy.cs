using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("攻撃力")]
    private int _enemyAttackPower;

    private Rigidbody2D _rigid;
    private Animator _anim;
    private Vector2 _moveDirection;
    private bool _bFloor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _moveDirection = Vector2.left;
        _bFloor = true;
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _ChangeMoveDirection();
        _LookMoveDirection();
        _HitFloor();
    }

    private void _Move()
{
    if(!_bFloor) return;
     _rigid.linearVelocity = new Vector2(_moveDirection.x * _moveSpeed, _rigid.linearVelocity.y);
}

private void _ChangeMoveDirection()
{
    Vector2 halfSize = transform.lossyScale / 2.0f;
    int layerMask = LayerMask.GetMask("Floor");
    RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
    if(ray.transform == null)return;
    if(ray.transform.tag == "Floor")
    {
        _moveDirection =-_moveDirection;
    }
}

private void OnCollisionEnter2D(Collision2D collision)
{
    if(collision.gameObject.CompareTag("CounterEnemy"))
    {
        _moveDirection =-_moveDirection;
    }
    if(collision.gameObject.CompareTag("EnterDoor2"))
    {
        _moveDirection =-_moveDirection;
    }
    if(collision.gameObject.CompareTag("SignBoard"))
    {
        _moveDirection =-_moveDirection;
    }
    else if(collision.gameObject.CompareTag("Goal"))
    {
        Destroy(gameObject);
    }
}

private void _LookMoveDirection()
{
    if(_moveDirection.x<0.0f)
    {
        transform.eulerAngles = Vector3.zero;
    }

    else if(_moveDirection.x>0.0f)
    {
        transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
    }
}


private void _HitFloor()
{
    int layerMask = LayerMask.GetMask("Floor");
    Vector3 rayPos = transform.position - new Vector3(0.0f,transform.lossyScale.y / 2.0f);
    Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f,0.1f);
    RaycastHit2D rayHit = Physics2D.BoxCast(rayPos,raySize,0.0f,Vector2.zero, 0.0f, layerMask);
    if(rayHit.transform ==null)
    {
        _bFloor = false;
        _anim.SetBool("Idle",true);
        return;
    }
    else if(rayHit.transform.tag =="Floor"&&!_bFloor)
    {
        _bFloor = true;
        _anim.SetBool("Idle",true);
    }

}
public void PlayerDamage(Player player)
{
    player.Damage(_enemyAttackPower);
}
}
