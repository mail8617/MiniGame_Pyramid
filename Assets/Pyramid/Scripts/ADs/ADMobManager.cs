using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class ADMobManager : MonoBehaviour
{
    public static ADMobManager instance;

    Admob ad;
    string appID= "";
    string bannerID = "";
    string interstitialID = "";
    string videoID = "";
    string nativeBannerID = "";
    
    
    private void Awake()
    {
        instance = this;

        initAdmob();
    }

    void initAdmob()
    {
#if UNITY_IOS
        		// appID="ca-app-pub-3940256099942544~1458002511";
				 bannerID="ca-app-pub-3940256099942544/2934735716";
				 interstitialID="ca-app-pub-3940256099942544/4411468910";
				 videoID="ca-app-pub-3940256099942544/1712485313";
				 nativeBannerID = "ca-app-pub-3940256099942544/3986624511";
#elif UNITY_ANDROID
        appID= "ca-app-pub-3535705278798960~1140641020";
        bannerID = "ca-app-pub-3535705278798960/8527450108";
        //bannerID = "ca-app-pub-3940256099942544/6300978111";
        

#endif
        AdProperties adProperties = new AdProperties();
        adProperties.isTesting(false);
        adProperties.isAppMuted(true);
        adProperties.isUnderAgeOfConsent(false);
        adProperties.appVolume(100);
        adProperties.maxAdContentRating(AdProperties.maxAdContentRating_G);
        string[] keywords = { "diagram", "league", "brambling" };
        adProperties.keyworks(keywords);

        ad = Admob.Instance();
        //ad.bannerEventHandler += onBannerEvent;
        //ad.interstitialEventHandler += onInterstitialEvent;
        //ad.rewardedVideoEventHandler += onRewardedVideoEvent;
        //ad.nativeBannerEventHandler += onNativeBannerEvent;
        ad.initSDK(adProperties);//reqired,adProperties can been null
    }

    public void ShowBanner()
    {
        Debug.Log("ShowBanner");
        Admob.Instance().showBannerRelative(bannerID, AdSize.SMART_BANNER, AdPosition.BOTTOM_CENTER);
    }

    public void HideBanner()
    {
        Admob.Instance().removeBanner();
        //Admob.Instance().removeBanner("mybanner");
    }

    //void onInterstitialEvent(string eventName, string msg)
    //{
    //    Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
    //    if (eventName == AdmobEvent.onAdLoaded)
    //    {
    //        Admob.Instance().showInterstitial();
    //    }
    //}
    //void onBannerEvent(string eventName, string msg)
    //{
    //    Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
    //}
    //void onRewardedVideoEvent(string eventName, string msg)
    //{
    //    Debug.Log("handler onRewardedVideoEvent---" + eventName + "  rewarded: " + msg);
    //}
    //void onNativeBannerEvent(string eventName, string msg)
    //{
    //    Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
    //}
}
