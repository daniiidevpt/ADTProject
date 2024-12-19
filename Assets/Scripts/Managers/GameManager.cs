using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Image fillableImage;
    public float fillAmountPerPress = 0.1f;
    public UnityEvent onFillComplete;

    private float currentFill = 0f;

    public MeshMorpher sword;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseFill();
        }
    }

    public void MorphInto(Mesh meshToMorph)
    {
        StartCoroutine(sword.MorphBetweenMeshes(sword.test1, meshToMorph, 2f));
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
