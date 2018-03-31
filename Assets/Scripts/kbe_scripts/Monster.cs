namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	
    public class Monster : MonsterBase
    {
		public Monster() : base()
		{
		}

		public override void recvDamage(Int32 attackerID, Int32 skillID, Int32 damageType, Int32 damage)
		{
			// Dbg.DEBUG_MSG(className + "::recvDamage: attackerID=" + attackerID + ", skillID=" + skillID + ", damageType=" + damageType + ", damage=" + damage);
			
			Entity entity = KBEngineApp.app.findEntity(attackerID);

			Event.fireOut("recvDamage", new object[]{this, entity, skillID, damageType, damage});
		}

		public override void onHPChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_HP: " + old + " => " + v); 
			Event.fireOut("set_HP", new object[]{this, HP, HP_Max});
		}
		
		public override void onMPChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_MP: " + old + " => " + v); 
			Event.fireOut("set_MP", new object[]{this, MP, MP_Max});
		}
		
		public override void onHP_MaxChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_HP_Max: " + old + " => " + v); 
			Event.fireOut("set_HP_Max", new object[]{this, HP_Max, HP});
		}
		
		public override void onMP_MaxChanged(Int32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_MP_Max: " + old + " => " + v); 
			Event.fireOut("set_MP_Max", new object[]{this, MP_Max, MP});
		}
		
		public override void onNameChanged(string oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_name: " + old + " => " + v); 
			Event.fireOut("set_name", new object[]{this, name});
		}
		
		public override void onStateChanged(SByte oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_state: " + old + " => " + v); 
			Event.fireOut("set_state", new object[]{this, state});
		}

		public override void onMoveSpeedChanged(Byte oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_moveSpeed: " + old + " => " + v); 
			Event.fireOut("set_moveSpeed", new object[]{this, moveSpeed});
		}
		
		public override void onModelScaleChanged(Byte oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_modelScale: " + old + " => " + v); 
			Event.fireOut("set_modelScale", new object[]{this, modelScale});
		}
		
		public override void onModelIDChanged(UInt32 oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_modelID: " + old + " => " + v); 
			Event.fireOut("set_modelID", new object[]{this, modelID});
		}

    }
} 
