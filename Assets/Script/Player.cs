using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 0f;

    float hAxis;
    float vAxis;
    bool walkDown;

    Vector3 moveVec;

    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>(); //자식오브젝트에 있는 컴포넌트 가저오는것;
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
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
        transform.position += moveVec * speed * (walkDown ? 0.3f : 1f)* Time.deltaTime; //그냥이동


        anim.SetBool("isRun",moveVec != Vector3.zero);  // 움직임있을때
        anim.SetBool("isWalk", walkDown);  // 걷기가 눌러졌을때
        

        



    }
}
