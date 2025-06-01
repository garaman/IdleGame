using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HeroInfo : MonoBehaviour
{
    [Header("Hero")]
    [SerializeField] public Image heroRarity;
    [SerializeField] public Image heroIcon;
    [SerializeField] public TextMeshProUGUI rarityText;
    [SerializeField] public TextMeshProUGUI heroNameText;
    [SerializeField] public TextMeshProUGUI heroDesText;

    [Space(10.0f)]
    [Header("State")]
    [SerializeField] public TextMeshProUGUI fightScoreText;
    [SerializeField] public TextMeshProUGUI ATKScoreText;
    [SerializeField] public TextMeshProUGUI HpScoreText;

    [Space(10.0f)]
    [Header("Slider")]
    [SerializeField] public Image sliderFill;
    [SerializeField] public TextMeshProUGUI heroLevelText;
    [SerializeField] public TextMeshProUGUI heroCountText;

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
