using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurretAI : MonoBehaviour
{

    public enum TurretType
    {
        Single = 1,
        Dual = 2,
        Catapult = 3,
    }

    public GameObject currentTarget;
    public Transform turreyHead;

    //public Quaternion randomRot;
    public Vector3 randomRot;
    public Animator animator;

    private float timer;

    [Header("이름")]
    [SerializeField] private string _turretName;

    [Header("폭발 범위")]
    [SerializeField, Range(0f, 5f)] private float _exploveRadius;

    [Header("공격 속성값")]
    [SerializeField] private int _attackDamage;
    [SerializeField] private float shootCoolDown;

    [Header("공격 속성값")]
    [SerializeField] private float lockSpeed = 100.0f;
    [SerializeField] private float _attackDist = 10.0f;
    [SerializeField] private bool _isIceBullet = false;

    [Header("[Turret Type]")]
    public TurretType turretType = TurretType.Single;

    public Transform muzzleMain;
    public Transform muzzleSub;
    public PoolingParticle muzzleEff;
    public GameObject bullet;
    private bool shootLeft = true;

    private Transform lockOnPos;
    private PoolManager _poolManager;

    public bool canAttack = false;

    private readonly int _animationIdleHash = Animator.StringToHash("Idle");
    private readonly int _animationFireHash = Animator.StringToHash("Fire");
    private readonly int _animationReloadHash = Animator.StringToHash("Reload");
    //private readonly int _animationInstallHash = Animator.StringToHash("Install");
    //private readonly int _animationRemoveHash = Animator.StringToHash("Remove");

    void Start()
    {
        _poolManager = PoolManager.Instance;
        InvokeRepeating("ChackForTarget", 0, 0.5f);
        //shotScript = GetComponent<TurretShoot_Base>();

        randomRot = new Vector3(0, Random.Range(0, 359), 0);
    }

    void Update()
    {
        if (canAttack)
        {
            if (currentTarget != null)
            {
                FollowTarget();

                float currentTargetDist = Vector3.Distance(transform.position, currentTarget.transform.position);
                if (currentTargetDist > _attackDist)
                {
                    currentTarget = null;
                }
            }
            else
            {
                IdleRitate();
            }

            timer += Time.deltaTime;
            if (timer >= shootCoolDown)
            {
                if (currentTarget != null)
                {
                    timer = 0;

                    if (animator != null)
                    {
                        animator.SetTrigger(_animationFireHash);
                        ShootTrigger();
                    }
                    else
                    {
                        ShootTrigger();
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        canAttack = false;
    }

    private void ChackForTarget()
    {
        Collider[] colls = Physics.OverlapSphere(transform.position, _attackDist);
        float distAway = Mathf.Infinity;

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Enemy")
            {
                float dist = Vector3.Distance(transform.position, colls[i].transform.position);
                if (dist < distAway)
                {
                    currentTarget = colls[i].gameObject;
                    distAway = dist;
                }
            }
        }
    }

    private void FollowTarget() //todo : smooth rotate
    {
        Vector3 targetDir = currentTarget.transform.position - turreyHead.position;
        targetDir.y = 0;
        //turreyHead.forward = targetDir;
        if (turretType == TurretType.Single)
        {
            turreyHead.forward = targetDir;
        }
        else
        {
            turreyHead.transform.rotation = Quaternion.RotateTowards(turreyHead.rotation, Quaternion.LookRotation(targetDir), lockSpeed * Time.deltaTime);
        }
    }

    private void ShootTrigger()
    {
        //shotScript.Shoot(currentTarget);
        Shoot(currentTarget);
        //Debug.Log("We shoot some stuff!");
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origen, float time)
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackDist);
    }

    public void IdleRitate()
    {
        bool refreshRandom = false;

        if (turreyHead.rotation != Quaternion.Euler(randomRot))
        {
            turreyHead.rotation = Quaternion.RotateTowards(turreyHead.transform.rotation, Quaternion.Euler(randomRot), lockSpeed * Time.deltaTime * 0.2f);
        }
        else
        {
            refreshRandom = true;

            if (refreshRandom)
            {

                int randomAngle = Random.Range(0, 359);
                randomRot = new Vector3(0, randomAngle, 0);
                refreshRandom = false;
            }
        }
    }

    private void SummonEffect()
    {
        PoolingParticle muzzlePoolEff = _poolManager.Pop(muzzleEff.name) as PoolingParticle;
        muzzlePoolEff.SetPosition(muzzleMain.position);
        muzzlePoolEff.transform.rotation = muzzleMain.rotation;
        muzzlePoolEff.Play();
    }

    private void SummonBullet(Vector3 pos, Quaternion rot, Transform target)
    {
        Projectile projectile = _poolManager.Pop(bullet.name) as Projectile;
        projectile.transform.position = pos; // muzzleMain.position;
        projectile.transform.rotation = rot; // muzzleMain.rotation;
        projectile.target = target; // lockOnPos;

        projectile.SettingProjectileProperty(_attackDamage, _exploveRadius, _isIceBullet);
    }

    public void Shoot(GameObject go)
    {
        if (turretType == TurretType.Catapult)
        {
            lockOnPos = go.transform;

            SummonEffect();
            SummonBullet(muzzleMain.position, muzzleMain.rotation, lockOnPos);
            animator.SetTrigger(_animationReloadHash);

        }
        else if (turretType == TurretType.Dual)
        {
            if (shootLeft)
            {
                SummonEffect();
                SummonBullet(muzzleMain.position, muzzleMain.rotation, currentTarget.transform);
            }
            else
            {
                SummonEffect();
                SummonBullet(muzzleSub.position, muzzleSub.rotation, currentTarget.transform);
            }

            shootLeft = !shootLeft;
        }
        else
        {
            SummonEffect();
            SummonBullet(muzzleMain.position, muzzleMain.rotation, currentTarget.transform);
            animator.SetTrigger(_animationReloadHash);
        }

        
    }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }

    public BlockInfo GetInfo()
    {
        return new BlockInfo()
        {
            name = _turretName,
            attackDamage = _attackDamage,
            attackDist = _attackDist,
        };
    }
}
