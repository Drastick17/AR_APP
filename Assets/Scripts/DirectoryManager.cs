using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityVolumeRendering;
using System.Collections.Generic;


namespace DirectoryManagement
{

    // Script que permite administrar el directorio de resonancia a buscar y cargar
    public class DirectoryManager : MonoBehaviour
    {

        [Header("GameObject UI")]
        // Referencia del canvas principal donde se adjunta el directorio
        [SerializeField] private GameObject pnlCanvasDirectory;

        // Referencia del panel que realiza la busqueda de directorios y ficheros
        [SerializeField] private GameObject pnlContainerPath;

        // Referencia del componente scroll donde se adjuntan los botones de ruta
        [SerializeField] private GameObject scrollViewMainPath;

        // Referencia al panel donde se adjunta el mensaje de advertencia
        [SerializeField] private GameObject pnlMessageError;

        // Referencia del transform donde se anclan los botones de ruta principal
        [SerializeField] private Transform pnlMainPathButton;

        // Referencia del transform donde se anclan los nuevos botones
        [SerializeField] private Transform contentDirectory;


        [Header("Component UI")]
        // Componente de texto, donde se muestra la ruta completa actual
        [SerializeField] private TMP_Text txtPathCurrentDirectory;

        // Referencia del componente de texto para mostrar el respectivo mensaje
        [SerializeField] private TMP_Text txtError;

        // Referencia del componente de linea de sombreado del boton nii
        [SerializeField] private Outline outlineBtnImportNii;

        // Referencia del componente de linea de sombreado del boton dicom
        [SerializeField] private Outline outlineBtnImportDicom;

        // Referencia del boton que permite cargar el fichero selecionado
        [SerializeField] private Button btnLoadFileOrDirectory;


        [Header("Prefab button")]
        // Referencia del boton prefabricado que permite cargar la ruta
        [SerializeField] private GameObject prefabBtnPath;

        // Referencia del boton prefabricado que permite cargar el directorio raiz
        [SerializeField] private GameObject prefabBtnDir;


        // Referencia del componente de texto del boton de carga de fichero o directorio
        private TMP_Text txtBtnLoadFileOrDir;

        // Se almacena la ruta del directorio
        private string currentDirectory;

        // Se almacena la ruta de carga del fichero o directorio
        private string currentFile;

        // Si es verdadero, busca ficheros .nii, caso contrario debe buscar directorios DICOM
        private bool browseFileNii;

        // Lista que almacena los botones de los discos dentro de la PC: C:/, D:/ etc.
        private readonly List<Button> allButtonDrives = new();



        // Metodo de llamada de Unity, se llama una unica vez al iniciar el aplicativo
        // Se configura la logica de eventos de los botones del administrador de directorio
        private void Awake()
        {
            // Se asigna el componente de texto para los respectivos mensajes de boton de carga
            txtBtnLoadFileOrDir = btnLoadFileOrDirectory.transform.GetChild(0)
                .GetComponent<TMP_Text>();

            // Se emplea para acceder a todos los discos del computador y crear referencias
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                // Se crea el nuevo boton para el disco buscado
                GameObject newButtonDrive = Instantiate(prefabBtnPath, pnlMainPathButton);

                // Se busca el componente de texto de boton generado, y se le asigna el nombre
                TMP_Text textDrive = newButtonDrive.transform.GetChild(0).GetComponent<TMP_Text>();
                textDrive.text = driveInfo.Name;

                // Se busca el componente de boton del boton generado
                Button newButton = newButtonDrive.GetComponent<Button>();
                newButton.onClick.AddListener(() => SetPathDirectory(driveInfo.Name));
                allButtonDrives.Add(newButton);
            }
        }


        // Metodo de llamada de Unity, se activa cuando se destruye el objeto
        private void OnDestroy()
        {
            // Se limpia el arreglo
            if (allButtonDrives.Count == 0 || allButtonDrives == null) return;
            foreach (Button button in allButtonDrives)
            {
                button.onClick.RemoveAllListeners();
            }
            allButtonDrives.Clear();
        }


