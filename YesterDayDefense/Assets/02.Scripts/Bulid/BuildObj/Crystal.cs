using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : BuildAbleMono
{
    public override void Damage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            MonsterManager.Instance.GameOver();
    }

    private void Start()
    {
        LoadWeight.Instance.isSetup[16, 16] = this;
    }

    protected override void OnMouseDown() { }
    protected override void OnMouseEnter() { }
    protected override void OnMouseExit() { }
}
