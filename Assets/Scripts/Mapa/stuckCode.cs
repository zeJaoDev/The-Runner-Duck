using System.Collections;

using UnityEngine;


public class TrapStick : MonoBehaviour
{


    [Tooltip("Tag do jogador")]

    public string playerTag = "Duck";

    public bool holdForever = true;

    public float holdSeconds = 2f;

    public bool parentToTrap = true;

void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag(playerTag)) return;
    var pm = other.GetComponent<PlayerMove>();
    if (pm == null) return;

    pm.Freeze();
    if (parentToTrap) other.transform.SetParent(transform, true);

    if (!holdForever) StartCoroutine(ReleaseAfter(pm.gameObject, holdSeconds));
}

IEnumerator ReleaseAfter(GameObject player, float t)
{
    yield return new WaitForSeconds(t);
    Release(player);
}

public void Release(GameObject player)
{
    if (player == null) return;
    var pm = player.GetComponent<PlayerMove>();
    if (pm != null) pm.Unfreeze();
    if (player.transform.parent == transform) player.transform.SetParent(null, true);
}

}