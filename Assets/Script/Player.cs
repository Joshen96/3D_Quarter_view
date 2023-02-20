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
    bool isTouchWall = false; //���Թ��� 

    bool walkDown = false;
    bool jumpDown = false;
    bool dodgeDown = false;



    //���� ��������
    bool isJump = false;
    bool isDodge = false;

    Vector3 moveVec;


    Rigidbody rigi;
    Animator anim;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>(); //�ڽĿ�����Ʈ�� �ִ� ������Ʈ �������°�
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput(); //Ű���ް�
        Move(); // �����̰�
        Turn(); //ȸ���ϰ�
        Jump();
        Dodge();
        StopToWall(); // ����ĳ��Ʈ ����Ͽ� ���̾��ũ Wall ����
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
        if (isTouchWall) // ��Ž���ϸ� ����
        {
            moveVec = Vector3.zero;
        }
        else
        {

            if (!isDodge) // ������ƴϸ�==����
            {
                Debug.Log("����");

                moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            }

                // if�� ����
                /*

                if (walkDown)
                {
                    transform.position += moveVec * speed* 0.3f * Time.deltaTime; 
                }
                else
                {
                    transform.position += moveVec * speed * Time.deltaTime; //�׳��̵�
                }

                */
                //3�׿����ڷ� ����
                transform.position += moveVec * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; //�׳��̵�


                anim.SetBool("isRun", moveVec != Vector3.zero);  // ������������
                anim.SetBool("isWalk", walkDown);  // �ȱⰡ ����������

            
            
        }
    }
        void Turn()//ȸ������
        {


            transform.LookAt(transform.position + moveVec); //�迧 �Լ��� �������ǿ��� �����̴� ���� �ٶ󺻴�  
        }
        void Jump()
        {

            if (jumpDown && !isJump && !isDodge &&jumpcount>0)
            {
                jumpcount--;
                Debug.Log("����");
                //isJump = true;
                anim.SetBool("isJump", true);  // ������������
                anim.SetTrigger("doJump");
                rigi.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
            }
        }

        void Dodge()
        {

            if (dodgeDown && !isDodge && moveVec != Vector3.zero)
            {

                Debug.Log("������");
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
                Debug.Log("������");
                anim.SetBool("isJump", false);
                //isJump = false;
                jumpcount = 2;
            }


        }



    } 
