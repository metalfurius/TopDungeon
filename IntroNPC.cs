using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNPC : Collidable
{
    public string message;
    float cooldown = 4.0f;
    float lastShout;
    protected override void OnCollide(Collider2D coll)
    {
        if (Time.time - lastShout > cooldown)
        {

            lastShout = Time.time;

            GameManager.instance.ShowText(message, 25, Color.white, transform.position+new Vector3(0,0.16f,0), Vector3.zero, cooldown);
        }
    }
}
