using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleDeCena : MonoBehaviour
{
    [Header("Telas")]
    [SerializeField] private GameObject telaInicial;
    [SerializeField] private GameObject telaCreditos;
    [SerializeField] private GameObject interfaceDoJogo;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Objetos do jogo")]
    [SerializeField] private GameObject conteudoDoJogo;

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
        Time.timeScale = 1f;

        DefinirObjetoAtivo(telaInicial, false);
        DefinirObjetoAtivo(telaCreditos, false);
        DefinirObjetoAtivo(gameOverPanel, false);

        DefinirObjetoAtivo(interfaceDoJogo, true);
        DefinirObjetoAtivo(conteudoDoJogo, true);
    }

    public void AbrirCreditos()
    {
        Time.timeScale = 0f;

        DefinirObjetoAtivo(telaInicial, false);
        DefinirObjetoAtivo(interfaceDoJogo, false);
        DefinirObjetoAtivo(gameOverPanel, false);
        DefinirObjetoAtivo(conteudoDoJogo, false);

        DefinirObjetoAtivo(telaCreditos, true);
    }

    public void FecharCreditos()
    {
        MostrarTelaInicial();
    }

    public void ReiniciarJogo()
    {
        // Recarrega a cena e inicia
        // diretamente a partida.
        iniciarDiretamente = true;

        RecarregarCena();
    }

    public void VoltarParaTelaInicial()
    {
        // Recarrega tudo e mostra
        // novamente a tela inicial.
        iniciarDiretamente = false;

        RecarregarCena();
    }

    public void SairDoJogo()
    {
        Time.timeScale = 1f;

        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        // Para o Play Mode dentro da Unity.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Fecha o jogo compilado.
        Application.Quit();
#endif
    }

    private void MostrarTelaInicial()
    {
        Time.timeScale = 0f;

        DefinirObjetoAtivo(telaInicial, true);

        DefinirObjetoAtivo(telaCreditos, false);
        DefinirObjetoAtivo(interfaceDoJogo, false);
        DefinirObjetoAtivo(gameOverPanel, false);
        DefinirObjetoAtivo(conteudoDoJogo, false);
    }

    private void RecarregarCena()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private void DefinirObjetoAtivo(
        GameObject objeto,
        bool ativar)
    {
        if (objeto != null)
        {
            objeto.SetActive(ativar);
        }
    }

    private void VerificarConfiguracao()
    {
        if (telaInicial == null)
        {
            Debug.LogWarning(
                "Arraste a TelaInicial para o ControleDeCena.",
                gameObject
            );
        }

        if (telaCreditos == null)
        {
            Debug.LogWarning(
                "Arraste a TelaCreditos para o ControleDeCena.",
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

        if (conteudoDoJogo == null)
        {
            Debug.LogWarning(
                "Arraste o ConteudoDoJogo para o ControleDeCena.",
                gameObject
            );
        }
    }
}