using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f; // カメラの移動速度
    public Scrollbar scrollbar;
    public PostProcessVolume postProcessVolume;
    private ChromaticAberration chromaticAberration;
    private Vector3 initialPosition;
    private Coroutine resetCoroutine;

    public float minHeight = -60f;
    public float maxHeight = 20f;
    public float resetDelay = 0.1f;

    void Start()
    {
        // カメラの初期位置を保存
        initialPosition = transform.position;
        // スクロールバーの値を変更する
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
    }

    void Update()
    {
        // 現在のカメラ位置を取得
        Vector3 pos = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * panSpeed * 10f;
        pos.y = Mathf.Clamp(pos.y, 1f, 50f);

        if (Input.GetMouseButton(1)) // 右クリックを押している間
        {
            float rotationSpeed = 200.0f;

            //float h = rotationSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
            float v = rotationSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;

            transform.Rotate(v, 0, 0);
        }
    }

    void OnScrollbarValueChanged(float value)
    {
        float newZ = Mathf.Lerp(minHeight, maxHeight, value);
        Vector3 newPosition = initialPosition;
        newPosition.z = newZ;
        transform.position = newPosition;
        chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, value);

        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(ResetChromaticAberration());
    }

    private IEnumerator ResetChromaticAberration()
    {
        yield return new WaitForSeconds(resetDelay);

        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = 0f;
        }
    }
}
