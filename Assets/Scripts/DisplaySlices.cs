using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class DisplaySlices : MonoBehaviour {
        
        // Listado de los recortes de la resonancia
        [SerializeField] private List<Slice> allSlices;


        // Metodo de llamada de Unity, se llama una vez al iniciar el aplicativo
        private void Awake(){
            CleanSlices();
        }


        // Metodo que permite limpiar la seccion de recorte, en la ventana de recorte
        public void CleanSlices(){
            // Se recorre la lista de todos los recortes para asignar la vista
            foreach(Slice currentSlice in allSlices) {
                    currentSlice.PaintResonance();
            }
        }


        // Metodo que permite pintar la seccion de recorte, en la ventana de recorte
        public void PaintSlices(int value, List<GameObject> goSlice){
            if(allSlices.Count != goSlice.Count) return;

            // Se recorre la lista de todos los recortes para asignar la vista
            for (int i = 0; i < allSlices.Count; i++)
            {
                // Se valida si el GO dispone de un componente MeshRenderer""
                GameObject currentSlice = goSlice[i];
                if (!currentSlice.TryGetComponent(out MeshRenderer meshRenderer)) continue;

                allSlices[i].PaintResonance(meshRenderer.sharedMaterial);

                // Una vez pintado, se procede a desactivar el GO de los recortes
                currentSlice.SetActive(false);
            }
        }
    }
