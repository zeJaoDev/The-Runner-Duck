using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleDeCena : MonoBehaviour
{
    [Header("Telas")]
    [SerializeField] private GameObject telaInicial;
    [SerializeField] private GameObject interfaceDoJogo;
    [SerializeField] private GameObject gameOverPanel;

    private static bool iniciarDiretamente;

    private void Awake()
    {
        VerificarConfiguracao();
    }

    private void Start()
    {
        bool deveIniciarDiretamente =
            iniciarDiretamente;

        iniciarDiretamente = false;

        if (deveIniciarDiretamente)
        {
            IniciarJogo();
        }
        else
        {
            MostrarTelaInicial();
        }
    }

    public void IniciarJogo()
    {
        if (telaInicial == null)
        {
            Debug.LogError(
                "O painel TelaInicial não foi configurado!",
                gameObject
            );

            return;
        }

        telaInicial.SetActive(false);

        if (interfaceDoJogo != null)
        {
            interfaceDoJogo.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void ReiniciarJogo()
    {
        // Recarrega a cena e inicia a partida
        // sem mostrar a tela inicial.
        iniciarDiretamente = true;

        RecarregarCena();
    }

    public void VoltarParaTelaInicial()
    {
        // Recarrega tudo e volta para o menu.
        iniciarDiretamente = false;

        RecarregarCena();
    }

    private void MostrarTelaInicial()
    {
        Time.timeScale = 0f;

        if (telaInicial != null)
        {
            telaInicial.SetActive(true);
        }

        if (interfaceDoJogo != null)
        {
            interfaceDoJogo.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void RecarregarCena()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void VerificarConfiguracao()
    {
        if (telaInicial == null)
        {
            Debug.LogWarning(
                "Arraste o painel TelaInicial para o ControleDeCena.",
                gameObject
            );
        }

        if (interfaceDoJogo == null)
        {
            Debug.LogWarning(
                "Arraste a InterfaceDoJogo para o ControleDeCena.",
                gameObject
            );
        }

        if (gameOverPanel == null)
        {
            Debug.LogWarning(
                "Arraste o GameOverPanel para o ControleDeCena.",
                gameObject
            );
        }
    }
}