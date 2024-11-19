using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IHealth
{
    public float range = 10f; // 射程範囲
    public float fireRate = 1f; // 攻撃速度
    private HealthManager healthManager;
    public float Health => healthManager.Health;
    public float MaxHealth => healthManager.MaxHealth;
    public GameObject BulletPrefab; // 発射するプロジェクトタイル
    public Renderer towerRenderer;
    private Color originalColor; // Towerの元となる色
    public float transparency = 0.5f; // Towerの透明度

    private float fireCountdown = 0f;
    private Transform target;

    void Start()
    {
        if (towerRenderer == null)
        {
            towerRenderer = GetComponent<Renderer>();
        }
        originalColor = towerRenderer.material.color;
    }
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= range)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
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
    void OnMouseEnter() // マウスカーソルがTowerの上に来たら透明に
    {
        Debug.Log("enter");
        Color newColor = originalColor;
        newColor.a = transparency;
        towerRenderer.material.color = newColor;
    }
    void OnMouseExit()
    {
        Debug.Log("exit");
        towerRenderer.material.color = originalColor;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
