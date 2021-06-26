using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;
    public OneCardManager PreviewManager;

    [Header("Text Component References")]
    public Text NameText;
    public Text DescriptionText;
    public Text ValueText;

    [Header("Image References")]

    public Image FaceFrame;
    public Image Character;
    public Image IntroFrame;
    public Image NameFrame;

    public Image BattleCircle;
    public Image BattleType;

    public Image SpecialCircle;
    public Image SpecialType;

    public Image ValueCircle;

    public Image BackFrame;
    public Image BackGround;

    void Awake()
    {
        if (cardAsset != null)
            ReadCardFromAsset();
    }

    private bool canBePlayedNow = false;
    public bool CanBePlayedNow
    {
        get
        {
            return canBePlayedNow;
        }

        set
        {
            canBePlayedNow = value;

            CardFaceGlowImage.enabled = value;
        }
    }

    public void ReadCardFromAsset()
    {
        // universal actions for any Card
        // 1) apply tint
        if (cardAsset.deckAsset != null)
        {
            FaceFrame.color = cardAsset.deckAsset.DeckTint;
            IntroFrame.color = cardAsset.deckAsset.DeckTint;
            NameFrame.color = cardAsset.deckAsset.DeckTint;
            BattleCircle.color = cardAsset.deckAsset.DeckTint;
            SpecialCircle.color = cardAsset.deckAsset.DeckTint;
            ValueCircle.color = cardAsset.deckAsset.DeckTint;
            BackFrame.color = cardAsset.deckAsset.DeckTint;
            BackGround.color = cardAsset.deckAsset.DeckTint;
        }
        else
        {
            //CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            CardFaceFrameImage.color = Color.black;
            //CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            //CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
        }
        NameText.text = cardAsset.name;
        DescriptionText.text = cardAsset.Description;
        CardGraphicImage.sprite = cardAsset.CardImage;

        if (cardAsset.cardType == cardAsset.)
        {
            // this is a creature
            AttackText.text = cardAsset.Attack.ToString();
            HealthText.text = cardAsset.MaxHealth.ToString();
        }

        if (PreviewManager != null)
        {
            // this is a card and not a preview
            // Preview GameObject will have OneCardManager as well, but PreviewManager should be null there
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }
}
