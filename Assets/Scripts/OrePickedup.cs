using UnityEngine;

public class OrePickuped : MonoBehaviour
{
    public int oreAmount = 1;
    public float pickupRange = 1f;
    private Transform player;
    private UIManager uiManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < pickupRange)
        {
            uiManager.AddOre(oreAmount);
            Destroy(gameObject);
        }
    }
}
