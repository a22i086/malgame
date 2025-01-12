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
    public Transform firePoint; // 発射位置
    public Renderer towerRenderer;
    private Color originalColor; // Towerの元となる色
    public float transparency = 0.5f; // Towerの透明度

    private float fireCountdown = 0f;
    private Transform target;
    private GameManager gameManager;
    public int team;

    void Start()
    {
        if (towerRenderer == null)
        {
            towerRenderer = GetComponent<Renderer>();
        }
        originalColor = towerRenderer.material.color;
        gameManager = FindObjectOfType<GameManager>(); //ゲームマネージャーをシーンから見つけて取得

        // Vector3 firePointPosition = firePoint.position;
        // firePointPosition.y += 1.0f;
        // firePoint.position = firePointPosition;

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

    void FindTarget() //チームが違う動物を見つける
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        List<Character> enemies = gameManager.GetEnemiesForTower(this);
        float shortestDistance = Mathf.Infinity;
        Character nearestEnemy = null;

        foreach (Character enemy in enemies)
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
        GameObject projectileGO = (GameObject)Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
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
        Gizmos.DrawWireSphere(firePoint.position, range);
    }
    public void TakeDamage(float amount)
    {
        healthManager.TakeDamage(amount);
    }
}
