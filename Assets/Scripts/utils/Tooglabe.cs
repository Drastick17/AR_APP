using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooglabe : MonoBehaviour
{
    [SerializeField] private GameObject MenuObject;

    public void ToogleMenu()
    {
        if (MenuObject.activeInHierarchy)
        {
            MenuObject.SetActive(false);
        }
        else
        {
            MenuObject.SetActive(true);
        }
    }
}
