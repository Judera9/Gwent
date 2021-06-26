# Unity学习笔记

## 6.21

### 精灵图问题

1. 图片不清晰：filter mode调为point，max size调为合适（根据图片大小），compression模式调为None；
2. scene和game清晰度不一样，game tab下面将free aspect的low resolution...去掉，然后将scale设为1X；

### 总结

1. 以后精灵图还是采用宫格的形式，不要一长条，这样max size要很大才行（占用空间大）



## 6.24

### 卡牌翻转效果

https://www.bilibili.com/video/BV1py4y187TS?p=9&spm_id_from=pageDriver

#### 实现方式（关键在于0.01的Z轴厚度）

利用了3D的Collider，如果Ray能够检测到Collider则返回true，说明当前是反面：

 	1. 如果showBack这时候是true，说明没问题，不用做变换
 	2. 如果showBack这时候是false，说明在上一帧这个卡还是正面，那么现在应该翻面了（变成反面）

![image-20210625004730299](C:\Users\27096\AppData\Roaming\Typora\typora-user-images\image-20210625004730299.png)



#### 脚本解读

```C#
using UnityEngine;
using System.Collections;

/// <summary>
/// This script should be attached to the card game object to display card`s rotation correctly.
/// </summary>

/* 意思是说，Unity默认只有在Play Mode下，才会运行游戏当前运行场景里的GameObject挂载的脚本
所以这里加上[ExecuteInEditMode]或[ExecuteAlways]才能使脚本在Play Mode以外的状态下被执行 */
[ExecuteInEditMode]

/* Unity代码必须继承MonoBehavior */
public class BetterCardRotation : MonoBehaviour {

    // parent game object for all the card face graphics
    public RectTransform CardFront;

    // parent game object for all the card back graphics
    public RectTransform CardBack;

    // an empty game object that is placed a bit above the face of the card, in the center of the card
    public Transform targetFacePoint;

    // 3d collider attached to the card (2d colliders like BoxCollider2D won`t work in this case)
    public Collider col;

    // if this is true, our players currently see the card Back
    private bool showingBack = false;

    /* https://blog.csdn.net/alexhu2010q/article/details/105437083
    Awake和Start：加载时调用，也就是脚本赋给物体的时候被调用
    Update : 只有当场景中的某个物体发生变化时，才调用，当进程切出去再回来，也会调用一次。
    OnGUI : 当GameView接收到一个Event时才调用。
    OnRenderObject 和其他的渲染回调函数 : SceneView或者GameView重绘时，比如，一直移动鼠标的时候OnRenderObject会被调用。 
    */
    
