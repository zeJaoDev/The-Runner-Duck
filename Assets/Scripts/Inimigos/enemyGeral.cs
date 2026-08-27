using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class enemyGeral : MonoBehaviour
{

    [SerializeField]
    private int vida;

  // [SerializeField]
  // private Animator animator;
    
    [SerializeField]
    private BarraVida barraVida;

    public Transform player;
    public float horizontalSpeed = 3f;

void Start()
{
  player = GameObject.FindGameObjectWithTag("Duck").transform;

  this.barraVida.VidaMax = this.vida;
  this.barraVida.Vida = this.vida;
}
void Update()
{
  if (this.vida > 0)
{
  Mover();
}

  transform.position = new Vector2(Mathf.MoveTowards (transform.position.x, player.transform.position.x, horizontalSpeed * Time.deltaTime) ,transform.position.y);
}

public void ReceberDano()
{
  if (this.vida > 0) 
{ 

  this.vida--;
  this.barraVida.Vida = this.vida;

  if(this.vida <= 0)
{
  Destroy(gameObject);
  // this.animator.SetBool("eliminado", true);
}
  else 
{
  //this.animator.SetTrigger("recebendoDano");        
}

}

}

private void OnCollisionEnter2D(Collision2D collision)
{
  if (collision.gameObject.CompareTag("Duck"))
{
  PlayerLife playerLife =  collision.gameObject.GetComponent<PlayerLife>();

  if (playerLife != null)
{
     playerLife.Morrer();
}
}

  if (collision.gameObject.CompareTag("Enemy"))
{
  Object.Destroy(gameObject);
}

}
 private void Mover() 
{ 

}

}
