using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : BuildAbleMono
{
    public override void Damage(int damage)
    {
        _currentHealth -= damage;
        
        UIManager.Instance.HPUI.TweenHPAnim((float)_currentHealth / _maxHealth);
        if (_currentHealth <= 0)
        {
            
            MonsterManager.Instance.GameOver();
        }
    }

    private void Start()
    {
        LoadWeight.Instance.isSetup[16, 16] = this;
    }
}
