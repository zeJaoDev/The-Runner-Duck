using UnityEngine;

public class sandTrap : MonoBehaviour
{
private void OnTriggerEnter2D(Collider2D collision)
{
  if (collision.CompareTag("Duck"))
{
  PlayerMove duck = collision.GetComponent<PlayerMove>();

  if (duck != null)
{
  duck.Stuck();
}
  Destroy(gameObject);
}
}
}