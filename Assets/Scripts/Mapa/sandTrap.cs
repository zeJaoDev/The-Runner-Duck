using UnityEngine;

public class sandTrap : MonoBehaviour
{
    [SerializeField]
    private float velocidade = 4f;

    private void Update()
    {
        transform.Translate(Vector2.down * velocidade * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
}