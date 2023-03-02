using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;
    //무기관련
    public GameObject[] weapons;
    public bool[] hasWeapons;



    public float jumppower = 0f;

    static int jumpcount = 2;

    float hAxis;
    float vAxis;
    bool isTouchWall = false; //벽뚤방지 

    bool walkDown = false;
    bool jumpDown = false;
    bool dodgeDown = false;
    bool iDown = false;

    bool sDown1;
    bool sDown2;
    bool sDown3;




    //제약조건
    bool isJump ;
    bool isDodge ;
    bool isSwap;

    Vector3 moveVec;


    Rigidbody rigi;
    Animator anim;

    GameObject nearObject;
    GameObject equipWeapon;
    int equalsWeaponIndex = -1;
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
        Interation();
        Swap();
    }
    void Swap() 
    {
        
        ///무기를 습득해야만 들수있게 + 같은무기 누르면 스왑안하게
        if (sDown1 && (!hasWeapons[0] || equalsWeaponIndex == 0))  //1번을 눌럿을때 망치 소유x 나 1번을 눌럿을때는 스왑x
            return;
        if (sDown2 && (!hasWeapons[1] || equalsWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equalsWeaponIndex == 2))
            return;

        int weaponIndex = -1;
        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;


        if ((sDown1|| sDown2 || sDown3) && !isJump && !isDodge)
        {
            if (equipWeapon != null) { equipWeapon.SetActive(false); }
                isSwap = true;
                equalsWeaponIndex = weaponIndex;
                equipWeapon = weapons[weaponIndex];
                equipWeapon.SetActive(true);

                anim.SetTrigger("doSwap");
                
            Invoke(nameof(SwapOut), 0.5f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }
    void Interation()
    { 
        if(iDown&& nearObject != null&&!isJump&&!isDodge)
        {
            if (nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                
                Rigidbody itemrigi=nearObject.GetComponent<Rigidbody>();
                itemrigi.AddForce(Vector3.up * 20f, ForceMode.Impulse);
                
                

                Invoke(nameof(DestroyDelay),0.5f);
            }
        }

    }
    void DestroyDelay()
    {
        Destroy(nearObject.gameObject);
    }
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        dodgeDown = Input.GetButtonDown("Fire1");
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");




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
                //Debug.Log("평상시");

                moveVec = new Vector3(hAxis, 0, vAxis).normalized;
            }
            if (isSwap)
            {
                moveVec = Vector3.zero;
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

            if (jumpDown && !isJump && !isDodge &&jumpcount>0 && !isSwap)
            {
                jumpcount--;
                //Debug.Log("점프");
                //isJump = true;
                anim.SetBool("isJump", true);  // 움직임있을때
                anim.SetTrigger("doJump");
                rigi.AddForce(Vector3.up * jumppower, ForceMode.Impulse);
            }
        }

        void Dodge()
        {

            if (dodgeDown && !isDodge && moveVec != Vector3.zero&&!isSwap)
            {

                //Debug.Log("구르기");
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
                //Debug.Log("땅위임");
                anim.SetBool("isJump", false);
                //isJump = false;
                jumpcount = 2;
            }


        }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
        {
            nearObject = other.gameObject;
            Debug.Log(nearObject);
            
        }
    }



} 
