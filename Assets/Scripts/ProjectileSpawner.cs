using UnityEngine;
using System.Collections.Generic;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] GameObject projectile = null;
    [SerializeField] Transform poolParent = null;
    [SerializeField] int poolSize = 30;
    [SerializeField] float cooldown = 0.25f;

    List<Projectile> projectilePool = new List<Projectile>();

    float nextShootTime = 0f;

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newProjectile = Instantiate(projectile, poolParent);

            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.Disable();

            projectilePool.Add(projectileComponent);
        }
    }

    public void Shoot()
    {
        if (Time.time < nextShootTime)
            return;

        Projectile available = GetAvailableProjectile();

        if (available != null)
        {
            available.Enable(transform.position, transform.rotation);
            nextShootTime = Time.time + cooldown;
        }
    }

    Projectile GetAvailableProjectile()
    {
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (!projectilePool[i].IsActive())
                return projectilePool[i];
        }

        return null;
    }
}
