using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingPopup : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image icon;

    [SerializeField] private float floatDuration = 0.5f;
    [SerializeField] private float floatDistance = 30f;

    private CanvasGroup canvasGroup;
    private Vector3 startPos;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        
    }

    public void Setup(string message, Sprite iconSprite = null) {
        text.text = message;
        if (iconSprite != null) {
            icon.sprite = iconSprite;
            icon.gameObject.SetActive(true);
        }
        else {
            icon.gameObject.SetActive(false);
        }
        startPos = transform.position;
        StartCoroutine(Animate());
    }

    private System.Collections.IEnumerator Animate() {
        float elapsed = 0f;
        Vector3 endPos = startPos + Vector3.up * floatDistance;
        //Debug.Log(startPos);
        //Debug.Log(endPos);
        while (elapsed < floatDuration) {
            float t = elapsed / floatDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            canvasGroup.alpha = 1f - t;

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}