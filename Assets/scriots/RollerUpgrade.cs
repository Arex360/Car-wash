using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerUpgrade : MonoBehaviour
{
    public List<GameObject> rollerRender;
    public int level;
    void Start()
    {
        foreach (GameObject roller in rollerRender) roller.SetActive(false);
        level = PlayerPrefs.GetInt("roller", 0);
        rollerRender[level].SetActive(true);
    }
    private void OnEnable()
    {
        Refresh();
    }
    public void Refresh()
    {
        foreach (GameObject roller in rollerRender) roller.SetActive(false);
        level = PlayerPrefs.GetInt("roller", 0);
        rollerRender[level].SetActive(true);
    }
}
