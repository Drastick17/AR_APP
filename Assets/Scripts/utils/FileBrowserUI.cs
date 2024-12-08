using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityVolumeRendering;
using System.Collections.Generic;
using App.Helper;


namespace App.FileBrowserUI
{
    public class FileBrowserUI : MonoBehaviour
    {
        // Referencia del transform donde se anclan los botones de ruta principal
        [SerializeField] private Transform pnlContent;

        [Header("Prefab button")]
        // Referencia del boton prefabricado que hace referencia a un dispositivo de almacenamiento
        [SerializeField] private GameObject prefabBtnDrivePath;

        // Referencia del boton prefabricado que hace referencia a un archivo
        [SerializeField] private GameObject prefaBtnFile;

        // Referencia del boton prefabricado que permite mostrar un directorio
        [SerializeField] private GameObject prefabBtnDir;

        // Referencia de la etiqueta, que muestra la ruta
        [SerializeField] private TMP_Text PathLabel;

        //Guarda todas las ubicaciones disponibles de la PC en botones
        private readonly List<Button> allButtonDrives = new();


        private string currentDirectory;

        private string selectedDirectory;


        private void UpdatePathLabel(string dir)
        {
            PathLabel.text = dir;
        }


        // Metodo que permite regresar una ruta hacia atras
        public void PathBack()
        {
            // Se valida que la ruta no sea nula o vacia
            if (string.IsNullOrEmpty(currentDirectory)) return;


            DirectoryInfo parentDir = Directory.GetParent(currentDirectory);


            if (parentDir != null)
            {
                UpdatePathLabel(parentDir.FullName);
                currentDirectory = parentDir.FullName;
                LoadDirectoriesAndFiles();
            }
            else
            {
                UpdatePathLabel("");
                ListDrives();
            }

        }

        // Metodo que permite abrir la ruta del escritorio
        public void PathDesktop()
        {
            SetPathDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }


        // Metodo que permite abrir la ruta de documentos
        public void PathDocuments()
        {
            SetPathDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Documents"));
        }

        // Metodo que permite abrir la ruta de descargas
        public void PathDownloads()
        {
            SetPathDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"));
        }



        private void SetPathDirectory(string dir)
        {
            UpdatePathLabel(dir);
            SetPathAnLoad(dir);
        }

        private void SetPathAnLoad(string dirPath)
        {
            currentDirectory = dirPath;
            LoadDirectoriesAndFiles();

        }


        private void ListDrives()
        {
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                // Se crea el nuevo boton para el disco buscado
                GameObject newButtonDrive = Instantiate(prefabBtnDrivePath, pnlContent);

                // Se busca el componente de texto de boton generado, y se le asigna el nombre
                TMP_Text textDrive = newButtonDrive.transform.GetChild(0).GetComponent<TMP_Text>();
                textDrive.text = driveInfo.Name;

                // Se busca el componente de boton del boton generado
                Button newButton = newButtonDrive.GetComponent<Button>();
                newButton.onClick.AddListener(() => SetPathDirectory(driveInfo.Name));

                allButtonDrives.Add(newButton);
            }
        }

        private void CleanPanel()
        {
            foreach (Transform currentButton in pnlContent)
            {
                Destroy(currentButton.gameObject);
            }

        }


        private void LoadDirectoriesAndFiles()
        {
            CleanPanel();

            if (string.IsNullOrEmpty(currentDirectory)) return;

            foreach (string file in Directory.GetFiles(currentDirectory))
            {
                FileInfo fileInfo = new(file);

                //omite los documentos que no son medicos
                if (!MedicalFiles.IsMedicalFile(fileInfo.Extension.ToLower())) continue;

                // Crear un nuevo boton para el archivo
                GameObject newBtnGo = Instantiate(prefabBtnDir, pnlContent);
                TMP_Text newTxtBtn = newBtnGo.transform.GetChild(0)
                    .GetComponent<TMP_Text>();

                newTxtBtn.text = fileInfo.Name;
                // Evento para cargar el archivo cuando se hace clic en el boton
                Button newButton = newBtnGo.GetComponent<Button>();
                // newButton.onClick.AddListener(() => SetPathFile(file));
            }

            // Se recorre todos los directorios dentro de la ruta
            foreach (string dir in Directory.GetDirectories(currentDirectory))
            {
                DirectoryInfo dirInfo = new(dir);
                // Crear un nuevo boton para el directorio
                GameObject newBtnGoDir = Instantiate(prefabBtnDir, pnlContent);
                TMP_Text newTxtBtnDir = newBtnGoDir.transform.GetChild(0)
                    .GetComponent<TMP_Text>();

                newTxtBtnDir.text = dirInfo.Name;
                // Evento para cargar los archivos y directorios dentro del nuevo directorio
                Button newButtonDirectory = newBtnGoDir.GetComponent<Button>();
                newButtonDirectory.onClick.AddListener(() => SetPathDirectory(dir));
            }


        }


        private async void LoadFileOrDirectory()
        {
            // btnLoadFileOrDirectory.interactable = false;
            // Se valida el tipo de busqueda
            try
            {

                if(File.Exists(selectedElement))
                await MedicalFiles.LoadDirectory("");
                await MedicalFiles.LoadFile("");
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }


        private void Awake()
        {
            CleanPanel();
            ListDrives();
        }


    }
}