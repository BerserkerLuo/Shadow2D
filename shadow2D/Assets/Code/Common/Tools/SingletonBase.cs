
public class SingletonBase<T>where T : new()
{
		
	protected SingletonBase(){}
		
	public static T Singleton
	{
		get 
		{
			if (null == s_singleton)
			{
				//lock(s_objectLock)
				{
					if (null == s_singleton)
					{
						s_singleton = new T();
					}
				}
			}
			return s_singleton;
		}
	}
		
		
	private static T s_singleton = default(T);
	//private static object s_objectLock = new object();
}
