﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World3D;

namespace World3DWindowTests
{
    public class UserInterface
    {
        public string Text { get { return ui.Text; } set { ui.Text = value; } }

        public UserInterfaceSimpleTextbox UI { get { return ui; } }

        WorldWindow game;
        UserInterfaceSimpleTextbox ui;
        double time;
        double fps = 30;
        double interval = 30;

        public UserInterface(WorldWindow game)
        {
            this.game = game;
            ui = AddUI(game);
        }


        public static UserInterfaceSimpleTextbox AddUI(WorldWindow game)
        {
            UserInterfaceSimpleTextbox ui = new UserInterfaceSimpleTextbox();
            ui.BoxBackground = Color.FromArgb(0, 0, 0, 0);
            ShaderModelRenderer sm = new ShaderModelRenderer(ui)
            {
                VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                FragmentShaderFilename = "Shaders/fs_tex.glsl",
            };
            game.Models.Add(sm);
            return ui;
        }

        public void PrintCommonStats(double timeSinceLastRender)
        {
            double p = 0.05;
            time += (float)timeSinceLastRender;
            interval = interval * (1-p) + 1000*timeSinceLastRender * p;
            double fp = timeSinceLastRender == 0 ? 1000 : 1.0 / timeSinceLastRender;
            fps = fps * (1-p) + p * fp;
            AzElCamera cam = game.Camera as AzElCamera;
            Text = "Fps: " + fps.ToString("N1") + " Time: " + interval.ToString("N2") + "\n"
                    + "CamLoc:  " + cam.Eye.X.ToString("F2") + "," + cam.Eye.Y.ToString("F2") + "," + cam.Eye.Z.ToString("F2") + "\n"
                    + "CamAzEl: " + cam.Azimuth.ToString("F2") + ":" + cam.Elevation.ToString("F2");
        }
    }
}