        // Metodo de llamada de Unity, se llama una vez al iniciar el aplicativo, despues de Awake
        // Se procede a ocular las linea de sombreado
        private void Start()
        {
            // Se desabilitan los paneles correspondientes
            pnlMessageError.SetActive(false);
            pnlCanvasDirectory.SetActive(false);
        }


        // Metodo que permite iniciar el administrador de directorios
        public void InitManagerDirectory()
        {
            outlineBtnImportNii.enabled = false;
            outlineBtnImportDicom.enabled = false;

            pnlContainerPath.SetActive(false);
            pnlCanvasDirectory.SetActive(true);
        }


        // Metodo que permite buscar ficheros en base al formato especificado
        public void BrowserFile(bool value)
        {
            browseFileNii = value;
            outlineBtnImportNii.enabled = value;
            outlineBtnImportDicom.enabled = !value;

            pnlContainerPath.SetActive(true);
            scrollViewMainPath.SetActive(false);

            txtBtnLoadFileOrDir.text = value ? "Cargar archivo .nii" : "Cargar directorio DICOM";

            btnLoadFileOrDirectory.interactable = !value;

            // Se reinicia las rutas de los ficheros y directorios
            currentDirectory = "";
            currentFile = "";
            txtPathCurrentDirectory.text = "";
        }


        // Metodo que permite cargar la ruta anterior, regresa una ruta hacia atras
        public void PathBack()
        {
            // Se valida que la ruta no sea nula o vacia
            if (!string.IsNullOrEmpty(currentDirectory))
            {
                // Se fija el directorio hacia atras
                DirectoryInfo parentDir = Directory.GetParent(currentDirectory);
                if (parentDir != null)
                {
                    currentDirectory = parentDir.FullName;
                    LoadDirectoriesAndFiles();
                }
            }
            currentFile = "";
            btnLoadFileOrDirectory.interactable = false;
        }


