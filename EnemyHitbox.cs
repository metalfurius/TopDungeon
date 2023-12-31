using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    //Damage
    public int damage=1;
    public float pushForce=3;
    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player_0")
        {
            //create a new dmg object,before sending to the fighter
            Damage dmg = new Damage()
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };
            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
