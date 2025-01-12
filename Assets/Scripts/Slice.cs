using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Slice
{

        // Componente de imagen para la resonancia
        [SerializeField] private RawImage imgResonance;


        // Permite pintar la seccion de la resonancia
        public void PaintResonance(Material material = null){
            PaintSlice(material);
        }


        // Pinta la rawImage que pasa como parametro
        private void PaintSlice(Material material) {
            bool isActive = material != null;
            imgResonance.material = material;
            imgResonance.gameObject.SetActive(isActive);
        }
 }

