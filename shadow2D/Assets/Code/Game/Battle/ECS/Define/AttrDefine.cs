using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS
{
    partial class AttrType
    {
        public const int Lv1Begin   = 1;

        public const int Str        = 1;    //力量
        public const int Int        = 2;    //智力
        public const int Agi        = 3;    //敏捷
        public const int Phy        = 4;    //体质
        public const int Spi        = 5;    //精神
        public const int Luck       = 6;    //幸运

        public const int Lv1End     = 10;

        public const int Lv2Begin   = 10;

        public const int HPMax      = 10;   //最大血量
        public const int HP         = 11;   //血量
        public const int Atk        = 12;   //攻击
        public const int Crit       = 13;   //暴击率 
        public const int CritEffect = 14;   //暴击伤害倍率
        public const int MoveSpeed  = 15;   //移动速度
        public const int BodySize   = 16;   //体型
        public const int ReloadSpeed = 17;  //重载速度 重载时间 * (1-ReloadSpeed)
        public const int BulletNum  = 18;   //子弹数量
        public const int ShotNum    = 19;   //每次发射数量
        public const int ShotSpeed   = 20;  //子弹发射速度 子弹技能CD * (1-ShotSpeed)
        public const int BulletSpeed = 21;  //子弹移动速度
        public const int WeaponRange = 22;  //武器射程
        public const int PickUpRange = 23;  //拾取范围

        //public const int BulletPenet = 100;    //子弹穿透次数
        //public const int BulletCatapult = 101; //子弹弹射次数

        public const int Lv2End     = 100;

        //public const int Megic      = 16;    //魔力
        //public const int MegDef     = 17;    //法抗
        //public const int DefPenet   = 24;   //护甲穿透
        //public const int MDefPenet  = 25;   //魔抗穿透
        //public const int HPRecover  = 26;   //HP恢复
        //public const int MPRecover  = 27;   //MP恢复
        //public const int MPMax      = 12;    //最大法力 
        //public const int MP         = 13;    //法力
        //public const int Def = 15;    //防御


        #region
        private static Dictionary<string, int> AttrStr = new Dictionary<string, int>();
        private static Dictionary<int, string> AttrName = new Dictionary<int, string>();
       
        public static List<int> DetailAttrs { get { return _DetailAttrList; } }
        private static List<int> _DetailAttrList = new List<int>();

        public static List<int> BrefAttrs { get { return _BrefAttrList; } }
        private static List<int> _BrefAttrList = new List<int>();

        public static List<int> BrefAttrs2 { get { return _BrefAttrList2; } }
        private static List<int> _BrefAttrList2 = new List<int>();

        static AttrType() {
            RegisterAttrName("Str", Str);
            RegisterAttrName("Int", Int);
            RegisterAttrName("Agi", Agi);
            RegisterAttrName("Phy", Phy);
            RegisterAttrName("Spi", Spi);
            RegisterAttrName("Luck", Luck);

            RegisterAttrName("CurHP", HP, false);    //当前血量
            RegisterAttrName("HP", HPMax);    //最大血量
            //RegisterAttrName("CurMP", MP, false);    //当前血量
            //RegisterAttrName("MP", MPMax);    //最大法力
            RegisterAttrName("Atk", Atk);    //攻击
            //RegisterAttrName("Def", Def);    //防御
            //RegisterAttrName("DefPe", DefPenet);   //护甲穿透
            //RegisterAttrName("Megic", Megic);    //魔力
            //RegisterAttrName("MegDef", MegDef);    //法抗
            //RegisterAttrName("MDefPe", MDefPenet);   //魔抗穿透
            RegisterAttrName("Crit", Crit);    //暴击率 
            RegisterAttrName("CritEff", CritEffect);   //暴击效果
            //RegisterAttrName("CritDef", CritDef);   //暴击抵抗
            RegisterAttrName("AtkSpeed", ShotSpeed);   //速度
            RegisterAttrName("CDReduce", ReloadSpeed);   //技能CD缩减
            RegisterAttrName("Speed", MoveSpeed);   //速度
            //RegisterAttrName("HPRec", HPRecover);   //生命恢复
            //RegisterAttrName("MPRec", MPRecover);   //法力恢复

            _BrefAttrList.Add(Str);
            _BrefAttrList.Add(Int);
            _BrefAttrList.Add(Agi);
            _BrefAttrList.Add(Phy);
            _BrefAttrList.Add(Spi);
            _BrefAttrList.Add(Luck);

            _BrefAttrList2.Add(Str);
            _BrefAttrList2.Add(Phy);
            _BrefAttrList2.Add(Int);
            _BrefAttrList2.Add(Spi);
            _BrefAttrList2.Add(Agi);
            _BrefAttrList2.Add(Luck);
        }

        public static void RegisterAttrName(string attrName, int attrType,bool addSort = true) {
            AttrStr.Add(attrName, attrType);
            AttrName.Add(attrType, attrName);
            if(addSort) _DetailAttrList.Add(attrType);
        }

        public static int GetAttrType(string attrStr) {
            return AttrStr.GetValueOrDefault(attrStr,-1);
        }

        public static string GetAttrName(int attrType) {
            return AttrName.GetValueOrDefault(attrType, "");
        }

        #endregion
    }


}
