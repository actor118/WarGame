﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace WorldMapStrategyKit {

    public static class SRP {

        static bool isURP {
            get {
#if UNITY_2019_1_OR_NEWER
                if (GraphicsSettings.currentRenderPipeline == null) return false;
                string pipe = GraphicsSettings.currentRenderPipeline.GetType().Name;
                return pipe.Contains("LWRP") || pipe.Contains("Lightweight") || pipe.Contains("Universal") || pipe.Contains("URP");
#else
                return false;
#endif
            }
        }

        public static void Configure (Material mat, int renderQueue) {

            if (mat == null || mat.shader == null) return;

            string n = mat.shader.name;
            int mat_URP_PrefixIndex = n.IndexOf("LWRP");
            bool urp = isURP;
            if (mat_URP_PrefixIndex < 0 && urp) {
                int i = n.LastIndexOf('/');
                if (i >= 0) {
                    n = n.Substring(i + 1);
                }
                string sn = "Shader Graphs/LWRP " + n;
                Shader comp = Shader.Find(sn);
                if (comp != null) {
                    mat.shader = comp;
                }
            }
            else if (mat_URP_PrefixIndex >= 0 && !urp) {
                string sn = "WMSK/" + n.Substring(mat_URP_PrefixIndex + 5);
                Shader comp = Shader.Find(sn);
                if (comp != null) {
                    mat.shader = comp;
                }
            }

            mat.renderQueue = renderQueue;
        }

        public static bool ConfigureTerrainShader (Material mat) {

            if (mat == null || mat.shader == null) return false;

            string n = mat.shader.name;
            int mat_URP_PrefixIndex = n.IndexOf("URP");
            bool urp = isURP;
            if (mat_URP_PrefixIndex < 0 && urp) {
                int i = n.LastIndexOf('/');
                if (i >= 0) {
                    n = n.Substring(i + 1);
                }
                string sn = "WMSK/URP/" + n;
                Shader comp = Shader.Find(sn);
                if (comp != null) {
                    mat.shader = comp;
                }
                else {
                    string msg = "URP compatible terrain shader of World Map Strategy Kit not found. Please import the URP terrain shaders package from WMSK/Resources/WMSK/Shaders/LWRP/TerrainShaders folder.";
                    Debug.LogError(msg);
#if UNITY_EDITOR
                    UnityEditor.EditorUtility.DisplayDialog("World Map Strategy Kit", msg, "Ok");
#endif
                    return false;
                }
            }
            else if (mat_URP_PrefixIndex >= 0 && !urp) {
                string sn = "WMSK" + n.Substring(mat_URP_PrefixIndex + 3);
                Shader comp = Shader.Find(sn);
                if (comp != null) {
                    mat.shader = comp;
                }
            }
            return true;
        }

        public static void DisableMSAA (Camera camera) {
            var pipeline = GraphicsSettings.currentRenderPipeline;
            if (pipeline != null) {
                var msaaProperty = pipeline.GetType().GetProperty("msaaSampleCount", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (msaaProperty != null) {
                    msaaProperty.SetValue(pipeline, 1);
                }
            }
            else {
                camera.allowMSAA = false;
            }
        }


    }
}