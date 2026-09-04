using TMPro;
using UnityEngine;

public class ContadorPontos : MonoBehaviour
{
    [Header("Interface durante o jogo")]
    [SerializeField] private TMP_Text textoPontos;
    [SerializeField] private GameObject interfaceDuranteJogo;

    [Header("Pontuação")]
    [SerializeField] private float pontosPorSegundo = 1f;

    private float pontuacao;
    private bool contando = true;

    public int PontosAtuais =>
        Mathf.FloorToInt(pontuacao);

    private void Awake()
    {
        if (interfaceDuranteJogo == null &&
            textoPontos != null)
        {
            interfaceDuranteJogo =
                textoPontos.gameObject;
        }
    }

    private void Start()
    {
        pontuacao = 0f;
        contando = true;

        AtualizarTexto();
    }

    private void Update()
    {
        if (!contando)
        {
            return;
        }

        pontuacao +=
            pontosPorSegundo * Time.deltaTime;

        AtualizarTexto();
    }

    public void AdicionarPontos(float quantidade)
    {
        if (!contando)
        {
            return;
        }

        pontuacao += quantidade;

        AtualizarTexto();
    }

    public int FinalizarPontuacao()
    {
        contando = false;

        int resultadoFinal = PontosAtuais;

        if (interfaceDuranteJogo != null)
        {
            interfaceDuranteJogo.SetActive(false);
        }

        return resultadoFinal;
    }

    private void AtualizarTexto()
    {
        if (textoPontos == null)
        {
            return;
        }

        textoPontos.text =
            $"Pontos: {PontosAtuais}";
    }
}