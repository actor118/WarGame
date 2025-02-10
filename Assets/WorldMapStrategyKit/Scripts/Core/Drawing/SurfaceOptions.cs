using UnityEngine;

namespace WorldMapStrategyKit {

    public enum SDFQuality {
        Low,      // 128
        Medium,   // 256
        High,     // 512
        VeryHigh  // 1024
    }

    public class SurfaceOptions {
        public Color color = Color.white;
        public Texture2D texture;
        public Vector2 textureScale = Vector2.one;
        public Vector2 textureOffset;
        public float textureRotation;

        public bool useSDFGradient;
        public Color sdfBorderColor = Color.white;
        public float sdfBorderFallOff = 25;
        public SDFQuality sdfQuality = SDFQuality.Medium;
    }

}