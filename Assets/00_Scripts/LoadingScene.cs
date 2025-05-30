using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static LoadingScene Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;        
        }        
    }


    GameObject loadingOBJ;
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] TextMeshProUGUI versionText;
    [SerializeField] GameObject TapOBJ;

    private AsyncOperation asyncOperation;

    private void Start()
    {
        asyncOperation = null;
        loadingOBJ = loadingSlider.transform.parent.gameObject;
        versionText.text = "Version: " + Application.version;
        
        TapOBJ.SetActive(false);        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && asyncOperation != null && asyncOperation.progress >= 0.9f)
        {

            BaseManager.isGameStart = true;
            
            asyncOperation.allowSceneActivation = true;            
        }
    }


    IEnumerator LoadDataCoroutine()
    {
        asyncOperation = SceneManager.LoadSceneAsync("Main");
        asyncOperation.allowSceneActivation = false;
        loadingOBJ.SetActive(true);

        while (asyncOperation.progress < 0.9f)
        {
            float progress = asyncOperation.progress;
            LoadingUpdate(progress);
            yield return null;
        }

        LoadingUpdate(1f);
        TapOBJ.SetActive(true);
    }

    public void LaodingMain()
    {
        Debug.Log("LaodingStart");
        StartCoroutine(LoadDataCoroutine());
    }

    private void LoadingUpdate(float progress)
    {
        loadingSlider.value = progress;
        loadingText.text = string.Format("데이터를 가져오고 있습니다....{0}%", progress * 100.0f);
    }

}
