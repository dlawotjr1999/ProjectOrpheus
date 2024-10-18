using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public const int CANVAS_WIDTH = 1920;               // ĵ���� �ʺ�
    public const int CANVAS_HEIGHT = 1080;              // ĵ���� ����

    // UI ���콺 ���� �� �����ּ���
    public float WidthRatio { get { return CANVAS_WIDTH / (float)ScreenWidth; } }       // ȭ��-ĵ���� �ʺ� ����
    public float HeightRatio { get { return CANVAS_HEIGHT / (float)ScreenHeight; } }    // ȭ��-ĵ���� ���� ����

    private int ScreenWidth { get { return Screen.width; } }        // ��ũ�� �ʺ�
    private int ScreenHeight { get { return Screen.height; } }      // ��ũ�� ����

    private void SetResolution()
    {
        Screen.SetResolution(CANVAS_WIDTH, CANVAS_HEIGHT, true);
    }


    private void Awake()
    {
        SetResolution();
    }
}
