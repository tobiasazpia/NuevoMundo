using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class cController : MonoBehaviour
{
    public PlayerInput py;

    public Button jugar;

    void Start()
    {
        py = GetComponent<PlayerInput>();

        //Button btn = yourButton.GetComponent<Button>();
        jugar.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {

    }
    // Update is called once per frame
    void Update()
    {
        py.actions["Select"].WasPressedThisFrame();
        py.actions["Back"].WasPressedThisFrame();
        /*py.actions["Change Option Up"].WasPressedThisFrame();
        py.actions["Change Option Down"].WasPressedThisFrame();*/

    }

}
