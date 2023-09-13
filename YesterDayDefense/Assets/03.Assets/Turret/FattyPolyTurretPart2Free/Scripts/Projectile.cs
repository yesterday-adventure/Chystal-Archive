﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolableMono
{

    public TurretAI.TurretType type = TurretAI.TurretType.Single;
    public Transform target;
    public bool lockOn;
    //public bool track;
    public float speed = 1;
    public float turnSpeed = 1;
    public bool catapult;

    public float knockBack = 0.1f;
    public float boomTimer = 1;
    private readonly float _defaultBoomTimeLimit = 1f;
    //public Vector3 _startPosition;
    //public float dist;

    public PoolingParticle explosion;
    private PoolManager _poolManager;

    private void Awake()
    {
        _poolManager = PoolManager.Instance;
    }

    private void Update()
    {
        if (target == null)
        {
            Explosion();
            return;
        }

        if (transform.position.y < -0.2F)
        {
            Explosion();
        }

        boomTimer -= Time.deltaTime;
        if (boomTimer < 0)
        {
            Explosion();
        }

        if (type == TurretAI.TurretType.Catapult)
        {
            if (lockOn)
            {
                Vector3 Vo = CalculateCatapult(target.transform.position, transform.position, 1);

                transform.GetComponent<Rigidbody>().velocity = Vo;
                lockOn = false;
            }
        }else if(type == TurretAI.TurretType.Dual)
        {
            Vector3 dir = target.position - transform.position;
            //float distThisFrame = speed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, dir, Time.deltaTime * turnSpeed, 0.0f);
            Debug.DrawRay(transform.position, newDirection, Color.red);

            //transform.Translate(dir.normalized * distThisFrame, Space.World);
            //transform.LookAt(target);

            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            transform.rotation = Quaternion.LookRotation(newDirection);

        }else if (type == TurretAI.TurretType.Single)
        {
            float singleSpeed = speed * Time.deltaTime;
            transform.Translate(transform.forward * singleSpeed * 2, Space.World);
        }
    }

    Vector3 CalculateCatapult(Vector3 target, Vector3 origen, float time)
    {
        Vector3 distance = target - origen;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            // 명중효과 작성
            EnemyDamage(other.gameObject);
            Explosion();
        }
    }

    private void EnemyDamage(GameObject gameObject)
    {
        //
        Debug.Log("데미지");
    }

    public void Explosion()
    {
        if (explosion != null)
        {
            PoolingParticle explosionParticle = 
                (_poolManager.Pop(explosion.name) as PoolingParticle);

            explosionParticle.Play();
            explosionParticle.SetPosition(transform.position);
        }
            
        _poolManager.Push(this);
    }

    public override void Reset()
    {
        if (catapult)
        {
            lockOn = true;
        }

        if (type == TurretAI.TurretType.Single)
        {
            Vector3 dir = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir);
        }

        if (type == TurretAI.TurretType.Catapult)
            boomTimer = _defaultBoomTimeLimit;
    }
}
