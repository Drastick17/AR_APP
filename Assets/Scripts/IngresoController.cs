using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IngresoController : MonoBehaviour
{

    private string user = "";
    private string password = "";



    public void OnChangeUserField(string value) {
        this.user = value;
    }

    public void OnChangePasswordField(string value)
    {
        this.password = value;
    }

    public void OnLoginUser() {
        print(user+" "+password);
    }


}
