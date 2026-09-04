using TMPro;
using UnityEngine;

public class PontuacaoFinal : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textoPontuacaoFinal;

    public void MostrarPontuacao(int pontos)
    {
        if (textoPontuacaoFinal == null)
        {
            Debug.LogWarning(
                "O texto da pontuação final não foi configurado!"
            );

            return;
        }

        textoPontuacaoFinal.text =
            $"Pontuação final: {pontos}";
    }
}