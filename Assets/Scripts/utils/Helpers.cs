using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityVolumeRendering;


namespace App.Helper
{
        
    public class MedicalFiles
    {

        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>
    {
        "nii",
        "dcm",
        "dicom",
        "dicm",
    };

        // Método para verificar si un archivo es médico
        public static bool IsMedicalFile(string filePath)
        {
            return AllowedExtensions.Contains(Path.GetExtension(filePath).ToLower());
        }
    

    // Método para cargar un directorio de archivos .dcm
    private async void LoadDirectory(string filePath)
        {
            try
            {
                // Se da lectura a todos los ficheros
                IEnumerable<string> fileCandidates = Directory.EnumerateFiles(filePath, "*.*", true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    ?.Where(medicalFile => IsMedicalFile(medicalFile));

                // Importa el conjunto de datos
                IImageSequenceImporter importer = ImporterFactory
                        .CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
                IEnumerable<IImageSequenceSeries> seriesList = await importer
                        .LoadSeriesAsync(fileCandidates);

                float numVolumesCreated = 0;

                // Se recorre la secuencia de imagenes para poder armar el modelo de resonancia
                foreach (IImageSequenceSeries series in seriesList)
                {
                    VolumeDataset dataset = await importer.ImportSeriesAsync(series);

                    if (dataset == null) continue;

                    // Aparece el objeto
                    VolumeRenderedObject obj = await VolumeObjectFactory
                            .CreateObjectAsync(dataset);
                    obj.transform.position = new UnityEngine.Vector3(numVolumesCreated, 0, 0);
                    //DisplaySetting.DisSingleton.SetResonance(obj);
                    numVolumesCreated++;

                    // Se procede a saltar del bucle para evitar cargar mas de una carpeta DICOM
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }



        // Metodo que permite cargar el directorio que contiene los ficheros Nii
        private async void LoadFile(string filePath)
        {
            try
            {
                // Logica para cargar el fichero .nii
                IImageFileImporter importer = ImporterFactory
                        .CreateImageFileImporter(ImageFileFormat.NIFTI);
                VolumeDataset dataset = await importer.ImportAsync(filePath);


                if (dataset == null) return;

                // Si los datos son validos se carga
                VolumeRenderedObject vol = await VolumeObjectFactory.CreateObjectAsync(dataset);

            }
            catch (Exception ex)
            {
                // txtError.text = "<color=red><b>Error: Archivo no válido.</b></color>\n";
                // txtError.text += "Selecciona un archivo con formato '.nii' para continuar.";
                // pnlMessageError.SetActive(true);
                Debug.LogError(ex.Message);
            }
        }
    }

}