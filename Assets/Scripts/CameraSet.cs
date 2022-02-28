using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    [HideInInspector] public static bool moreHeightLong = false;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // 기기 화면비(가로/세로) / 게임 화면비(가로/세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1) // 게임 화면비가 더 큰경우(기기 화면비가 게임 화면비보다 좀더 길쭉한 경우)
        {
            rect.height = scaleheight; // 카메라 높이를 낮춰 기존 게임 화면비에 맞춤
            rect.y = (1f - scaleheight) / 2f; // 낮춘 카메라 높이를 고려해 화면 중앙에 오도록 y값을 살짝 위로 조정
            moreHeightLong = true;
        }
        else // 기기 화면비가 더 큰경우(기기 화면비가 게임 화면비보다 좀더 납작한 경우)
        {
            rect.width = scalewidth; // 카메라 너비를 낮춰 기존 게임 화면비에 맞춤
            rect.x = (1f - scalewidth) / 2f; // 낮춘 카메라 너비를 고려하여 화면 중앙에 오도록 x값을 살짝 오른쪽으로 조정
        }
        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}