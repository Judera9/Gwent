using UnityEngine;
using System.Collections;

public enum DeckClass{ ff7, genshin, Self}

public class DeckAsset : ScriptableObject 
{
	public DeckClass deck;
	public string ClassName;
    public string playerName;
    public string DeckAbility;
    public Sprite DeckIcon;
    public Sprite DeckFrame;
    public Color32 DeckTint;
    public Sprite playerIcon;
    public Sprite playerFrame;
    public Color32 playerTint;
    public int cardInhand;
    public int lives;  // each one have two lives
    public int currentValue;
    public bool isLeading;
}
