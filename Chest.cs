using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectible
{
    public Animator anim;
    public int pesosAmount = 5;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            anim.Play("IdleChest");
            GameManager.instance.pesos += pesosAmount;
            GameManager.instance.ShowText("+"+pesosAmount+" coins",25,Color.yellow,transform.position,Vector3.up*25,1.0f);
        }
    }
}
