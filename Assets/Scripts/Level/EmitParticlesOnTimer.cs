using UnityEngine;

public class EmitParticlesOnTimer : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlesToEmit;
    [SerializeField] private float emitTimerMax = 0.2f;
    private float emitTimer = 0f;

    private void FixedUpdate()
    {
        emitTimer -= Time.deltaTime;
        if (emitTimer > 0) return;
        Instantiate(this.particlesToEmit, this.transform.position, Quaternion.identity);
        this.emitTimer = this.emitTimerMax;
    }
}
