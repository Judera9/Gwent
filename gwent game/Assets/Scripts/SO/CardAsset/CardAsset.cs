using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Specials  // Special abillities
{
    None,
    AddRow,
    Ally,
    DoubleRow,
    Kill,
    Relive,
    Scarecrow,
    Spy
};

public enum CardTypes
{
    NormalCard,  // Withour the SpecialFrame
    HeroCard,
    SpecialCard,
    FunctionCard,  // like scarecrow
    WeatherCard
};

public enum BattleFields
{
    Closed,
    Middle,
    Ranged,
    All,
    Warrior  // both Closed and Middle
};

public enum WeatherTypes
{
    Sunny,
    Rainny,
    Cloudy,
    Snowy
};

public class CardAsset : ScriptableObject
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public DeckAsset deckAsset; // might get enemy's card
    public string name;
    [TextArea(2, 3)]
    public string Description;  // Description for this card
    public CardTypes cardType;
    public Sprite CardImage;
    public Specials special;

    [Header("CharacterCard info")]  // contain HeroCard and NormalCard and SpecialCard
    public int Value;
    public int currentValue;
    public BattleFields battleFeilds;
    // public string CharacterScriptName;
    //public int AttacksForOneTurn = 1;
    //public bool Taunt;
    //public bool Charge;
    //public int specialCreatureAmount;

    [Header("Function info")]
    // public string FunctionScriptName;
    //public int specialSpellAmount;
    //public TargetingOptions Targets;

    [Header("Weather info")]
    public WeatherTypes weather;

}
