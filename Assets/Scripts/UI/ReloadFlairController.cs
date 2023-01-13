using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadFlairController : MonoBehaviour
{
    public Image reloadImage;

    public void Start()
    {
        FormController.Instance.reloadFlairController = this;
    }

    public void SetReloadPercentage(float input)
    {
        reloadImage.fillAmount = input;
    }

}
