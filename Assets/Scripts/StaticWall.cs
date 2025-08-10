using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticWall : MonoBehaviour, ICollideable
{
    public void HandlePlayerCollision()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
