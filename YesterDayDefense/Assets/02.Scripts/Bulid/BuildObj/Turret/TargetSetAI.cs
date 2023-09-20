using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetAI : MonoBehaviour
{
    TurretAI _turretAI;
    Vector3 _position;
    float _radius;

    [Header("�Ӽ� ��")]
    [SerializeField]
    private LayerMask _targetLayer;

    void Start()
    {
        _turretAI = GetComponent<TurretAI>();
        _position = transform.position;
        _radius = _turretAI.attackDist;
    }

    private void Update()
    {
        SearchTarget();
    }

    private void SearchTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(
            _position, _radius, _targetLayer);

        if (enemies.Length == 0)
            return;

        // ���� ª�� �Ÿ�
        float minDist = Vector3.Distance(_position, enemies[0].transform.position);
        //�ִܰŸ� Ÿ��
        GameObject minDistTarget = enemies[0].gameObject;  
        for(int i = 1; i < enemies.Length; ++i)
        {
            float dist = Vector3.Distance(_position, enemies[i].transform.position);
            if (minDist <= dist)
                return;

            minDistTarget = enemies[i].gameObject;
            minDist = dist;
        }

        _turretAI.SetTarget(minDistTarget);
    }
}
