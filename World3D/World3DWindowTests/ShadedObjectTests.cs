﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World3D;

namespace World3DWindowTests
{
    public class ShadedObjectTests
    {
        public static void ThreeShaders()
        {
            Cube cube1 = new Cube();
            Cube cube2 = new ColouredCube();
            Cube cube3 = new TexturedCube();
            ShaderModelRenderer sm1 = new ShaderModelRenderer(cube1);
            ShaderModelRenderer sm2 = new ShaderModelRenderer(cube2)
            {
                VertexShaderFilaneme = "Shaders/vs_col.glsl"
            };
            ShaderModelRenderer sm3 = new ShaderModelRenderer(cube3)
            {
                VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                FragmentShaderFilename = "Shaders/fs_tex.glsl"
            };

            using (WorldWindow game = new WorldWindow() { Title = "ThreeShaders" })
            {
                game.Camera.LookAt(new Vector3(-4, 4, -4), new Vector3());

                (cube3 as TexturedCube).TextureID = TextureLoader.LoadImage("Textures/opentksquare.png");

                game.Models.Add(sm1);
                game.Models.Add(sm2);
                game.Models.Add(sm3);

                UserInterface ui = new UserInterface(game);

                float time = 0;
                game.UpdateFrame += (o, e) =>
                            {
                                time += (float)e.Time;
                                cube1.Position = new Vector3(2, 0.05f * (float)Math.Sin(time), 0);
                                cube1.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                                cube2.Position = new Vector3(0, 0.05f * (float)Math.Sin(time), 2);
                                cube2.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                                cube3.Position = new Vector3(0, 0.05f * (float)Math.Sin(time), 0);
                                cube3.Rotation = new Vector3(0.3f * time, 0.1f * time, 0);

                                ui.PrintCommonStats(e.Time);
                            };
                game.Run();
            }
        }

        public static void ManyCubes()
        {
            using (WorldWindow game = new WorldWindow())
            {
                Vector3 eye = new Vector3(-4, 4, -4);
                game.Camera.LookAt(eye, eye*0.8f);
                int texID = TextureLoader.LoadImage("Textures/opentksquare.png");
                Vector3[] posOffsets = GenerateRandomCubes(game, texID);

                UserInterface ui = new UserInterface(game);
                

                float time = 0;
                game.UpdateFrame += (o, e) =>
                {
                    time += (float)e.Time;
                    ui.PrintCommonStats(e.Time);


                    MoveModels(game, time, posOffsets);
                };
                game.Run(60.0);
            }
        }
        

        static Vector3[] GenerateRandomCubes(WorldWindow game, int texID)
        {
            int numCubes = 100;
            Random rand = new Random();
                
            Vector3[] posOffsets = new Vector3[numCubes];
            for(int i = 0; i < numCubes; i++)
            {
                int type = rand.Next(3);
                Cube cube;
                ShaderModelRenderer sm;
                if (type == 0)
                {
                    cube = new Cube();
                    sm = new ShaderModelRenderer(cube);
                }
                else if(type == 1)
                {
                    cube = new ColouredCube();
                    sm = new ShaderModelRenderer(cube)
                    {
                        VertexShaderFilaneme = "Shaders/vs_col.glsl",
                        FragmentShaderFilename = "Shaders/fs.glsl"
                    };
                }
                else
                {
                    cube = new TexturedCube();
                    sm = new ShaderModelRenderer(cube)
                    {
                        VertexShaderFilaneme = "Shaders/vs_tex.glsl",
                        FragmentShaderFilename = "Shaders/fs_tex.glsl"
                    };
                        (cube as TexturedCube).TextureID = TextureLoader.LoadImage("Textures/opentksquare.png");
                    
                }

                cube.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                game.Models.Add(sm);
                Vector3 v = new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble())*(float)(Math.PI*2);
                posOffsets[i] = v - (Vector3.One * (float)Math.PI);
            }
            return posOffsets;
        }

        static void MoveModels(WorldWindow game, float time, Vector3[] posOffsets)
        {
            for (int i = 0; i < posOffsets.Length; i++)
            {
                MovableModel cube = game.Models[i].Model as MovableModel;
                cube.Position = new Vector3(posOffsets[i].X, (float)Math.Sin(time + posOffsets[i].Y), posOffsets[i].Z);
                cube.Rotation = new Vector3(0.5f * time, 0.1f * time, 0);
            }
        }
    }
}
