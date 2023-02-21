using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo,Coin,Grenade,Heart,Weapon};
    public Type type;
    public int value;
    float count = 0f;

    float updownT = 2;

    

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);

        
        updown();
        //Debug.Log(count);
    }
    void updown()
    {
        count += Time.deltaTime;

        if (count < 1f)
        {
           
            transform.position += Vector3.up * 1.5f * Time.deltaTime;
            
        }
        else if(count > 1f && count<2f){
           
            transform.position += Vector3.down * 1.5f * Time.deltaTime;
            
        }
        else
        {
            count = 0;
        }
    }
    
}
