using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Manager : MonoBehaviour
{
    [SerializeField] GameObject PortalPanel;

    public static Portal_Manager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        Game_Manager.Instance.IsBusy = true;
        PortalPanel.SetActive(true);
    }

    public void Close()
    {
        PortalPanel.SetActive(false);
        Game_Manager.Instance.IsBusy = false;
    }
}
