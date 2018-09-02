using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{

    public GameObject Blinker;
    private int currentSelection = 4;
    private List<int> selectableThroughLeftMove = new List<int>() { 0, 1, 3, 4, 6, 7 };
    private List<int> selectableThroughRightMove = new List<int>() { 1, 2, 4, 5, 7, 8 };
    public float TimeBetweenFlashes = 0.1f;

    public List<Image> StuffToBlinkOnSelect;
    bool starting = false;

    private void Start()
    {
        foreach (var child in GameObject.FindGameObjectsWithTag("BlinkOnSelect"))
        {
            StuffToBlinkOnSelect.Add(child.GetComponent<Image>());
        }
    }

    public enum Boss
    {
        BubbleMan, AirMan, QuickMan,
        HeatMan, Wily, WoodMan,
        MetalMan, FlashMan, CrashMan
        //0   1   2
        //3   4   5
        //6   7   8
    }
    public Boss CurrentBoss(int aBoss)
    {
        return (Boss)aBoss;
    }
    public string GetNameOfCurrentBoss()
    {
        return ((Boss)currentSelection).ToString();
    }

    void Update()
    {
        if (!starting)
        {
            //Up
            if (Input.GetKeyDown(KeyCode.W) && currentSelection - 3 >= 0)
            {
                currentSelection = (currentSelection - 3);
                Blinker.transform.parent = this.gameObject.transform.GetChild(currentSelection);
                Blinker.transform.localPosition = Vector3.zero;
            }
            //Down
            else if (Input.GetKeyDown(KeyCode.S) && currentSelection + 3 < this.gameObject.transform.childCount)
            {
                currentSelection = (currentSelection + 3);
                Blinker.transform.parent = this.gameObject.transform.GetChild(currentSelection);
                Blinker.transform.localPosition = Vector3.zero;
            }
            //Left
            else if (Input.GetKeyDown(KeyCode.A) && selectableThroughLeftMove.Contains(currentSelection - 1))
            {
                currentSelection = (currentSelection - 1);
                Blinker.transform.parent = this.gameObject.transform.GetChild(currentSelection);
                Blinker.transform.localPosition = Vector3.zero;
            }
            //Right
            else if (Input.GetKeyDown(KeyCode.D) && selectableThroughRightMove.Contains(currentSelection + 1))
            {
                currentSelection = (currentSelection + 1);
                Blinker.transform.parent = this.gameObject.transform.GetChild(currentSelection);
                Blinker.transform.localPosition = Vector3.zero;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                SelectBoss();
        }
    }

    public void SelectBoss()
    {
        starting = true;
        StartCoroutine(Flash(3));
        //GoTo next scene baserat på CurrentBoss(currentSelection)
    }

    IEnumerator Flash(int aTimesToFlash)
    {
        while (aTimesToFlash > 0)
        {
            foreach (var blinker in StuffToBlinkOnSelect)
            {
                blinker.enabled = true;
            }
            yield return new WaitForSeconds(TimeBetweenFlashes);
            foreach (var blinker in StuffToBlinkOnSelect)
            {
                blinker.enabled = false;
            }
            yield return new WaitForSeconds(TimeBetweenFlashes);
            aTimesToFlash--;
        }
    }
}
