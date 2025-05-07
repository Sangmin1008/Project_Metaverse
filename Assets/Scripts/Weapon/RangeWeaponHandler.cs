using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponHandler : WeaponHandler
{
    [Header("Ranged Attack Data")]
    [SerializeField] private Transform projectileSpawnPosition;
    
    [SerializeField] private int bulletIndex;
    public int BulletIndex
    {
        get { return bulletIndex; }
    }
    
    [SerializeField] private float bulletSize = 1;
    public float BulletSize
    {
        get { return bulletSize; }
    }
    
    [SerializeField] private float duration;
    public float Duration
    {
        get { return duration; }
    }
    
    [SerializeField] private float spread;
    public float Spread
    {
        get { return spread; }
    }
    
    [SerializeField] private int numberOfProjectilesPerShot;
    public int NumberOfProjectilesPerShot
    {
        get { return numberOfProjectilesPerShot; }
    }
    
    [SerializeField] private float multipleProjectilesAngel;
    public float MultipleProjectilesAngel
    {
        get { return multipleProjectilesAngel; }
    }
    
    [SerializeField] private Color projectileColor;
    public Color ProjectileColor
    {
        get { return projectileColor; }
    }

    private ProjectileManager _projectileManager;

    protected override void Start()
    {
        base.Start();
        _projectileManager = ProjectileManager.Instance;
    }

    public override void Attack()
    {
        base.Attack();

        float projectilesAngleSpace = multipleProjectilesAngel;
        int _numberOfProjectilesPerShot = numberOfProjectilesPerShot;

        float minAngle = -(_numberOfProjectilesPerShot / 2f) * projectilesAngleSpace;

        for (int i = 0; i < _numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-spread, spread);
            angle += randomSpread;
            CreateProjectile(Controller.LookDirection, angle);
        }
    }

    private void CreateProjectile(Vector2 _lookDirection, float angle)
    {
        _projectileManager.ShootBullet(this, projectileSpawnPosition.position, RotateVector2(_lookDirection, angle));
    }

    private static Vector2 RotateVector2(Vector2 v, float degree)
    {
        return Quaternion.Euler(0, 0, degree) * v;
    }
}
