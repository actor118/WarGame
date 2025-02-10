using UnityEngine;

namespace WorldMapStrategyKit {

    public class Demo : MonoBehaviour {

        WMSK map;

        void Start() {

            map = WMSK.instance;

            // Add gradient to Spain
            int spainIndex = map.GetCountryIndex("Spain");

            SurfaceOptions options = new SurfaceOptions();
            options.color = new Color(1, 1, 0, 0); // we'll only display the gradient border, so yellow with 0 alpha
            options.useSDFGradient = true;
            options.sdfBorderColor = Color.yellow;
            options.sdfBorderFallOff = 10;
            options.sdfQuality = SDFQuality.High;

            map.ToggleCountryMainRegionSurface(spainIndex, true, options);


            // Add gradient to 10 random provinces in France
            int franceIndex = map.GetCountryIndex("France");
            Country france = map.GetCountry(franceIndex);

            options.sdfBorderColor = Color.blue;

            map.EnsureProvincesDataIsLoaded(); // since provinces are not drawn, they won't be loaded by default - we force them to load now

            for (int k = 0; k < 10; k++) {
                int randomProvince = Random.Range(0, france.provinces.Length);
                int provinceIndex = map.GetProvinceIndex(france.provinces[randomProvince]);
                map.ToggleProvinceMainRegionSurface(provinceIndex, true, options);
            }

            // Zoom to Spain
            map.FlyToCountry(spainIndex, 2, 0.1f);
        }

    }

}

