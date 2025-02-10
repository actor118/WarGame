using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace WorldMapStrategyKit {

    public class DemoRandomMap : MonoBehaviour {

        WMSK map;

        public int seed = 10;
        public MapGenerationQuality quality = MapGenerationQuality.Final;
        public int numCells = 512;
        public int numberOfCountries = 24;
        public bool generateProvinces = true;
        public int minimumNumberOfProvincesPerCountry = 2;
        public int maximumNumberOfProvincesPerCountry = 7;
        public bool generateCities = true;
        public int minimumNumberOfCitiesPerCountry = 5;
        public int maximumNumberOfCitiesPerCountry = 20;
        public float seaLevel = 0.25f;
        public float islandFactor;
        public bool drawSeaShoreline = true;
        public int shorelineWidth = 2;
        public int backgroundTextureWidth = 2048;
        public int backgroundTextureHeight = 1024;
        public int heightMapWidth = 1024;
        public int heightMapHeight = 512;
        
        private void Start() {
            map = WMSK.instance;
        }

        void Update() {
            if (map.input.GetKeyDown(KeyCode.Space)) {

                WMSK_Editor mapGen = map.editor;

                // random seed
                mapGen.seed = ++seed;      // random seed for land generation
                mapGen.seedNames = 1;  // random seed for names generation

                // zone to populate with land (full rect is -0.5,-0.5 to 0.5,0.5 for a total width/height of 1.0)
                mapGen.landRect = new Rect(-0.45f, -0.45f, 0.9f, 0.9f);

                // number of countries/provinces/cities to generate
                mapGen.numCountries = numberOfCountries;

                mapGen.generateProvinces = generateProvinces;
                mapGen.numProvincesPerCountryMin = minimumNumberOfProvincesPerCountry;
                mapGen.numProvincesPerCountryMax = maximumNumberOfProvincesPerCountry;

                mapGen.generateCities = generateCities;
                mapGen.numCitiesPerCountryMin = minimumNumberOfCitiesPerCountry;
                mapGen.numCitiesPerCountryMax = maximumNumberOfCitiesPerCountry;

                // sea styling
                mapGen.seaLevel = seaLevel;
                mapGen.seaColor = new Color(0, 0.4f, 1f);
                mapGen.islandFactor = islandFactor;
                mapGen.drawSeaShoreline = drawSeaShoreline;   // draw shoreline
                mapGen.shorelineWidth = shorelineWidth;

                // land styling
                mapGen.heightGradientPreset = HeightMapGradientPreset.RandomHSVColors;
                mapGen.heightGradientMinHue = 0;
                mapGen.heightGradientMaxHue = 1;
                mapGen.heightGradientMinSaturation = 0.1f;
                mapGen.heightGradientMaxSaturation = 0.3f;
                mapGen.heightGradientMinValue = 0.7f;
                mapGen.heightGradientMaxValue = 0.9f;
                mapGen.colorProvinces = false; // if provinces or countries are colored
                mapGen.gradientPerPixel = false;  // fill color uniformly per province or per pixel

                // borders
                mapGen.edgeMaxLength = 0.002f; // maximum length of polygon edges, a lower value makes contour more detailed
                mapGen.drawBorders = true;    // find country borders and draw them
                mapGen.bordersWidth = 4;
                mapGen.bordersIntensity = 0.1f;
                mapGen.smoothBorders = true;   // blur/smooth borders

                // quality of generation
                mapGen.numCells = numCells;
                mapGen.mapGenerationQuality = quality;
                mapGen.backgroundTextureWidth = backgroundTextureWidth;
                mapGen.backgroundTextureHeight = backgroundTextureHeight;
                mapGen.heightMapWidth = heightMapWidth;
                mapGen.heightMapHeight = heightMapHeight;

                // optional features
                mapGen.generateNormalMap = false;      // currently generating normal map at runtime is not possible. Must be generated from the Map Editor.
                mapGen.generateScenicWaterMask = false; // generate Scenic Style water mask with motion vectors
                mapGen.generatePathfindingMaps = false; // do not generate path-finding data; enable if you will use them

                // generate the map
                mapGen.GenerateMap(saveData: false, changeStyle: true);

                map.earthStyle = EARTH_STYLE.NaturalHighRes;

            }
        }
    }

}

