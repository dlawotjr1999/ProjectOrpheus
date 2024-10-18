using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public const int CANVAS_WIDTH = 1920;               // 캔버스 너비
    public const int CANVAS_HEIGHT = 1080;              // 캔버스 높이

    // UI 마우스 조작 시 곱해주세요
    public float WidthRatio { get { return CANVAS_WIDTH / (float)ScreenWidth; } }       // 화면-캔버스 너비 비율
    public float HeightRatio { get { return CANVAS_HEIGHT / (float)ScreenHeight; } }    // 화면-캔버스 높이 비율

    private int ScreenWidth { get { return Screen.width; } }        // 스크린 너비
    private int ScreenHeight { get { return Screen.height; } }      // 스크린 높이

    private void SetResolution()
    {
        Screen.SetResolution(CANVAS_WIDTH, CANVAS_HEIGHT, true);
    }


    private void Awake()
    {
        SetResolution();
    }
}
