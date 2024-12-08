using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static void OnInitAppScene() {
        SceneManager.LoadScene("Ingreso");
    }

    public static void OnRegisterAppScene(){
        SceneManager.LoadScene("Registro");
    }

}
