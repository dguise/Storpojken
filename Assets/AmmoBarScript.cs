using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBarScript : MonoBehaviour {

    [Range(0, 27)]
    public float currentAmmo;
    public Sprite Background;
    public Sprite ImageToTile;
    public List<GameObject> bars;

    private void OnGUI()
    {
        bars.ForEach(b => b.SetActive(false));

        int ammoCeiled = Mathf.CeilToInt(currentAmmo);
        int ammoFloored = Mathf.FloorToInt(currentAmmo);

        for (int i = 1; i < currentAmmo ; i++)
        {
            bars[i].SetActive(true);
            //bars[bars.Count - i].SetActive(true);
        }
    }
}
