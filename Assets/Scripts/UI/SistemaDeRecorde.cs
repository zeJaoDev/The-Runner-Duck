using TMPro;
using UnityEngine;

public class SistemaDeRecorde : MonoBehaviour
{
    public static SistemaDeRecorde Instancia
    {
        get;
        private set;
    }

    private const string ChaveRecorde =
        "MelhorPontuacao";

    [Header("Interface do Game Over")]
    [SerializeField] private TMP_Text textoRecorde;

    public int MelhorPontuacao { get; private set; }

    private void Awake()
    {
        if (Instancia != null &&
            Instancia != this)
        {
            Debug.LogWarning(
                "Existe mais de um SistemaDeRecorde!",
                gameObject
            );

            enabled = false;
            return;
        }

        Instancia = this;

        CarregarRecorde();
    }

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Instancia = null;
        }
    }

    public void VerificarRecorde(int pontuacaoFinal)
    {
        bool conseguiuNovoRecorde =
            pontuacaoFinal > MelhorPontuacao;

        if (conseguiuNovoRecorde)
        {
            MelhorPontuacao = pontuacaoFinal;

            PlayerPrefs.SetInt(
                ChaveRecorde,
                MelhorPontuacao
            );

            PlayerPrefs.Save();
        }

        MostrarRecorde(conseguiuNovoRecorde);
    }

    private void CarregarRecorde()
    {
        MelhorPontuacao = PlayerPrefs.GetInt(
            ChaveRecorde,
            0
        );
    }

    private void MostrarRecorde(
        bool novoRecorde)
    {
        if (textoRecorde == null)
        {
            Debug.LogWarning(
                "O texto do recorde não foi configurado!",
                gameObject
            );

            return;
        }

        textoRecorde.gameObject.SetActive(true);

        if (novoRecorde)
        {
            textoRecorde.text =
                $"Novo recorde: {MelhorPontuacao}!";
        }
        else
        {
            textoRecorde.text =
                $"Melhor pontuação: {MelhorPontuacao}";
        }
    }

    [ContextMenu("Apagar recorde salvo")]
    private void ApagarRecorde()
    {
        PlayerPrefs.DeleteKey(ChaveRecorde);
        PlayerPrefs.Save();

        MelhorPontuacao = 0;

        Debug.Log("Recorde apagado!");
    }
}