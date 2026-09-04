using TMPro;
using UnityEngine;

public class ContadorKills : MonoBehaviour
{
    public static ContadorKills Instancia { get; private set; }

    [Header("Interface do Game Over")]
    [SerializeField] private TMP_Text textoKillsFinais;

    public int QuantidadeDeKills { get; private set; }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Debug.LogWarning(
                "Existe mais de um ContadorKills na cena!",
                gameObject
            );

            enabled = false;
            return;
        }

        Instancia = this;
        QuantidadeDeKills = 0;
    }

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Instancia = null;
        }
    }

    public void RegistrarKill()
    {
        QuantidadeDeKills++;
    }

    public void MostrarKillsFinais()
    {
        if (textoKillsFinais == null)
        {
            Debug.LogWarning(
                "O texto das kills finais não foi configurado!",
                gameObject
            );

            return;
        }

        textoKillsFinais.gameObject.SetActive(true);

        textoKillsFinais.text =
            $"Inimigos derrotados: {QuantidadeDeKills}";
    }
}