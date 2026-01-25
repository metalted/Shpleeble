using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shpleeble
{
    public static class ShpleebleUtils
    {
        public static ShpleebleData GetLocalData()
        {
            try
            {
                ZeepkistNetworking.CosmeticIDs cosmeticIDs = ProgressionManager.Instance.GetAdventureCosmetics();

                ShpleebleData playerData = new ShpleebleData()
                {
                    chatColor = GetOnlinePlayerHexColor(),
                    color = cosmeticIDs.color,
                    color_body = cosmeticIDs.color_body,
                    color_leftArm = cosmeticIDs.color_leftArm,
                    color_leftLeg = cosmeticIDs.color_leftLeg,
                    color_rightArm = cosmeticIDs.color_rightArm,
                    color_rightLeg = cosmeticIDs.color_rightLeg,
                    frontWheels = cosmeticIDs.frontWheels,
                    glasses = cosmeticIDs.glasses,
                    hat = cosmeticIDs.hat,
                    horn = cosmeticIDs.horn,
                    name = PlayerManager.Instance.steamAchiever.GetPlayerName(false),
                    paraglider = cosmeticIDs.paraglider,
                    rearWheels = cosmeticIDs.rearWheels,
                    state = 0,
                    steamID = PlayerManager.Instance.steamAchiever.GetPlayerSteamID(),
                    zeepkist = cosmeticIDs.zeepkist
                };

                return playerData;
            }
            catch
            {
                return new ShpleebleData()
                {
                    chatColor = "#FFFFFF",
                    color = 0,
                    color_body = 0,
                    color_leftArm = 0,
                    color_leftLeg = 0,
                    color_rightArm = 0,
                    color_rightLeg = 0,
                    frontWheels = 0,
                    glasses = 0,
                    hat = 0,
                    horn = 0,
                    name = "null",
                    paraglider = 0,
                    rearWheels = 0,
                    state = 0,
                    steamID = 0,
                    zeepkist = 0
                };
            }          
        }

        private static string GetOnlinePlayerHexColor()
        {
            float h = PlayerManager.Instance.instellingen.GlobalSettings.online_name_color_H;
            float s = PlayerManager.Instance.instellingen.GlobalSettings.online_name_color_S;
            float v = PlayerManager.Instance.instellingen.GlobalSettings.online_name_color_V;

            //Convert the hsv to hex 
            return HsvToHex(h, s, v);
        }

        private static string HsvToHex(float h, float s, float v)
        {
            // Ensure HSV values are within expected ranges
            h = Math.Clamp(h, 0f, 360f);
            s = Math.Clamp(s, 0f, 1f);
            v = Math.Clamp(v, 0f, 1f);

            // Convert HSV to RGB
            int hi = (int)(h / 60) % 6;
            float f = (h / 60) - hi;
            float p = v * (1 - s);
            float q = v * (1 - f * s);
            float t = v * (1 - (1 - f) * s);

            float r = 0, g = 0, b = 0;

            switch (hi)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                case 5: r = v; g = p; b = q; break;
            }

            // Convert RGB floats to integers (0-255)
            int rInt = (int)(r * 255);
            int gInt = (int)(g * 255);
            int bInt = (int)(b * 255);

            // Convert to hexadecimal string
            return $"#{rInt:X2}{gInt:X2}{bInt:X2}";
        }
    }
}
