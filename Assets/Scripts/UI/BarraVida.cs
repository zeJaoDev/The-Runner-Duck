using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
  [SerializeField]
private Slider slider;

public int VidaMax
{
set { slider.maxValue = value; }
}

public int Vida
{
set { slider.value = value; }
}
}