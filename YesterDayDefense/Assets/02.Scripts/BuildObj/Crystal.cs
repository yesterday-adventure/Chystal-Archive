using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : BuildAbleMono
{
    public override void Damage(int damage)
    {
        base.Damage(damage);
        if (_currentHealth <= 0)
            MonsterManager.Instance.GameOver();

    }
}
