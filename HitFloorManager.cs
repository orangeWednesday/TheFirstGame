// using UnityEngine;

// public class HitFloorManager : MonoBehaviour
// {





//     public bool _bJump;
//     private Animator _anim;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {

//         _bJump = false;
//         _anim = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//         _HitFloor();
//     }
//     private void _HitFloor()
//     {
//            int layerMask = LayerMask.GetMask("Floor");
//             Vector3 rayPos = transform.position - new Vector3 (0.0f, transform.lossyScale.y / 2.0f);
//             Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f,0.1f);
//             RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
            
//             if(rayHit.transform == null)
//             {
//                 _bJump = true;
//                 _anim.SetBool("Jump", _bJump);
//                 return;
//             }

//             if(rayHit.transform.tag == "Floor" && _bJump)
//             {
//                 _bJump = false;
//                _anim.SetBool("Jump",_bJump);
//             }
//     }
// }
