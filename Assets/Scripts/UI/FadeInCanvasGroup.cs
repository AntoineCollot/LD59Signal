using System.Collections;
using UnityEngine;

public class FadeInCanvasGroup : MonoBehaviour
{
    CanvasGroup group;
    [SerializeField] float fadeInTime = 1;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / fadeInTime;

            group.alpha = Curves.QuadEaseInOut(0, 1, Mathf.Clamp01(t));

            yield return null;
        }
    }
}
