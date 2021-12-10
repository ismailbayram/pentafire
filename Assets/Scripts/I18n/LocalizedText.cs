using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour {
    public string key;
    private Text text;

    void Start() {
        this.text = this.GetComponent<Text>();
        this.text.text = LocaleHelper.GetText(this.key);
    }
}
