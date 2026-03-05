using Table;
namespace PlayerSystemData
{
    public class HeroGiver : BaseGiver
    {
        public void DoReward(int itemId, int itemCount) {
            //DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //if (itemCfg == null)
            //    return;

            //int heroId,rarity,level;
            //GiverUtils.ParseEquipRewardStr(itemCfg.Value,out heroId,out rarity,out level);

            //HeroSystemUtils.OnActiveHero(heroId, rarity,level);
        }

        public bool CheckOnlyReward(int itemId) {
            //DataItem itemCfg = DataItemManager.Instance.GetDataByID(itemId);
            //if (itemCfg == null)
            //    return true;

            //int heroId, rarity, level;
            //GiverUtils.ParseEquipRewardStr(itemCfg.Value, out heroId, out rarity, out level);

            //if (HeroSystemUtils.HasHero(heroId))
                //return true;

            return false; 
        }
    }
}
