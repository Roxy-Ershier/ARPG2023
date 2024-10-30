using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake
{
    [SerializeField]private AnimationCurve ShakeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float Duration = 2;
    [SerializeField] private float Speed = 22;
    [SerializeField] private float Magnitude = 1;
    [SerializeField] private float DistanceForce = 1000;
    [SerializeField] private float RotationDamper = 2;
    public void PlayShake(float shakeForce,float shakeSpeed,float shakeDuration)
    {
        Duration = shakeDuration;
        Speed = shakeSpeed;
        RotationDamper= shakeForce;

        Debug.Log($"{Duration}   {Speed}      {RotationDamper}   ");
        
    }

    public IEnumerator Shake()
    {
        var elapsed = 0.0f;
        var camT = Camera.main.transform.parent;
        var originalCamRotation = camT.rotation.eulerAngles;
        var direction = (EnemyManager.MainInstance.GetCurTarget().position - camT.position).normalized;
        var time = 0f;
        var randomStart = Random.Range(-1000.0f, 1000.0f);
        var distanceDamper = 1 - Mathf.Clamp01((camT.position - EnemyManager.MainInstance.GetCurTarget().position).magnitude / DistanceForce);
        Vector3 oldRotation = Vector3.zero;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            var percentComplete = elapsed / Duration;
            var damper = ShakeCurve.Evaluate(percentComplete) * distanceDamper;
            time += Time.deltaTime * damper;
            camT.position -= direction * Time.deltaTime * Mathf.Sin(time * Speed) * damper * Magnitude / 2;

            var alpha = randomStart + Speed * percentComplete / 10;
            var x = Mathf.PerlinNoise(alpha, 0.0f) * 2.0f - 1.0f;
            var y = Mathf.PerlinNoise(1000 + alpha, alpha + 1000) * 2.0f - 1.0f;
            var z = Mathf.PerlinNoise(0.0f, alpha) * 2.0f - 1.0f;

            if (Quaternion.Euler(originalCamRotation + oldRotation) != camT.rotation)
                originalCamRotation = camT.rotation.eulerAngles;
            oldRotation = Mathf.Sin(time * Speed) * damper * Magnitude * new Vector3(0.5f + y, 0.3f + x, 0.3f + z) * RotationDamper;
            camT.rotation = Quaternion.Euler(originalCamRotation + oldRotation);

            yield return null;
        }
    }
}
