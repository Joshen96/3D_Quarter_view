using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;

    public float jumppower = 0f;

    static int jumpcount = 2;

    float hAxis;
    float vAxis;
    bool isTouchWall = false; //벽뚤방지 

    bool walkDown = false;
    bool jumpDown = false;
    bool dodgeDown = false;



    //점프 제약조건
    bool isJump = false;
    bool isDodge = false;

    Vector3 moveVec;


    Rigidbody rigi;
    Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //자식오브젝트에 있는 컴포넌트 가저오는것
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); //키를받고
        Move(); // 움직이고
        Turn(); //회전하고
        Jump();
        Dodge();
        StopToWall(); // 레이캐스트 사용하여 레이어마스크 Wall 감지
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        dodgeDown = Input.GetButtonDown("Fire1");



    }
    void Move()
    {
        if (isTouchWall) // 벽탐지하면 멈춤
        {
            moveVec = Vector3.zero;
        }
        else
        {

            if (!isDodge) // 구르기아니면==평상시
            {
                Debug.Log("평상시");

                moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            }

                // if문 사용시
                /*

                if (walkDown)
                {
                    transform.position += moveVec * speed* 0.3f * Time.deltaTime; 
                }
                else
                {
                    transform.position += moveVec * speed * Time.deltaTime; //그냥이동
                }

                */
                //3항연산자로 사용시
                transform.position += moveVec * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; //그냥이동


                anim.SetBool("isRun", moveVec != Vector3.zero);  // 움직임있을때
                anim.SetBool("isWalk", walkDown);  // 걷기가 눌러졌을때

            
            
        }
    }
        void Turn()//회전구현
        {


            transform.LookAt(transform.position + moveVec); //룩엣 함수로 내포지션에서 움직이는 곳을 바라본다  
        }
        void Jump()
        {

            if (jumpDown && !isJump && !isDodge &&jumpcount>0)
            {
                jumpcount--;
                Debug.Log("점프");
                //isJump = true;
                anim.SetBool("isJump", true);  // 움직임있을때
                anim.SetTrigger("doJump");
                rigi.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
            }
        }

        void Dodge()
        {

            if (dodgeDown && !isDodge && moveVec != Vector3.zero)
            {

                Debug.Log("구르기");
                speed *= 2;
                rigi.AddForce(Vector3.up, ForceMode.Impulse);
                anim.SetTrigger("doDodge");
                isDodge = true;


                Invoke("DodgeOut", 0.5f);


            }


        }
        void DodgeOut()
        {
            speed *= 0.5f;
            isDodge = false;
        }
        void StopToWall()
        {
            Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
            isTouchWall = Physics.Raycast(transform.position, moveVec, 1, LayerMask.GetMask("Wall"));
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Floor")
            {
                Debug.Log("땅위임");
                anim.SetBool("isJump", false);
                //isJump = false;
                jumpcount = 2;
            }


        }



    } 
