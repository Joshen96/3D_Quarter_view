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
        anim = GetComponentInChildren<Animator>(); //�ڽĿ�����Ʈ�� �ִ� ������Ʈ �������°�;
    }

    // Update is called once per frame
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        walkDown = Input.GetButton("Walk");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
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
        transform.position += moveVec * speed * (walkDown ? 0.3f : 1f)* Time.deltaTime; //�׳��̵�


        anim.SetBool("isRun",moveVec != Vector3.zero);  // ������������
        anim.SetBool("isWalk", walkDown);  // �ȱⰡ ����������
        

        



    }
}