        // Metodo que permite abrir la ruta del escritorio, desde el buscador de ficheros
        public void PathDesktop()
        {
            SetPathAndLoad(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }


        // Metodo que permite abrir la ruta de documentos, desde el buscador de ficheros
        public void PathDocuments()
        {
            SetPathAndLoad(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
        }


        // Metodo que permite abrir el directorio segun la ruta especifica 
        private void SetPathDirectory(string dir)
        {
            SetPathAndLoad(dir);
        }


        // Metodo generico para establecer el directorio actual y cargar los directorios y archivos
        private void SetPathAndLoad(string directoryPath)
        {
            btnLoadFileOrDirectory.interactable = !browseFileNii;
            currentDirectory = directoryPath;
            LoadDirectoriesAndFiles();
        }


        // Metodo que permite fijar el fichero o directorio de carga
        private void SetPathFile(string filePath)
        {
            currentFile = filePath;
            txtPathCurrentDirectory.text = filePath;
            // Se habilita el boton que permite cargar los ficheros .nii
            btnLoadFileOrDirectory.interactable = true;
        }


        // Se emplea para poder cargar los ficheros y directorios de la ruta seleccionada
        private void LoadDirectoriesAndFiles()
        {
            try
            {
                // Limpiar el contenido actual antes de mostrar nuevos botones
                ClearContent();
                scrollViewMainPath.SetActive(true);
                txtPathCurrentDirectory.text = currentDirectory;

                // Se valida que la ruta no sea nula o vacia
                if (string.IsNullOrEmpty(currentDirectory)) return;

                // Se estan buscando ficheros .nii
                if (browseFileNii)
                {
                    // Se recorre todos los archivos dentro de la ruta
                    foreach (string file in Directory.GetFiles(currentDirectory))
                    {
                        FileInfo fileInfo = new(file);
                        // Verificar si el archivo tiene la extension ".nii"
                        if (fileInfo.Extension.ToLower() == ".nii")
                        {
                            // Crear un nuevo boton para el archivo
                            GameObject newBtnGo = Instantiate(prefabBtnDir, contentDirectory);
                            TMP_Text newTxtBtn = newBtnGo.transform.GetChild(0)
                                .GetComponent<TMP_Text>();

                            newTxtBtn.text = fileInfo.Name;
                            // Evento para cargar el archivo cuando se hace clic en el boton
                            Button newButton = newBtnGo.GetComponent<Button>();
                            newButton.onClick.AddListener(() => SetPathFile(file));
                        }
                    }
                }

                // Se recorre todos los directorios dentro de la ruta
                foreach (string dir in Directory.GetDirectories(currentDirectory))
                {
                    DirectoryInfo dirInfo = new(dir);
                    // Crear un nuevo boton para el directorio
                    GameObject newBtnGoDir = Instantiate(prefabBtnDir, contentDirectory);
                    TMP_Text newTxtBtnDir = newBtnGoDir.transform.GetChild(0)
                        .GetComponent<TMP_Text>();

                    newTxtBtnDir.text = dirInfo.Name;
                    // Evento para cargar los archivos y directorios dentro del nuevo directorio
                    Button newButtonDirectory = newBtnGoDir.GetComponent<Button>();
                    newButtonDirectory.onClick.AddListener(() => SetPathDirectory(dir));
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }


        // Metodo que permite limpiar el contenido de botones
        private void ClearContent()
        {
            foreach (Transform currentButton in contentDirectory)
            {
                Destroy(currentButton.gameObject);
            }
        }


        // Metodo que permite cargar el fichero o directorio de la resonancia a la Escena
        public void LoadFileOrDirectory()
        {
            btnLoadFileOrDirectory.interactable = false;
            // Se valida el tipo de busqueda
            if (browseFileNii)
            {
                // Se esta buscando ficheros .nii
                LoadFile(currentFile);
            }
            else
            {
                // Se busca ficheros DICOM
                LoadDirectoryDicom(currentDirectory);
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
                // Si los datos son validos se carga
                if (dataset != null)
                {
                    VolumeRenderedObject vol = await VolumeObjectFactory.CreateObjectAsync(dataset);
                    btnLoadFileOrDirectory.interactable = true;
                    pnlCanvasDirectory.SetActive(false);
                    //DisplaySetting.DisSingleton.SetResonance(vol);
                }
                else
                {
                    txtError.text = "<color=red><b>Error: Fallo de carga.</b></color>\n";
                    txtError.text += "No se pudo importar el conjunto de datos.";
                    pnlMessageError.SetActive(true);
                    // Debug.LogError("Failed to import datset");
                }
            }
            catch (Exception ex)
            {
                txtError.text = "<color=red><b>Error: Archivo no válido.</b></color>\n";
                txtError.text += "Selecciona un archivo con formato '.nii' para continuar.";
                pnlMessageError.SetActive(true);
                // Debug.LogError(ex.Message);
            }
        }


        // Metodo que permite cargar el directorio que contiene los ficheros DICOM
        private async void LoadDirectoryDicom(string filePath)
        {
            try
            {
                // Se da lectura a todos los ficheros
                IEnumerable<string> fileCandidates = Directory.EnumerateFiles(filePath, "*.*",
                            true ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
                    .Where(p => p.EndsWith(".dcm", StringComparison.InvariantCultureIgnoreCase) ||
                    p.EndsWith(".dicom", StringComparison.InvariantCultureIgnoreCase) ||
                    p.EndsWith(".dicm", StringComparison.InvariantCultureIgnoreCase));

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
                    // Aparece el objeto
                    if (dataset != null)
                    {
                        VolumeRenderedObject obj = await VolumeObjectFactory
                                .CreateObjectAsync(dataset);
                        obj.transform.position = new Vector3(numVolumesCreated, 0, 0);
                        numVolumesCreated++;
                        //DisplaySetting.DisSingleton.SetResonance(obj);
                        // Se procede a saltar del bucle para evitar cargar mas de una carpeta DICOM
                        break;
                    }
                }
                btnLoadFileOrDirectory.interactable = true;
                pnlCanvasDirectory.SetActive(false);
            }
            catch (Exception ex)
            {
                txtError.text = "<color=red><b>Error: Archivos no válidos.</b></color>\n";
                txtError.text += "Asegúrate de que el directorio contenga archivos DICOM.";
                pnlMessageError.SetActive(true);
                Debug.LogError(ex.Message);
            }
        }
    }
}