using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private bool IsCompsSet { get; set; }


    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        if (!IsCompsSet) { SetComps(); }
        UpdateUI();
    }
    public virtual void UpdateUI()
    {

    }
    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }




    public virtual void SetComps()
    {
        IsCompsSet = true;
    }
}
