using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class ADSManager 
{
    private bool TestMode = true;
    //public readonly string Android_Banner_id = "ca-app-pub-3482755551904464/9398841234";
    //public readonly string Android_Interstitial_id = "ca-app-pub-3482755551904464/4625619605";    

    //public readonly string Android_Banner_Test_id = "ca-app-pub-3940256099942544/9214589741";
    //public readonly string Android_Interstitial_Test_id = "ca-app-pub-3940256099942544/1033173712";

    public readonly string Android_Rewarded_id = "ca-app-pub-3482755551904464/5627935421";
    public readonly string Android_Rewarded_Test_id = "ca-app-pub-3940256099942544/5224354917";

    //BannerView bannerView;
    //InterstitialAd interstitial;
    RewardedAd rewardedAd;
    AdRequest adRequest;

    Action rewardedAdCallBack;

    public void Init()
    {
        MobileAds.Initialize(initStatus => { });

        TestMode = true; // 테스트모드로 광고 확인하기.
        RepareADS();
    }

    private void RepareADS()
    {
        //string bannerId;
        //string interstitialId;
        string rewardedId;

        if (TestMode)
        {
            //bannerId = Android_Banner_Test_id;
            //interstitialId = Android_Interstitial_Test_id;
            rewardedId = Android_Rewarded_Test_id;
        }
        else
        {
            //bannerId = Android_Banner_id;
            //interstitialId = Android_Interstitial_id;
            rewardedId = Android_Rewarded_id;
        }

        adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        //BannerView(bannerId);
        //InterstitialAd.Load(interstitialId, adRequest, OnAdLoadCallBack);

        RewardedAd.Load(rewardedId, adRequest, OnRewardedAdLoadCallBack);
    }


    private void OnRewardedAdLoadCallBack(RewardedAd ad, LoadAdError error)
    {
        if (error != null && ad == null)
        {
            Debug.LogError("보상형 광고 로드 실패! : " + error.GetMessage());
            return;
        }
        Debug.Log("보상형 광고 로드 성공! : " + ad.GetResponseInfo());
        rewardedAd = ad;
        RegisterReloadHandler(rewardedAd);
    }

    private void RegisterReloadHandler(RewardedAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("보상형 광고 닫힘!");
            RepareADS();
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("보상형 광고 실패! : " + error.GetMessage());
            RepareADS();
        };

        ad.OnAdPaid += (AdValue adValue) =>
        {
            if(rewardedAdCallBack != null)
            {
                rewardedAdCallBack.Invoke();
                rewardedAdCallBack = null;
            }            
        };
    }

    public void ShowRewardedAd(Action rewardcallBack)
    {
        rewardedAdCallBack = rewardcallBack;
        
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {            
            rewardedAd.Show((Reward reward) => 
            {
                Debug.Log("보상형 광고 보상: " + reward.Amount + " / " + reward.Type);

                // 삭제해도됨. 보상형 광고는 OnAdPaid에서 처리함.
                // 테스트시 OnAdPaid가 호출되지 않음.
                if (rewardedAdCallBack != null)
                {
                    rewardedAdCallBack.Invoke();
                    rewardedAdCallBack = null;
                }
            });
        }
        else
        {
            Debug.Log("보상형 광고 없음!");
            rewardedAdCallBack = null;
            RepareADS();
        }
    }

    /* 전면 광고
    public void ShowInterstitialAd()
    {
        if (interstitial != null && interstitial.CanShowAd())
        {
            interstitial.Show();           
        }
        
    }

    private void OnAdLoadCallBack(InterstitialAd ad, LoadAdError error)
    {
        if(error != null && ad == null)
        {
            Debug.LogError("InterstitialAd Load Failed: " + error.GetMessage());
            return;
        }

        Debug.Log("전면 광고 로드! : " + ad.GetResponseInfo());

        interstitial = ad;
        RegisterReloadHandler(interstitial);
    }

    private void RegisterReloadHandler(InterstitialAd ad)
    {
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("전면 광고 닫힘!");
            RepareADS();            
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("전면 광고 실패! : " + error.GetMessage());
            RepareADS();
        };
    }
    */
    /* 배너 광고    
    public void BannerView(string bannerId)
    {
        if(bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(bannerId, adaptiveSize, AdPosition.Bottom);
                
        bannerView.LoadAd(adRequest);
    }
    */
}
