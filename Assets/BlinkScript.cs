using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkScript : MonoBehaviour
{
    private Image Image;
    public float TimeBetweenFlashes = 0.5f;
    [SerializeField]
    private bool _flash = true;
    public bool flash
    {
        get { return _flash; }
        set
        {
            _flash = value;
            if (value == true)
                StartCoroutine(Flash());
        }
    }
    void OnValidate()
    {
        flash = _flash;
    }

    void Start()
    {
        //Image = this.GetComponent<Image>();
    }

    IEnumerator Flash()
    {
        Image = this.GetComponent<Image>();
        while (flash)
        {
            Image.enabled = false;
            yield return new WaitForSeconds(TimeBetweenFlashes);
            Image.enabled = true;
            yield return new WaitForSeconds(TimeBetweenFlashes);
        }
    }
}
