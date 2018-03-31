namespace KBEngine
{
  	using UnityEngine; 
	using System; 
	using System.Collections; 
	using System.Collections.Generic;
	
    public class NPC : NPCBase
    {
		public NPC() : base()
		{
		}
		
		public override void onNameChanged(string oldValue)
		{
			// Dbg.DEBUG_MSG(className + "::set_name: " + old + " => " + v); 
			Event.fireOut("set_name", new object[]{this, name});
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
