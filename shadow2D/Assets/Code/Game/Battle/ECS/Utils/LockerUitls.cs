
namespace ECS
{
    internal class LockerUitls
    {
        //===========================================================================================================

        private static void SetValue(Entity e,string key,object v) {
            LockerComponent comp = e.GetComponentData<LockerComponent>();
            if (comp == null)
                return;

            if (comp.Objs.ContainsKey(key))
                comp.Objs[key] = v;
            else
                comp.Objs.Add(key, v);
        }

        private static object tempObj;
        private static object GetValue(Entity e, string key,object defaultRet){
            LockerComponent comp = e.GetComponentData<LockerComponent>();
            if (comp == null)
                return defaultRet;

            tempObj = null;
            if (!comp.Objs.TryGetValue(key, out tempObj))
                return defaultRet;

            return tempObj;
        }

        //===========================================================================================================

        public static void SetIntValue(Entity e, string key,int v) { SetValue(e,key,v);}
        public static int GetIntValue(Entity e, string key) {return (int)GetValue(e,key,0);}

        public static void SetStringValue(Entity e, string key, string v) { SetValue(e, key, v); }
        public static string GetStringValue(Entity e, string key) { return (string)GetValue(e, key, ""); }

        //===========================================================================================================

        public static void SetBounceCount(Entity e,int count) { SetIntValue(e, "Bounce",count); }
        public static int GetBounceCount(Entity e) { return GetIntValue(e, "Bounce"); }
    }
}
