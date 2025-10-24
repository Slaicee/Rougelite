using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreGenerate : MonoBehaviour
{
    public GameObject orePrefab;
    public int oreCount = 20;//��������
    public float generateRange = 70f;//���ɷ�Χ
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < oreCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-generateRange, generateRange),
                1f, 
                Random.Range(-generateRange, generateRange)
            );
            Instantiate(orePrefab, randomPos, Quaternion.identity);
        }
    }
}
