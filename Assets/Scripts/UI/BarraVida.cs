using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Slider slider;

    private void Awake()
    {
        ProcurarSlider();
    }

    public void SetVidaMaxima(float vida)
    {
        if (slider == null)
        {
            return;
        }

        slider.minValue = 0f;
        slider.maxValue = vida;
        slider.value = vida;
    }

    public void AlterarVida(float vida)
    {
        if (slider == null)
        {
            return;
        }

        slider.value = Mathf.Clamp(
            vida,
            slider.minValue,
            slider.maxValue
        );
    }

    private void ProcurarSlider()
    {
        if (slider == null)
        {
            slider = GetComponentInChildren<Slider>(true);
        }

        if (slider == null)
        {
            Debug.LogError(
                "Nenhum Slider foi encontrado na BarraVida!",
                gameObject
            );
        }
    }
}