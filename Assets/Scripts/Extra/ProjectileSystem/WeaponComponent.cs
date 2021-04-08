using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    ProjectileSystem projectileSystem;
    [SerializeField]
    float m_projectileSpeed =3;

    private void Start()
    {
        projectileSystem = ProjectileSystem.GetInstance();
    }

    public void FireWeapon(Vector3 startPos, Vector3 _target)
    {
        projectileSystem.FireProjectile(startPos, _target, m_projectileSpeed);
    }
}
