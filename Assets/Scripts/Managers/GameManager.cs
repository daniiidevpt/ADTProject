using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image fillableImage; // The image to be filled
    public float fillAmountPerPress = 0.1f; // How much the image fills per space bar press
    public UnityEvent onFillComplete; // Event to invoke when the image is fully filled

    private float currentFill = 0f; // Current fill amount (0 to 1)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseFill();
        }
    }

    private void IncreaseFill()
    {
        currentFill += fillAmountPerPress;
        currentFill = Mathf.Clamp01(currentFill);

        fillableImage.fillAmount = currentFill;

        if (currentFill >= 1f)
        {
            onFillComplete?.Invoke();
        }
    }
}
