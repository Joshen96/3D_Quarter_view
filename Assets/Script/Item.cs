using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo,Coin,Grenade,Heart,Weapon};
    public Type type;
    public int value;
    float count = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 100 * Time.deltaTime);
        updown();

    }
    void updown()
    {
        if (count < 1f)
        {
            count += Time.deltaTime;
            transform.position += Vector3.up * 1 * Time.deltaTime;
            
        }
       
        

    }
}
