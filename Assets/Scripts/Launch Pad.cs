using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LaunchPad : MonoBehaviour
{
    [Header("Launch Settings")]
    public float launchForce = 20f;
    public Vector3 launchDirection = Vector3.up;
    public string targetTag = "Player";

    [Header("Visual Spring Rig")]
    public Transform springCylinder;
    public Transform padSurface;
    public Transform springTopTarget;

    [Header("Cartoon Animation Settings")]
    [Tooltip("How squashed the spring gets before launch (multiplier).")]
    public float squashScaleY = 0.2f;
    [Tooltip("How fast it squashes down (seconds).")]
    public float squashDuration = 0.05f;

    [Tooltip("How tall it stretches during the overshoot (e.g., 2.5 means 2.5x its normal height!).")]
    public float stretchScaleY = 2.5f;
    [Tooltip("How fast it explodes upward (seconds).")]
    public float stretchDuration = 0.15f;

    [Tooltip("How long it takes to wobble back to normal (seconds).")]
    public float settleDuration = 0.7f;
    [Tooltip("How many up-and-down wobbles it does before stopping.")]
    public int wobbles = 3;

    private Vector3 originalSpringScale;
    private Coroutine springRoutine;

    private void Start()
    {
        if (springCylinder != null)
        {
            originalSpringScale = springCylinder.localScale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(targetTag) && !other.CompareTag(targetTag))
            return;

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Physics Launch
            Vector3 currentVelocity = rb.linearVelocity;
            currentVelocity.y = 0f;
            rb.linearVelocity = currentVelocity;

            Vector3 launchVector = transform.TransformDirection(launchDirection.normalized) * launchForce;
            rb.AddForce(launchVector, ForceMode.Impulse);

            // Trigger Cartoon Animation
            if (springCylinder != null)
            {
                if (springRoutine != null) StopCoroutine(springRoutine);
                springRoutine = StartCoroutine(AnimateCartoonSpring());
            }
        }
    }

    private void LateUpdate()
    {
        // Snap the pad to the spring's top target
        if (padSurface != null && springTopTarget != null)
        {
            padSurface.position = springTopTarget.position;
            padSurface.rotation = springTopTarget.rotation;
        }
    }

    private IEnumerator AnimateCartoonSpring()
    {
        Vector3 origScale = originalSpringScale;
        float stretchPeak = origScale.y * stretchScaleY;

        // 1. SQUASH (Anticipation)
        float t = 0;
        while (t < squashDuration)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(origScale.y, origScale.y * squashScaleY, t / squashDuration);
            springCylinder.localScale = new Vector3(origScale.x, y, origScale.z);
            yield return null;
        }

        // 2. STRETCH (Explode upwards and hang)
        t = 0;
        while (t < stretchDuration)
        {
            t += Time.deltaTime;
            float percent = t / stretchDuration;
            // Using Sin creates an "Ease-Out" effect so it hangs slightly at the very peak
            float easeOut = Mathf.Sin(percent * (Mathf.PI / 2f));
            float y = Mathf.Lerp(origScale.y * squashScaleY, stretchPeak, easeOut);
            springCylinder.localScale = new Vector3(origScale.x, y, origScale.z);
            yield return null;
        }

        // 3. WOBBLE (Settle down like jelly)
        t = 0;
        float settleDistance = stretchPeak - origScale.y; // The distance it needs to fall back down

        while (t < settleDuration)
        {
            t += Time.deltaTime;
            float percent = t / settleDuration;

            // This math creates a damped harmonic oscillator (a physically accurate bouncy spring)
            float decay = Mathf.Exp(-percent * 5f); // 5f controls how fast it loses energy
            float wobble = Mathf.Cos(percent * wobbles * Mathf.PI * 2f) * settleDistance * decay;

            float y = origScale.y + wobble;
            springCylinder.localScale = new Vector3(origScale.x, y, origScale.z);
            yield return null;
        }

        // Lock exactly back to normal
        springCylinder.localScale = origScale;
    }
}