﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimArch.Visualization.UI
{
    public class ToolManager : Singleton<ToolManager>
    {
        //CameraMovement, DiagramMovement, Record 
        public string SelectedTool { set; get; }
        public bool ZoomingIn { set; get; } = false;
        public bool ZoomingOut { set; get; } = false;
        public bool IsJump { set; get; } = false;
        public int InterGraphJump { set; get; } = 0;
        public Color SelectedColor { set; get; }
        public bool isAnimating = false;
        public bool overUi = false;
        [SerializeField] private string startingSelectedColor;

        public void SelectTool(string toolName)
        {
            SelectedTool = toolName;
            if (SelectedTool.Equals("Highlighter"))
            {
                MenuManager.Instance.ActivatePanelColors(true);
            }
            else
            {
                MenuManager.Instance.ActivatePanelColors(false);
            }
        }

        public void ZoomIn(bool enabled)
        {
            ZoomingIn = enabled;
        }

        public void Jump()
        {
            IsJump = true;
            InterGraphJump = 1 - InterGraphJump;
        }

        public void Start()
        {
            SelectColor(startingSelectedColor);
        }

        public void ZoomOut(bool enabled)
        {
            ZoomingOut = enabled;
        }

        public void SelectColor(string colorID)
        {
            Color c;
            if (colorID.Contains("#"))
            {
                ColorUtility.TryParseHtmlString(colorID, out c);
            }
            else
            {
                ColorUtility.TryParseHtmlString("#" + colorID + "ff", out c);
            }

            SelectedColor = c;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            SelectedTool = "CameraMovement";
        }
    }
}
