using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public Camera mainCamera, leftCamera, topCamera;

    public void MainCamera()
    {
        DisableResizer();
        topCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        leftCamera.gameObject.SetActive(false);
    }

    public void TopCamera()
    {
        DisableResizer();
        topCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        leftCamera.gameObject.SetActive(false);
    }

    public void LeftCamera()
    {
        DisableResizer();
        topCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        leftCamera.gameObject.SetActive(true);
    }

    private void DisableResizer()
    {
        GameObject go = GameObject.Find("Cylinder");
        go.GetComponent<ObjectResizer>().StopResizing();
    }
}
