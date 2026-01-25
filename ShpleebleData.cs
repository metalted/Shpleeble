using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeepkistNetworking;

namespace Shpleeble
{
    public class ShpleebleData
    {
        public string chatColor;
        public int color;
        public int color_body;
        public int color_leftArm;
        public int color_leftLeg;
        public int color_rightArm;
        public int color_rightLeg;
        public int frontWheels;
        public int glasses;
        public int hat;
        public int horn;
        public string name;
        public int paraglider;
        public int rearWheels;
        public byte state;
        public ulong steamID;
        public int zeepkist;

        /// <summary>
        /// Converts player cosmetic data into a <see cref="CosmeticsV16"/> object.
        /// </summary>
        /// <returns>The converted cosmetics data.</returns>
        public CosmeticsV16 ToCosmeticsV16()
        {
            CosmeticsV16 cosmetics = new CosmeticsV16();
            cosmetics.zeepkist = (Object_Soapbox)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.zeepkist, zeepkist, false);
            cosmetics.frontwheels = (Object_Wheel)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.wheel, frontWheels, false);
            cosmetics.rearwheels = (Object_Wheel)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.wheel, rearWheels, false);
            cosmetics.paraglider = (Object_Paraglider)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.paraglider, paraglider, false);
            cosmetics.hat = (HatValues)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.hat, hat, false);
            cosmetics.glasses = (HatValues)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.glasses, glasses, false);
            cosmetics.color_body = (CosmeticColor)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.skin, color_body, false);
            cosmetics.color_leftArm = (CosmeticColor)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.skin, color_leftArm, false);
            cosmetics.color_rightArm = (CosmeticColor)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.skin, color_rightArm, false);
            cosmetics.color_leftLeg = (CosmeticColor)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.skin, color_leftLeg, false);
            cosmetics.color_rightLeg = (CosmeticColor)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.skin, color_rightLeg, false);
            cosmetics.horn = (Object_Horn)PlayerManager.Instance.objectsList.wardrobe.GetCosmetic(CosmeticItemType.horn, horn, false);
            return cosmetics;
        }

        public string ToDebugString()
        {
            return $"" +
                $"Name: {name}\n" +
                $"SteamID: {steamID}\n" +
                $"ChatColor: {chatColor}\n" +
                $"Color: {color}\n" +
                $"Color Body: {color_body}\n" +
                $"Color Left Arm: {color_leftArm}\n" +
                $"Color Left Leg: {color_leftLeg}\n" +
                $"Color Right Arm: {color_rightArm}\n" +
                $"Color Right Leg: {color_rightLeg}\n" +
                $"Front Wheels: {frontWheels}\n" +
                $"Glasses: {glasses}\n" +
                $"Hat: {hat}\n" +
                $"Horn: {horn}\n" +
                $"Paraglider: {paraglider}\n" +
                $"Rear Wheels: {rearWheels}\n" +
                $"State: {state}\n" +
                $"Zeepkist: {zeepkist}";
        }
    }
}
