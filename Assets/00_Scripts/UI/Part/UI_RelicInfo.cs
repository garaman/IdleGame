using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RelicInfo : MonoBehaviour
{
    [Header("Relic")]
    [SerializeField] public Image relicRarity;
    [SerializeField] public Image relicIcon;
    [SerializeField] public TextMeshProUGUI rarityText;
    [SerializeField] public TextMeshProUGUI relicNameText;
    [SerializeField] public TextMeshProUGUI relicDesText;
        
    [Space(10.0f)]
    [Header("Slider")]
    [SerializeField] public Image sliderFill;
    [SerializeField] public TextMeshProUGUI relicLevelText;
    [SerializeField] public TextMeshProUGUI relicCountText;

    [Space(10.0f)]
    [Header("Skill")]
    [SerializeField] public TextMeshProUGUI collectBufferText;
    [SerializeField] public Image skillIcon;
    [SerializeField] public TextMeshProUGUI skillNameText;
    [SerializeField] public TextMeshProUGUI skilldesText;

    [Space(10.0f)]
    [Header("Button")]
    [SerializeField] public Button summon;
    [SerializeField] public Button upgrade;
}