	// Update is called once per frame
    /* 脚本如果是激活的，这三个函数会被上层逻辑每帧调用，FixedUpdate调用的次数和fixedTime有关，后面详细介绍，Update和LateUpdate每帧调用一次。 */
	void Update () 
    {
        // Raycast from Camera to a target point on the face of the card
        // If it passes through the card`s collider, we should show the back of the card
        
        /* https://blog.csdn.net/qq_30454411/article/details/79140318
        RaycastHit类用于存储发射射线后产生的碰撞信息。常用的成员变量如下：
        collider与射线发生碰撞的碰撞器
        distance 从射线起点到射线与碰撞器的交点的距离
        normal 射线射入平面的法向量
        point 射线与碰撞器交点的坐标（Vector3对象）
        */
        RaycastHit[] hits;
        hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
                                  direction: (-Camera.main.transform.position + targetFacePoint.position).normalized, 
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude) ;
        bool passedThroughColliderOnCard = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        }
        //Debug.Log("TotalHits: " + hits.Length); 
        
        /* 
        如果passedThroughColliderOnCard是false，也就是没有找到挂载GameObject的碰撞检查器
        如果passedThroughColliderOnCard是true，也就是找到了Collider
        */
        if (passedThroughColliderOnCard != showingBack)
        {
            // something changed
            showingBack = passedThroughColliderOnCard;
            if (showingBack)
            {
                // show the back side
                CardFront.gameObject.SetActive(false);
                CardBack.gameObject.SetActive(true);
            }
            else
            {
                // show the front side
                CardFront.gameObject.SetActive(true);
                CardBack.gameObject.SetActive(false);
            }

        }

	}
}
```

### 制作Prefabs

这个没啥好说的，利用unpack可以取消prefabs的关联，这样可以根据HeroCard创建别的Card



## 6.25

### ScriptableObject

**MonoBehavior和ScriptableObject两个类**

前者是对特定对象的脚本，可以实现某些功能，比如鼠标的交互

后者是可脚本对象，一般用于在游戏中存储某些信息

> https://zhuanlan.zhihu.com/p/39183933
>
> - ScriptableObject是将数据存储在.asset文件中的，可以理解为类似material这样的文件，ScriptableObject中存储的数值在runtime中作出修改，在退出后也会被保存下来。
> - 同样，类似material文件，在多个地方需要用到同一个数据，只要把ScriptableObject生成的.asset文件拖入相应位置就可以，不必再指向某个单例或者实例。
> - 可以被任何场景引用，在项目之间、场景之间很容易的共享数据。
> - 不必为了保存数据在场景中放置一个空GameObject。让项目的复杂度和耦合度最大限度的降低，强迫症福音。
> - ScriptableObject是自定义的数据类型，应用非常灵活。
> - 通过UGUI Button中的On Click事件只需要指向某一个ScriptableObject文件，而响应事件的GameObject也只需要获取到这个ScriptableObject文件即可触发，不必让按钮指向GameObject实例。大幅度减少了项目逻辑和结构的复杂程度。

```c#
using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility2 {
	
	/// <summary>
	/// Create new asset from <see cref="ScriptableObject"/> type with unique name at
	/// selected folder in project window. Asset creation can be cancelled by pressing
	/// escape key when asset is initially being named.
	/// </summary>
	/// <typeparam name="T">Type of scriptable object.</typeparam>
	public static void CreateAsset<T>() where T : ScriptableObject {
		var asset = ScriptableObject.CreateInstance<T>();
		ProjectWindowUtil.CreateAsset(asset, "New " + typeof(T).Name + ".asset");
	}
	
}
```



CardUnityIntegration.cs: 创建CardAsset对象

```c
using UnityEngine;
using UnityEditor;

static class CardUnityIntegration 
{

	[MenuItem("Assets/Create/CardAsset")]  // 用于实现菜单命令
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<CardAsset>();
	}

}
```



CardAsset.cs定制ScriptableObject对象

```C#
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters
}

public class CardAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public CharacterAsset characterAsset;  // if this is null, it`s a neutral card
    [TextArea(2,3)]  // 用于显示一个文字框
    public string Description;  // Description for spell or character
	public Sprite CardImage;
    public int ManaCost;

    [Header("Creature Info")]
    public int MaxHealth;
    public int Attack;
    public int AttacksForOneTurn = 1;
    public bool Taunt;
    public bool Charge;
    public string CreatureScriptName;
    public int specialCreatureAmount;

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public TargetingOptions Targets;

}
```



DeckAsset.cs定制卡组加玩家信息

```C#
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
```



### 设计卡牌属性

注意以下几点设计：

1. HeroCard点数相对较高，但是不会有Special
2. 设定了两个ScriptableObject。一个是DeckAsset，用于存储玩家信息和卡组信息（相当于例子中的Character），另一个是CardAsset，存储了卡牌信息



### 映射Asset到实体

```C#
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
public class OneCardManager : MonoBehaviour {

    public CardAsset cardAsset;  // 获取Asset的信息，映射到实体
    public OneCardManager PreviewManager;
    
    [Header("Text Component References")]
    public Text NameText;  // 卡牌名字，我定义为Character名字
    public Text DescriptionText;  // 文字描述
    public Text ValueText;  // Value点数
    
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
        if (cardAsset.characterAsset != null)
        {
            CardBodyImage.color = cardAsset.characterAsset.ClassCardTint;
            CardFaceFrameImage.color = cardAsset.characterAsset.ClassCardTint;
            CardTopRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
            CardLowRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
        }
        else
        {
            //CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            CardFaceFrameImage.color = Color.white;
            //CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            //CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
        }
        // 2) add card name
        NameText.text = cardAsset.name;
        // 3) add mana cost
        ManaCostText.text = cardAsset.ManaCost.ToString();
        // 4) add description
        DescriptionText.text = cardAsset.Description;
        // 5) Change the card graphic sprite
        CardGraphicImage.sprite = cardAsset.CardImage;

        if (cardAsset.MaxHealth != 0)
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
```

