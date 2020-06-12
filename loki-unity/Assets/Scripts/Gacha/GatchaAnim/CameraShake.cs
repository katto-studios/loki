using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform camHolder;

    public bool shake;
    public float shakeDuration;
    public float shakeMagnitude;
    public float magnitudeIncrease;

    private float originalShakeMagnitude;
    private float elapsed = 0.0f;
    private bool getPos = false;
    private Vector3 originalPos;


    private void LateUpdate()
    {
        if (shake)
        {
            if (!getPos)
            {
                originalPos = camHolder.localPosition;
                getPos = true;
                originalShakeMagnitude = shakeMagnitude;
            }

            if (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;
                camHolder.localPosition = new Vector3(x, y, originalPos.z);

                shakeMagnitude += 0.001f * magnitudeIncrease * Time.deltaTime;
                elapsed += Time.deltaTime;
            }
            else if (elapsed >= shakeDuration)
            {
                camHolder.localPosition = originalPos;
                shakeMagnitude = originalShakeMagnitude;
                shake = false;
                getPos = false;
                elapsed = 0.0f;
            }

        }
    }

    public void Shake()
    {
        shake = true;
    }

    public IEnumerator BigShake(float magnitude)
    {
        Vector3 ogPos = camHolder.localPosition;

        float e = 0.0f;

        while (e < .2)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y =
            Random.Range(-1f, 1f) * magnitude;
            camHolder.localPosition = new Vector3(x, y, ogPos.z);

            e += Time.deltaTime;

            yield return null;
        }
        camHolder.localPosition = ogPos;
    }
}
