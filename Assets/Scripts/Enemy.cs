using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    public float range = 10f; // 射程範囲
    public float fireRate = 1f; // 攻撃速度
    private HealthManager healthManager;

    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    public GameObject BulletPrefab; // 発射するプロジェクトタイル
    private float fireCountdown = 0f;
    private Transform target;

    void Awake()
    {
        healthManager = GetComponent<HealthManager>();
    }

    void Update()
    {
        FindTarget();
        if (target != null && fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void FindTarget() //射程圏内からEnemyタグがついてるやつをみつける
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject tower in towers)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, tower.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = tower;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
            Debug.Log("Target acquired: " + target.name);
        }
        else
        {
            target = null;
        }
    }

    void Shoot()
    {
        GameObject projectileGO = (GameObject)Instantiate(BulletPrefab, transform.position, transform.rotation);
        Bullet bullet = projectileGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
