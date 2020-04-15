using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooterUpgrades : ProjectileShooter
{
    [Header("Firespeed")]
    [SerializeField]
    protected float fireSpeedUpgrade;
    [SerializeField]
    private int fireSpeedUpgradeCost;
    [SerializeField]
    private int fireSpeedUpgradeCostIncrease;

    [Header("Spread")]
    [SerializeField]
    protected float spreadAmountUpgrade;
    [SerializeField]
    private int spreadUpgradeCost;
    [SerializeField]
    private int spreadUpgradeCostIncrease;

    [Header("Damage")]
    [SerializeField]
    protected int damageUpgrade;
    [SerializeField]
    private int damgeUpgradeCost;
    [SerializeField]
    private int damgeUpgradeCostIncrease;

    [Header("Amount")]
    [SerializeField]
    private int amountUpgradeCost;
    [SerializeField]
    private int amountUpgradeCostIncrease;

    [Header("Components")]
    [SerializeField]
    private Game_Manager gm;
    [SerializeField]
    private UI_Manager ui;

    private int fireSpeedUpgradeTimes = 1;
    private int spreadUpgradeTimes = 1;
    private int damageUpgradeTimes = 1;
    private int projectileUpgradeTimes = 1;

    public int FireSpeedUpgradeTimes { get { return fireSpeedUpgradeTimes; } }
    public int SpreadUpgradeTimes { get { return spreadUpgradeTimes; } }
    public int DamageUpgradeTimes { get { return damageUpgradeTimes; } }
    public int ProjectileUpgradeTimes { get { return projectileUpgradeTimes; } }

    public void UpgradeFireSpeed()
    {
        if (fireSpeedUpgradeTimes < 10 && gm.ConsumeResources(fireSpeedUpgradeCost))
        {
            fireSpeed -= fireSpeedUpgrade;
            fireSpeedUpgradeTimes++;
            fireSpeedUpgrade += fireSpeedUpgradeCostIncrease;
            ui.FillSpeedUpgradeSlot();
        }
    }

    public void UpgradeSpread()
    {
        if (spreadUpgradeTimes < 10 && gm.ConsumeResources(spreadUpgradeCost))
        {
            spreadAmount -= spreadAmountUpgrade;
            spreadUpgradeTimes++;
            spreadUpgradeCost += spreadUpgradeCostIncrease;
            ui.FillSpreadUpgradeSlot();
        }
    }

    public void UpgradeDamage()
    {
        if (damageUpgradeTimes < 10 && gm.ConsumeResources(damgeUpgradeCost))
        {
            damage += damageUpgrade;
            damageUpgradeTimes++;
            damgeUpgradeCost += damgeUpgradeCostIncrease;
            ui.FillDamageUpgradeSlot();
        }
    }

    public void UpgradeAmount()
    {
        if (projectileUpgradeTimes < 10 && gm.ConsumeResources(amountUpgradeCost))
        {
            projectileAmount++;
            projectileUpgradeTimes++;
            amountUpgradeCost += amountUpgradeCostIncrease;
            ui.FillAmountUpgradeSlot();
        }
    }
}
