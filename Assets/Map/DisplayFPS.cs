using System;
using UnityEngine;

    /// <summary>
    /// 显示帧率
    /// </summary>
    public class DisplayFPS : MonoBehaviour
    {
        private bool showFPS = false;
        private float deltaTime = 0.0f;
        private GUIStyle style = new GUIStyle();

        private void Start()
        {
           

            // 设置显示帧率的GUI样式
            style.normal.textColor = Color.white;
            style.fontSize = 24;
        }

        /// <summary>
        /// 显示帧率
        /// </summary>
        /// <param name="arg"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ChangeStateEvent(bool arg)
        {
            showFPS=arg;
        }

        private void Update()
        {
        // 检查是否按下了F键
        if (Input.GetKeyDown(KeyCode.F))
        {
            showFPS = !showFPS; // 切换显示帧率的状态
        }

        // 计算帧率
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }

        private void OnGUI()
        {
            // 如果需要显示帧率
            if (showFPS)
            {
                // 显示帧率
                float fps = 1.0f / deltaTime;
                string fpsText = string.Format("{0:0.} FPS", fps);
                GUI.Label(new Rect(10, 10, 100, 30), fpsText, style);
            }
        }
    }

