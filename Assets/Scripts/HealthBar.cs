using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image redGauge; // 赤色のゲージ（背景）
    public Image greenGauge; // 緑色のゲージ（前景）
    private IHealth healthComponent;

    void Start()
    {
        healthComponent = GetComponentInParent<IHealth>();
    }

    void Update()
    {
        // HPゲージの管理
        if (healthComponent != null)
        {
            float healthPercentage = healthComponent.Health / healthComponent.MaxHealth;
            greenGauge.fillAmount = healthPercentage;
        }
    }
}
