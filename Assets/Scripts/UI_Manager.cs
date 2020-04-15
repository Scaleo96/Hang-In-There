using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] speedUpgrades;
    [SerializeField]
    private GameObject[] spreadUpgrades;
    [SerializeField]
    private GameObject[] damageUpgrades;
    [SerializeField]
    private GameObject[] amountUpgrades;

    [SerializeField]
    private GameObject heartMenu;
    
    [SerializeField]
    private Text resources;
    [SerializeField]
    private Text[] health;

    [SerializeField]
    private ProjectileShooterUpgrades ps;
    [SerializeField]
    private MouseAim ma;
    [SerializeField]
    private Player_Manager pm;


    public void OpenHeartMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        heartMenu.SetActive(true);
        ma.active = false;
        pm.active = false;
        pm.PauseMovement();

    }

    public void CloseHeartMenu()
    {
        heartMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        ma.active = true;
        pm.active = true;
        pm.ResumeMovement();
    }

    public void FillSpeedUpgradeSlot()
    {
        speedUpgrades[ps.FireSpeedUpgradeTimes - 2].GetComponent<Image>().color = Color.blue;
    }

    public void FillSpreadUpgradeSlot()
    {
        spreadUpgrades[ps.SpreadUpgradeTimes - 2].GetComponent<Image>().color = Color.blue;
    }

    public void FillDamageUpgradeSlot()
    {
        damageUpgrades[ps.DamageUpgradeTimes - 2].GetComponent<Image>().color = Color.blue;
    }

    public void FillAmountUpgradeSlot()
    {
        amountUpgrades[ps.ProjectileUpgradeTimes - 2].GetComponent<Image>().color = Color.blue;
    }

    public void UpdateResources(int amount)
    {
        resources.text = amount.ToString();
    }

    public void UpdateHealth(int amount, int index)
    {
        health[index].text = amount.ToString();
    }
}
