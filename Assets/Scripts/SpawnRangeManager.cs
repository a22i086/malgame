using UnityEngine;

public class SpawnRangeManager : MonoBehaviour
{
    public static SpawnRangeManager Instance { get; private set; }

    public Vector3 stageMinBounds = new Vector3(-7f, -2f, -52f);
    public Vector3 stageMaxBounds = new Vector3(13f, 10f, -12f);
    public float reduceMaxBound = 25f;

    private Vector3 initialMinBounds;
    private Vector3 initialMaxBounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            initialMinBounds = stageMinBounds;
            initialMaxBounds = stageMaxBounds;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ResetSpawnRange();
    }

    public void ReduceSpawnRange()
    {
        stageMaxBounds.z -= reduceMaxBound;
        Debug.Log("新しいスポーン範囲" + stageMaxBounds);
        Debug.Log("スポーン範囲が狭まった！");
    }

    public void ResetSpawnRange()
    {
        stageMinBounds = initialMinBounds;
        stageMaxBounds = initialMaxBounds;
        Debug.Log("スポーン範囲が初期化されました");
    }

    public Vector3 ClampToSpawnRange(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, stageMinBounds.x, stageMaxBounds.x);
        position.z = Mathf.Clamp(position.z, stageMinBounds.z, stageMaxBounds.z);

        return position;
    }
}
