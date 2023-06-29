using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class a : MonoBehaviour
{
    public PlayerInput playerInput;

    private bool isWaitingForKeyPress = false;

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log(playerInput.actions["Attack"].GetBindingDisplayString());

        }

        if (isWaitingForKeyPress)
        {
            var mouse = Mouse.current;
            if (mouse != null)
            {
                foreach (var button in mouse.allControls)
                {
                    if (button is ButtonControl buttonControl && buttonControl.wasPressedThisFrame)
                    {
                        isWaitingForKeyPress = false;
                        playerInput.actions["Attack"].ApplyBindingOverride($"<Mouse>/{buttonControl.name}");
                        Debug.Log($"Mouse button '{buttonControl.name}' binding set.");
                        return;
                    }
                }
            }

            foreach (var device in InputSystem.devices)
            {
                foreach (var control in device.allControls)
                {
                    if (control is KeyControl keyControl && keyControl.wasPressedThisFrame)
                    {
                        isWaitingForKeyPress = false;
                        playerInput.actions["Attack"].ApplyBindingOverride(keyControl.path);
                        Debug.Log($"Key binding set to: {keyControl.path}");
                        return;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isWaitingForKeyPress = true;
            Debug.Log("Press a key to bind...");
        }
    }
}

