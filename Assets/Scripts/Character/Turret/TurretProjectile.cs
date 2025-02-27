using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] protected Transform projectileSpawnPosition;
    [SerializeField] protected float delayBtwAttacks;



    protected float _nextAttackTime = 0f;
    protected Pooler _pooler;
    protected Projectile _currentProjectileLoaded;
    protected Turret _turret;

    private void Start()
    {

        _turret = GetComponent<Turret>();
        _pooler = GetComponent<Pooler>();

        LoadProjectile();
    }

    protected virtual void Update()
    {

        if (IsTurretEmpty())
        {
            LoadProjectile();

        }

        if (Time.time > _nextAttackTime)
        {
            if (_turret.CurrentEnemyTarget != null && _currentProjectileLoaded != null
                && _turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
                _currentProjectileLoaded.transform.parent = null;
                _currentProjectileLoaded.SetEnemy(_turret.CurrentEnemyTarget);

            }
            _nextAttackTime = Time.time + delayBtwAttacks;
        }


    }
    protected virtual void LoadProjectile()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnPosition);


        _currentProjectileLoaded = newInstance.GetComponent<Projectile>();

        _currentProjectileLoaded.TurretOwner = this;

        _currentProjectileLoaded.ResetProjectile();
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        return _currentProjectileLoaded == null;
    }

    public void ResetTurretProjectile()
    {
        _currentProjectileLoaded = null;
    }

}
