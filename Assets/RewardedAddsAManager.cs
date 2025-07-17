using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class RewardedAdsManager : MonoBehaviour
{
    #if UNITY_ANDROID
    private string adUnitId = "ca-app-pub-1222005208106990/4976457442"; // test
    #elif UNITY_IPHONE
    private string adUnitId = "ca-app-pub-3940256099942544/1712485313"; // test
    #else
    private string adUnitId = "unused";
    #endif

    private RewardedAd rewardedAd;

    void Start()
    {
        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("SDK inicializado.");
            LoadRewardedAd();
        });

        ShowRewardedAd();
    }

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var request = new AdRequest();
        RewardedAd.Load(adUnitId, request, (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError($"Falha ao carregar RewardedAd: {error}");
                return;
            }
            Debug.Log($"RewardedAd carregado: {ad.GetResponseInfo()}");

            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Ad recompensado exibido.");
            };
            rewardedAd.OnAdFullScreenContentFailed += (err) =>
            {
                Debug.LogError($"Ad recompensado falhou ao abrir: {err}");
                LoadRewardedAd();
            };
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Ad recompensado fechado.");
                rewardedAd.Destroy();
                LoadRewardedAd();
            };
        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                Debug.Log($"Usuário recompensado! Tipo: {reward.Type}, Quantidade: {reward.Amount}");
                // TODO: conceda a recompensa no seu jogo aqui
            });
        }
        else
        {
            Debug.LogWarning("Ad recompensado não está pronto.");
        }
    }

    void OnDestroy()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }
    }
}
