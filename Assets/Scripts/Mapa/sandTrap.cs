using UnityEngine;

public class sandTrap : MonoBehaviour
{
private void OnTriggerEnter2D(Collider2D collision)
{
  if (collision.CompareTag("Duck"))
{
  collision.GetComponent<moveDuck>();

  if (player != null)
{
  player.Stuck();
}
  Destroy(gameObject);
}
}
}