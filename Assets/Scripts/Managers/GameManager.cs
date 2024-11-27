using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Image fillableImage; // The image to be filled
    public float fillAmountPerPress = 0.1f; // How much the image fills per space bar press
    public UnityEvent onFillComplete; // Event to invoke when the image is fully filled

    private float currentFill = 0f; // Current fill amount (0 to 1)

    void Update()
    {
        // Check if space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseFill();
        }
    }

    void IncreaseFill()
    {
        // Increment the fill amount
        currentFill += fillAmountPerPress;

        // Clamp the fill value to 1 (maximum)
        currentFill = Mathf.Clamp01(currentFill);

        // Update the fillable image
        fillableImage.fillAmount = currentFill;

        // Check if the fill is complete
        if (currentFill >= 1f)
        {
            onFillComplete?.Invoke(); // Trigger the event
        }
    }
}
