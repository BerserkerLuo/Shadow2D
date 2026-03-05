
using Table;
namespace PlayerSystemData
{

    /// <summary>
    /// 给装备
    /// </summary>
    public class EquipGiver : BaseGiver
    {
        public void DoReward(int itemId, int itemCount)
        {
            //DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //if (itemCfg == null)
            //    return;

            //int equipId, rarity, level;
            //GiverUtils.ParseEquipRewardStr(itemCfg.Value, out equipId, out rarity, out level);

            //EquipSystemUtils.AddEquip(equipId);
        }

        public bool CheckOnlyReward(int itemId)
        {
            //DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //if (itemCfg == null)
            //    return true;

            //int equipId, rarity, level;
            //GiverUtils.ParseEquipRewardStr(itemCfg.Value, out equipId, out rarity, out level);

            //if (EquipSystemUtils.HasEquip(equipId))
                //return true;
            return false;
        }
    }
}
