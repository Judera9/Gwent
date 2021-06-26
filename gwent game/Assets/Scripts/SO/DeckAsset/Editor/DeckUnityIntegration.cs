using UnityEngine;
using UnityEditor;

static class DeckUnityIntegration {

	[MenuItem("Assets/Create/DeckAsset")]
	public static void CreateYourScriptableObject() {
		ScriptableObjectUtility2.CreateAsset<DeckAsset>();
	}

}
