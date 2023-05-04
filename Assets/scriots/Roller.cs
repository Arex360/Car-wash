using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public static Roller instance;
    public Transform leftRoller;
    public Transform rightRoller;
    public List<GameObject> rollerDecorations;
    public float speed;
    public bool started;
    void Start()
    {
        instance = this;
        leftRoller.gameObject.SetActive(false);
        rightRoller.gameObject.SetActive(false);
    }
    public void StartRoller()
    {
        if (!CarWashSystem.instance.RollerMode) return;
        leftRoller.gameObject.SetActive(true);
        rightRoller.gameObject.SetActive(true);
        foreach (GameObject dec in rollerDecorations) dec.SetActive(false);
        started = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!started) return;
        leftRoller.Rotate(0,0 , speed * Time.deltaTime);
        rightRoller.Rotate(0, 0, speed * Time.deltaTime);

    }
}
