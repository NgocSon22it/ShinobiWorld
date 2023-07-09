using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem;
using System.Data;

public class CustomKey_Manager : MonoBehaviour
{
    [SerializeField] List<TMP_Text> ListSkillTxt;

    [SerializeField] GameObject CustomKeyPanel;

    [SerializeField] TMP_Text Message;

    int IndexKey;

    private bool isWaitingForKeyPress = false;

    PlayerBase ScriptReference;
    public void LoadPlayerKey()
    {
        if (Game_Manager.Instance.PlayerManager != null)
        {
            ScriptReference = Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>();

            if (ScriptReference.SkillOne_Entity != null)
            {
                ListSkillTxt[0].text = ScriptReference.SkillOne_Entity.Key.Replace("/Keyboard/", "").ToUpper();
            }
            else
            {
                ListSkillTxt[0].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "One").Key.Replace("/Keyboard/", "").ToUpper();
            }

            if (ScriptReference.SkillTwo_Entity != null)
            {
                ListSkillTxt[1].text = ScriptReference.SkillTwo_Entity.Key.Replace("/Keyboard/", "").ToUpper();
            }
            else
            {
                ListSkillTxt[1].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Two").Key.Replace("/Keyboard/", "").ToUpper();
            }

            if (ScriptReference.SkillThree_Entity != null)
            {
                ListSkillTxt[2].text = ScriptReference.SkillThree_Entity.Key.Replace("/Keyboard/", "").ToUpper();
            }
            else
            {
                ListSkillTxt[2].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Three").Key.Replace("/Keyboard/", "").ToUpper();
            }
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
        isWaitingForKeyPress = false;
        Game_Manager.Instance.IsBusy = false;
    }

    public void SelectKey(int Key)
    {
        switch (Key)
        {
            case 1:
                if (ScriptReference.SkillOne_Entity != null)
                {
                    SetUpKey(Key - 1);
                }
                else
                {
                    Message.text = "Bạn chưa mở chiêu đó đổi ko được.";
                }
                break;
            case 2:
                if (ScriptReference.SkillTwo_Entity != null)
                {
                    SetUpKey(Key - 1);
                }
                else
                {
                    Message.text = "Bạn chưa mở chiêu đó đổi ko được.";
                }
                break;
            case 3:
                if (ScriptReference.SkillThree_Entity != null)
                {
                    SetUpKey(Key - 1);
                }
                else
                {
                    Message.text = "Bạn chưa mở chiêu đó đổi ko được.";
                }
                break;
        }

    }

    public void SetUpKey(int Key)
    {
        LoadPlayerKey();
        ListSkillTxt[Key].text = "Nhấn vào phím muốn thay đổi";
        IndexKey = Key;
        isWaitingForKeyPress = true;
        Message.text = "";
    }


    public void ChangeKey(int Key, string NewKey)
    {
        switch (Key)
        {
            case 0:
                ScriptReference.playerInput.actions["SkillOne"].ApplyBindingOverride(NewKey);
                HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "One", NewKey);
                break;
            case 1:
                ScriptReference.playerInput.actions["SkillTwo"].ApplyBindingOverride(NewKey);
                HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Two", NewKey);
                break;
            case 2:
                ScriptReference.playerInput.actions["SkillThree"].ApplyBindingOverride(NewKey);
                HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Three", NewKey);
                break;
        }

        ListSkillTxt[IndexKey].text = NewKey.Replace("/Keyboard/", "").ToUpper();
        Game_Manager.Instance.ReloadPlayerProperties();
    }

    public void SetDefaultKey()
    {
        ListSkillTxt[0].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "One").Key.Replace("/Keyboard/", "").ToUpper();
        ListSkillTxt[1].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Two").Key.Replace("/Keyboard/", "").ToUpper();
        ListSkillTxt[2].text = References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Three").Key.Replace("/Keyboard/", "").ToUpper();
        ScriptReference.playerInput.actions["SkillOne"].ApplyBindingOverride("/Keyboard/q");
        ScriptReference.playerInput.actions["SkillTwo"].ApplyBindingOverride("/Keyboard/e");
        ScriptReference.playerInput.actions["SkillThree"].ApplyBindingOverride("/Keyboard/space");
        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "One", "/Keyboard/q");
        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Two", "/Keyboard/e");
        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Three", "/Keyboard/space");
        Game_Manager.Instance.ReloadPlayerProperties();
        isWaitingForKeyPress = false;
    }


    private void Update()
    {
        if (isWaitingForKeyPress)
        {
            foreach (var device in InputSystem.devices)
            {
                foreach (var control in device.allControls)
                {
                    if (control is KeyControl keyControl && keyControl.wasPressedThisFrame)
                    {
                        isWaitingForKeyPress = false;
                        ChangeKey(IndexKey, keyControl.path);

                        Debug.Log($"Key binding set to: {keyControl.path}");
                        return;
                    }
                }
            }
        }
    }
}
