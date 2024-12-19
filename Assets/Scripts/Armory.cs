using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    [Header("Armory Settings")]
    [SerializeField] private List<Transform> bladesPosition = new List<Transform>();
    [SerializeField] private List<GameObject> bladesMeshes = new List<GameObject>();

    private void Start()
    {
        RandomizeBlades();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            RandomizeBlades();
        }
    }

    private void RandomizeBlades()
    {
        for (int i = 0; i < bladesPosition.Count; i++)
        {
            for (int j = bladesPosition[i].childCount - 1; j >= 0; j--)
            {
                Destroy(bladesPosition[i].GetChild(j).gameObject);
            }

            int rnd = Random.Range(0, bladesMeshes.Count);

            GameObject bladeInst = Instantiate(bladesMeshes[rnd], bladesPosition[i].position, Quaternion.identity);
            bladeInst.transform.parent = bladesPosition[i];
        }
    }

}
