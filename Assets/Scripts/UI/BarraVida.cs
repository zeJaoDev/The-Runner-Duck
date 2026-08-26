using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public int VidaMax { set
    { this.slider.maxValue = value; }  
    }

    public int Vida { set
    { this.slider.value = value; }
    }

}
