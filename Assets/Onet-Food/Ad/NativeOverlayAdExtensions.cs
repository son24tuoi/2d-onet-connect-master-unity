using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public static class NativeOverlayAdExtensions
{
    public static float DpPerPixel => 160f / GetDPI();

    public static float GetDPI()
    {
        float dpi = Screen.dpi;

        // Neu dpi khong xac dinh (1 so may tra ve 0), gan gia tri mac dinh
        if (dpi == 0f)
        {
            dpi = 160f; // gia tri mac dinh chuan mdpi
        }

        return dpi;
    }

    public static void RenderTemplate(this NativeOverlayAd nativeOverlayAd, NativeTemplateStyle nativeTemplateStyle, RectTransform placementTarget, Canvas canvas)
    {
        if (placementTarget == null || canvas == null)
            return;

        Vector2 adSize = new Vector2(placementTarget.rect.width, placementTarget.rect.height);
        // Debug.Log(adSize);
        Vector2 adPos = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ?
            placementTarget.position :
            canvas.worldCamera.WorldToScreenPoint(placementTarget.position);
        adPos.y = Screen.height - adPos.y;  // fix gia tri truc y do doi truc
        adPos -= adSize / 2;    // fix vi tri theo kich thuoc
        adPos *= DpPerPixel;
        // Debug.Log(adPos);

        nativeOverlayAd.RenderTemplate(nativeTemplateStyle, new AdSize((int)adSize.x, (int)adSize.y), (int)adPos.x, (int)adPos.y);
    }

    public static AdSize AdSize(RectTransform rt)
    {
        return new AdSize((int)rt.rect.width, (int)rt.rect.height);
    }

    public static Vector2Int AdPosition(RectTransform rt, Canvas canvas)
    {
        Vector2 adPos = (canvas.renderMode == RenderMode.ScreenSpaceOverlay) ?
            rt.position :
            canvas.worldCamera.WorldToScreenPoint(rt.position);

        adPos.y = Screen.height - adPos.y;  // fix gia tri truc y do doi truc
        adPos -= new Vector2(rt.rect.width, rt.rect.height) / 2;    // fix vi tri theo kich thuoc
        adPos *= DpPerPixel;

        return new Vector2Int((int)adPos.x, (int)adPos.y);
    }
}