using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CarWashSystem : MonoBehaviour
{
    public Transform stand;
    public MeshRenderer carRenderer;
    public ParticleSystem water;
    public Material material;
    public float washRate;
    public float fill;
    public float rotationSpeed;
    public TMPro.TextMeshProUGUI progressPercentage;
    public UnityEngine.UI.Image fillMeter;
    private float defaultFill;
    public float percentage;
    public TMPro.TextMeshProUGUI upgradeLevelCounter;
    public TMPro.TextMeshProUGUI rollerUpgradeLevelCounter;
    public UnityEngine.UI.Button rollerUpgradeBtn, useRollerBtn,rollerUnlockedBtn;
    public bool rollerLocked;
    private bool _completed;
    public bool completed { get { return _completed; } 
        set
        {
            if (value == true)
            {
                foreach(GameObject effect in effectEffects)
                {
                    effect.SetActive(false);
                }
                foreach (GameObject smoke in smokeEffects) smoke.SetActive(false);
                onComplete.Invoke();
            }
            _completed = value;
        }
    }
    public float target;
    public List<GameObject> effectEffects;
    public List<GameObject> smokeEffects;
    public bool RollerMode;
    public UnityEvent onComplete;
    public UnityEvent onStartWash;
    private int Level;
    private bool started;
    public UpgradeRate upgradeRate;
    public static CarWashSystem instance;
    void Start()
    {
        instance = this;
        Level = PlayerPrefs.GetInt("level", 1);
        upgradeLevelCounter.text = Level.ToString();
        rollerUpgradeLevelCounter.text = PlayerPrefs.GetInt("roller", 0).ToString();
        material = carRenderer.materials[0];
        fill = material.GetFloat("_progress");
        defaultFill = fill;
        foreach (GameObject effect in effectEffects) effect.SetActive(false);
        foreach (GameObject smoke in smokeEffects) smoke.SetActive(false);
        bool rollerUnlocked = PlayerPrefs.GetInt("rLocked", 0) == 1 ? true : false;
        rollerUpgradeBtn.interactable = useRollerBtn.interactable = rollerUnlocked;
        rollerLocked = !rollerUnlocked;
        rollerUnlockedBtn.gameObject.SetActive(rollerLocked);
    }
    public void StartWash()
    {
        washRate = upgradeRate.rate[Level-1];
        for(int i = 0; i < Level; i++)
        {
            effectEffects[i].SetActive(true);
        }
        started = true;
        StartCoroutine(StartSmoke());
        Roller.instance.StartRoller();
        onStartWash.Invoke();
        if (RollerMode)
        {
            washRate += upgradeRate.rollerRate;
        }
    }
    private IEnumerator StartSmoke()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject smoke in smokeEffects)
        {
            yield return new WaitForSeconds(0.7f);
            smoke.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!started) return;
        fill -= washRate * Time.deltaTime;
        material.SetFloat("_progress", fill);
        if(!completed && started && !RollerMode)
            stand.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        percentage = (1 - (fill / defaultFill))/2;
        percentage = Mathf.Clamp(percentage, 0, 1);
        progressPercentage.text = $"{Mathf.Floor(percentage * 100)}%";
        fillMeter.fillAmount = percentage;
        if (!completed)
        {
            if(fillMeter.fillAmount >= 1)
            {
                completed = true;
            }
        }
    }
    public void UpgradeWash()
    {
        ConfirmationSystem.instance.ShowConfimation("Upgrade Washing", 100, Upgrade);
    }
    private void Upgrade()
    {
        Level++;
        Level = Mathf.Clamp(Level, 1, effectEffects.Count);
        PlayerPrefs.SetInt("level", Level);
        upgradeLevelCounter.text = PlayerPrefs.GetInt("level").ToString();
    }
    public void UseRollerMode() => RollerMode = true;
    public void PayForRoller()
    {
        ConfirmationSystem.instance.ShowConfimation("Unlock the Roller", 100, UnlockTheRoller);
    }
    private void UnlockTheRoller()
    {
        PlayerPrefs.SetInt("rLocked", 1);
        bool rollerUnlocked = PlayerPrefs.GetInt("rLocked", 0) == 1 ? true : false;
        rollerUpgradeBtn.interactable = useRollerBtn.interactable = rollerUnlocked;
        rollerLocked = !rollerUnlocked;
        rollerUnlockedBtn.gameObject.SetActive(rollerLocked);
    }
    public void UpgradeRoller()
    {
        ConfirmationSystem.instance.ShowConfimation("Upgrade the roller", 200, _UpgradeRoller);
    }
    private void _UpgradeRoller()
    {
        int rollerLevel = PlayerPrefs.GetInt("roller", 0);
        rollerLevel++;
        rollerLevel = Mathf.Clamp(rollerLevel, 0, 2);
        PlayerPrefs.SetInt("roller", rollerLevel);
        RollerUpgrade[] rollers = FindObjectsOfType<RollerUpgrade>();
        foreach (RollerUpgrade roller in rollers) roller.Refresh();
        rollerUpgradeLevelCounter.text = PlayerPrefs.GetInt("roller", 0).ToString();
    }
}
