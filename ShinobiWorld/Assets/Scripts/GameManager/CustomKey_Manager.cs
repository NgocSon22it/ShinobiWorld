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
    string KeyboardExtension = "/Keyboard/";

    private bool isWaitingForKeyPress = false;

    PlayerBase ScriptReference;
    public void LoadPlayerKey()
    {
        if (Game_Manager.Instance.PlayerManager != null)
        {
            ScriptReference = Game_Manager.Instance.PlayerManager.GetComponent<PlayerBase>();

            if (ScriptReference.SkillOne_Entity != null)
            {
                ListSkillTxt[0].text = ShowKey(ScriptReference.SkillOne_Entity.Key);
            }
            else
            {
                ListSkillTxt[0].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "One").Key);
            }

            if (ScriptReference.SkillTwo_Entity != null)
            {
                ListSkillTxt[1].text = ShowKey(ScriptReference.SkillTwo_Entity.Key);
            }
            else
            {
                ListSkillTxt[1].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Two").Key);
            }

            if (ScriptReference.SkillThree_Entity != null)
            {
                ListSkillTxt[2].text = ShowKey(ScriptReference.SkillThree_Entity.Key);
            }
            else
            {
                ListSkillTxt[2].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Three").Key);
            }
        }

    }

    public string ShowKey(string Key)
    {
        return Key.Replace(KeyboardExtension, "").ToUpper();
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
            case 0:
                if (ScriptReference.SkillOne_Entity != null)
                {
                    SetUpKey(Key);
                }
                else
                {
                    Message.text = "Bạn chưa mở chiêu đó đổi ko được.";
                }
                break;
            case 1:
                if (ScriptReference.SkillTwo_Entity != null)
                {
                    SetUpKey(Key);
                }
                else
                {
                    Message.text = "Bạn chưa mở chiêu đó đổi ko được.";
                }
                break;
            case 2:
                if (ScriptReference.SkillThree_Entity != null)
                {
                    SetUpKey(Key);
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
        if (IsThatKeyInUse(NewKey))
        {
            Message.text = "Phím đó sử dụng rồi!";
            isWaitingForKeyPress = true;
        }
        else
        {
            Message.text = "";
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

            ListSkillTxt[IndexKey].text = NewKey.Replace(KeyboardExtension, "").ToUpper();
            Game_Manager.Instance.ReloadPlayerProperties();
        }
    }

    public bool IsThatKeyInUse(string NewKey)
    {
        for (int i = 0; i < ListSkillTxt.Count; i++)
        {
            if (ShowKey(NewKey).Equals(ListSkillTxt[i].text))
            {
                return true;
            }
        }
        return false;
    }

    public void SetDefaultKey()
    {
        ListSkillTxt[0].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "One").Key);
        ListSkillTxt[1].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Two").Key);
        ListSkillTxt[2].text = ShowKey(References.listSkill.Find(obj => obj.ID == "Skill_" + Game_Manager.Instance.Role + "Three").Key);

        ScriptReference.playerInput.actions["SkillOne"].ApplyBindingOverride(KeyboardExtension + "q");
        ScriptReference.playerInput.actions["SkillTwo"].ApplyBindingOverride(KeyboardExtension + "e");
        ScriptReference.playerInput.actions["SkillThree"].ApplyBindingOverride(KeyboardExtension + "space");

        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "One", KeyboardExtension + "q");
        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Two", KeyboardExtension + "e");
        HasSkill_DAO.ChangeKey(References.accountRefer.ID, "Skill_" + Game_Manager.Instance.Role + "Three", KeyboardExtension + "space");

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
                        return;
                    }
                }
            }
        }
    }
}
