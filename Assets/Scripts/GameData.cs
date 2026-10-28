using System;
using UnityEngine;

[Serializable]
public class GameData
{
    [SerializeField] private float life;
    [SerializeField] private float maxLife;

    [SerializeField] private float mana;
    [SerializeField] private float maxMana;

    [SerializeField] private int maxJumps;

    [SerializeField] private float damage;

    [SerializeField] private int currency;
    
    //
    public float Life
    {
        get { return life; }
        set { life = value; }
    }
    public float MaxLife
    {
        get { return maxLife; }
        set { maxLife = value; }
    }
    public float Mana
    {
        get { return mana; }
        set { mana = value; }
    }
    public float MaxMana
    {
        get { return maxMana; }
        set { maxMana = value; }
    }
    public int MaxJumps
    {
        get { return maxJumps; }
        set { maxJumps = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
    public int Currency
    {
        get { return currency; }
        set { currency = value; }
    }
}
