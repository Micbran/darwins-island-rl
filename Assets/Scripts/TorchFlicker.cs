using UnityEngine;

public class TorchFlicker : MonoBehaviour
{
    [SerializeField] private float flickerIntensityMax;
    [SerializeField] private float flickerIntensityMin;
    [Space(10)]
    [SerializeField] private float flickerMax;
    [SerializeField] private float flickerMin;
    [Space(10)]
    [SerializeField] private float flickerTimeMax;
    [SerializeField] private float flickerTimeMin;

    private float betweenFlickerTimer;

    private Light flickerLight;

    private void Awake()
    {
        this.flickerLight = this.GetComponent<Light>();
        this.betweenFlickerTimer = -1;
    }

    private void Update()
    {
        this.betweenFlickerTimer -= Time.deltaTime;
        if (this.betweenFlickerTimer > 0) return;

        float flickerValue = Random.Range(this.flickerMin, this.flickerMax);
        int flickerSign = GlobalRandom.RandomInt(1, -1); // this can be zero which means no change
        this.flickerLight.intensity = Mathf.Clamp(this.flickerLight.intensity += flickerValue * flickerSign, this.flickerIntensityMin, this.flickerIntensityMax);
        this.flickerLight.range = 20 + this.flickerLight.intensity;
        this.betweenFlickerTimer = Random.Range(this.flickerTimeMin, this.flickerTimeMax);
    }

}
