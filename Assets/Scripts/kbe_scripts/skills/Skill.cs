namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	
    public class Skill 
    {
    	public string name;
    	public string descr;
    	public Int32 id;
    	public float canUseDistMin = 0f;
    	public float canUseDistMax = 3f;

        // 最后一次使用时间， 最好的做法是给客户端实体增加cooldown系统，由于demo只是简单展示，就这样简单实现功能
    	public System.DateTime lastUsedTime = System.DateTime.Now;

        public Skill()
		{
		}
		
		public bool validCast(KBEngine.Entity caster, SCObject target)
		{
            TimeSpan span = DateTime.Now - lastUsedTime;
            if (span.TotalMilliseconds < 300)
                return false;

            float dist = Vector3.Distance(target.getPosition(), caster.position);
			if(dist > canUseDistMax)
				return false;
			
			return true;
		}
		
		public void use(KBEngine.Entity caster, SCObject target)
		{
			caster.cellCall("useTargetSkill", id, ((SCEntityObject)target).targetID);
            lastUsedTime = System.DateTime.Now;
        }
    }
} 
