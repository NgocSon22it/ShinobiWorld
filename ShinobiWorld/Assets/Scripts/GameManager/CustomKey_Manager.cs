using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;

public class CustomKey_Manager : MonoBehaviour
{
    [SerializeField] TMP_Text SkillOneTxt;
    [SerializeField] TMP_Text SkillTwoTxt;
    [SerializeField] TMP_Text SkillThreeTxt;

    [SerializeField] GameObject CustomKeyPanel;

    public void LoadPlayerKey()
    {
        if (Game_Manager.Instance.PlayerManager != null)
        {
            PlayerBase ScriptReference = Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>();

            if (ScriptReference.SkillOne_Entity != null)
            {
                SkillOneTxt.text = ScriptReference.SkillOne_Entity.Key;
            }
            else
            {
                SkillOneTxt.text = "1";
            }

            if (ScriptReference.SkillTwo_Entity != null)
            {
                SkillOneTxt.text = ScriptReference.SkillTwo_Entity.Key;
            }
            else
            {
                SkillOneTxt.text = "1";
            }

            if (ScriptReference.SkillThree_Entity != null)
            {
                SkillOneTxt.text = ScriptReference.SkillThree_Entity.Key;
            }
            else
            {
                SkillOneTxt.text = "1";
            }
        }
        else
        {
            Debug.Log("Null mer");
        }
    }

    public void OpenCustomKeyPanel()
    {
        CustomKeyPanel.SetActive(true);
        Game_Manager.Instance.IsBusy = true;
        LoadPlayerKey();
    }

    public void CloseCustomKeyPanel()
    {
        CustomKeyPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }

    private void Update()
    {
        /* if (isWaitingForKeyPress)
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

         if (Input.GetKeyDown(KeyCode.U))
         {
             isWaitingForKeyPress = true;
             Debug.Log("Press a key to bind...");
         }
     }*/
    }
}
