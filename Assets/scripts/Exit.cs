using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //level clear code
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
