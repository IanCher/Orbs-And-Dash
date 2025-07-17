using UnityEngine;

public class Shield : MonoBehaviour, ICollideable
{
    public void HandlePlayerCollision()
    {
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
