using UnityEngine;

namespace WorldMapStrategyKit {

    public class SDFTextureGenerator {

        static int[] vertices;
        static int pointsCount;
        static Color32[] distanceField;
        static ComputeShader jumpFloodComputeShader;
        static readonly int stepSizeId = Shader.PropertyToID("stepSize");


        public static RenderTexture Generate(Region region, SDFQuality quality) {
            Rect rect = region.rect2D;

            pointsCount = region.points.Length;

            if (vertices == null || vertices.Length < pointsCount * 2) {
                vertices = new int[pointsCount * 2];
            }

            float xMin = rect.xMin;
            float yMin = rect.yMin;
            float w = rect.width;
            float h = rect.height;

            int resolution;
            float size = Mathf.Max(w, h);

            switch(quality) {
                case SDFQuality.Low: resolution = (int)(1024 * size); break;
                case SDFQuality.High: resolution = (int)(4096 * size); break;
                case SDFQuality.VeryHigh: resolution = (int)(8192 * size); break;
                default:
                    resolution = (int)(2048 * size);
                    break;
            }

            // make resolution power of 2
            resolution = MakePowerOf2(resolution);
            resolution = Mathf.Clamp(resolution, 128, 8192);

            for (int k = 0; k < pointsCount; k++) {
                int x = (int)(resolution * (region.points[k].x - xMin) / w);
                if (x >= resolution) x--;
                vertices[k * 2] = x;
                int y = (int)(resolution * (region.points[k].y - yMin) / h);
                if (y >= resolution) y--;
                vertices[k * 2 + 1] = y;
            }

            RenderTexture sdfTexture = GenerateSDFTexture(resolution);
            return sdfTexture;
        }

        static RenderTexture GenerateSDFTexture(int resolution) {

            DrawRegionPolygon(resolution);

            Texture2D distanceFieldTexture = CreateDistanceFieldTexture(resolution);

            RenderTexture sdfTexture = JumpFlood(resolution, distanceFieldTexture);

            Object.DestroyImmediate(distanceFieldTexture);

            return sdfTexture;

        }

        static int MakePowerOf2(int n) {
            if (n <= 1)
                return 1;
            float log2 = Mathf.Log(n) / Mathf.Log(2);
            int lowerExp = Mathf.FloorToInt(log2);
            int upperExp = Mathf.CeilToInt(log2);
            int lowerPowerOf2 = (int)Mathf.Pow(2, lowerExp);
            int upperPowerOf2 = (int)Mathf.Pow(2, upperExp);
            return (n - lowerPowerOf2) < (upperPowerOf2 - n) ? lowerPowerOf2 : upperPowerOf2;
        }

        static void DrawRegionPolygon(int resolution) {
            int len = resolution * resolution;
            if (distanceField == null || distanceField.Length != len) {
                distanceField = new Color32[len];
            }
            for (int i = 0; i < len; i++) {
                distanceField[i].r = 255;
            }

            for (int i = 0; i < pointsCount - 1; i++) {
                DrawLineOnDistanceFieldArray(distanceField, i, i + 1, resolution);
            }
            DrawLineOnDistanceFieldArray(distanceField, pointsCount - 1, 0, resolution);
        }

        static void DrawLineOnDistanceFieldArray(Color32[] field, int startIndex, int endIndex, int resolution) {

            int x0 = vertices[startIndex * 2];
            int y0 = vertices[startIndex * 2 + 1];
            int x1 = vertices[endIndex * 2];
            int y1 = vertices[endIndex * 2 + 1];

            int dx = x1 - x0;
            if (dx < 0) dx = -dx;
            int dy = y1 - y0;
            if (dy < 0) dy = -dy;
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true) {
                field[x0 + y0 * resolution].r = 0;

                if (x0 == x1 && y0 == y1) break;
                int e2 = err * 2;
                if (e2 > -dy) {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        static Texture2D CreateDistanceFieldTexture(int resolution) {
            Texture2D distanceFieldTexture = new Texture2D(resolution, resolution, TextureFormat.R8, false);
            distanceFieldTexture.SetPixels32(distanceField);
            distanceFieldTexture.Apply();
            return distanceFieldTexture;
        }

        static RenderTexture JumpFlood(int resolution, Texture2D distanceFieldTexture) {

            // Prepare jump flood compute shader
            if (jumpFloodComputeShader == null) {
                jumpFloodComputeShader = Resources.Load<ComputeShader>("WMSK/Shaders/SDFJumpFlood");
                if (jumpFloodComputeShader == null) {
                    Debug.LogError("Compute Shader not found!");
                    return null;
                }
            }

            RenderTexture resultTexture = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.R8);
            resultTexture.enableRandomWrite = true;
            resultTexture.Create();

            // Copy the initial distance field to the result texture
            Graphics.Blit(distanceFieldTexture, resultTexture);

            int kernelHandle = jumpFloodComputeShader.FindKernel("CSMain");

            jumpFloodComputeShader.SetInt("resolution", resolution);
            jumpFloodComputeShader.SetTexture(kernelHandle, "ResultTexture", resultTexture);

            int levels = Mathf.CeilToInt(Mathf.Log(resolution, 2));
            for (int level = levels - 1; level >= 0; level--) {
                int stepSize = 1 << level;
                jumpFloodComputeShader.SetInt(stepSizeId, stepSize);
                jumpFloodComputeShader.Dispatch(kernelHandle, resolution / 8, resolution / 8, 1);
            }

            return resultTexture;
        }


    }
}
