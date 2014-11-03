using KBEngine;
using UnityEngine;
using System; 
using System.IO;  
using System.Collections; 
using System.Collections.Generic;
using System.Linq;

public class World : MonoBehaviour 
{
	private UnityEngine.GameObject terrain = null;
	public UnityEngine.GameObject terrainPerfab;
	
	private UnityEngine.GameObject player = null;
	public UnityEngine.GameObject entityPerfab;
	public UnityEngine.GameObject avatarPerfab;
	
	void Awake() 
	 {
		DontDestroyOnLoad(transform.gameObject);
	 }
	 
	// Use this for initialization
	void Start () 
	{
		installEvents();
	}

	void installEvents()
	{
		// in world
		KBEngine.Event.registerOut("addSpaceGeometryMapping", this, "addSpaceGeometryMapping");
		KBEngine.Event.registerOut("onAvatarEnterWorld", this, "onAvatarEnterWorld");
		KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
		KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
		KBEngine.Event.registerOut("set_position", this, "set_position");
		KBEngine.Event.registerOut("set_direction", this, "set_direction");
		KBEngine.Event.registerOut("update_position", this, "update_position");
		KBEngine.Event.registerOut("set_HP", this, "set_HP");
		KBEngine.Event.registerOut("set_MP", this, "set_MP");
		KBEngine.Event.registerOut("set_HP_Max", this, "set_HP_Max");
		KBEngine.Event.registerOut("set_MP_Max", this, "set_MP_Max");
		KBEngine.Event.registerOut("set_level", this, "set_level");
		KBEngine.Event.registerOut("set_name", this, "set_entityName");
		KBEngine.Event.registerOut("set_state", this, "set_state");
		KBEngine.Event.registerOut("set_moveSpeed", this, "set_moveSpeed");
		KBEngine.Event.registerOut("set_modelScale", this, "set_modelScale");
		KBEngine.Event.registerOut("set_modelID", this, "set_modelID");
		KBEngine.Event.registerOut("recvDamage", this, "recvDamage");
		KBEngine.Event.registerOut("otherAvatarOnJump", this, "otherAvatarOnJump");
		KBEngine.Event.registerOut("onAddSkill", this, "onAddSkill");
	}

	void OnDestroy()
	{
		KBEngine.Event.deregisterOut(this);
	}
	
	// Update is called once per frame
	void Update () 
	{
		createPlayer();
        if (Input.GetKeyUp(KeyCode.Space))
        {
			Debug.Log("KeyCode.Space");
			
			if(KBEngineApp.app.entity_type == "Avatar")
			{
				KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
				if(avatar != null)
					avatar.jump();
			}
        }
	}
	
	public void addSpaceGeometryMapping(string respath)
	{
		Debug.Log("loading scene(" + respath + ")...");
		UI.inst.info("scene(" + respath + "), spaceID=" + KBEngineApp.app.spaceID);
		if(terrain == null)
			terrain = Instantiate(terrainPerfab) as UnityEngine.GameObject;

		player.GetComponent<GameEntity>().entityEnable();
	}	
	
	public void onAvatarEnterWorld(UInt64 rndUUID, Int32 eid, KBEngine.Avatar avatar)
	{
		if(!avatar.isPlayer())
		{
			return;
		}

		UI.inst.info("loading scene...(加载场景中...)");
		Debug.Log("loading scene...");
		
		object speed = avatar.getDefinedPropterty("moveSpeed");
		if(speed != null)
			set_moveSpeed(avatar, speed);
		
		object state = avatar.getDefinedPropterty("state");
		if(state != null)
			set_state(avatar, state);
		
		object modelScale = avatar.getDefinedPropterty("modelScale");
		if(modelScale != null)
			set_modelScale(avatar, modelScale);
		
		object name = avatar.getDefinedPropterty("name");
		if(name != null)
			set_entityName(avatar, (string)name);
		
		object hp = avatar.getDefinedPropterty("HP");
		if(hp != null)
			set_HP(avatar, hp);
	}

	public void createPlayer()
	{
		if (player != null)
			return;

		if (KBEngineApp.app.entity_type != "Avatar") {
			return;
		}

		KBEngine.Avatar avatar = (KBEngine.Avatar)KBEngineApp.app.player();
		if(avatar == null)
		{
			Debug.Log("wait create(palyer)!");
			return;
		}
		
		float y = avatar.position.y;
		if(avatar.isOnGound)
			y = 1.3f;
		
		player = Instantiate(avatarPerfab, new Vector3(avatar.position.x, y, avatar.position.z), 
		                     Quaternion.Euler(new Vector3(avatar.direction.y, avatar.direction.z, avatar.direction.x))) as UnityEngine.GameObject;

		player.GetComponent<GameEntity>().entityDisable();
		avatar.renderObj = player;
		((UnityEngine.GameObject)avatar.renderObj).GetComponent<GameEntity>().isPlayer = true;
	}

	public void onAddSkill(KBEngine.Entity entity)
	{
		Debug.Log("onAddSkill");
	}
	
	public void onEnterWorld(KBEngine.Entity entity)
	{
		if(entity.isPlayer())
			return;
		
		float y = entity.position.y;
		if(entity.isOnGound)
			y = 1.3f;
		
		entity.renderObj = Instantiate(entityPerfab, new Vector3(entity.position.x, y, entity.position.z), 
			Quaternion.Euler(new Vector3(entity.direction.y, entity.direction.z, entity.direction.x))) as UnityEngine.GameObject;

		((UnityEngine.GameObject)entity.renderObj).name = entity.classtype + entity.id;
		
		object speed = entity.getDefinedPropterty("moveSpeed");
		if(speed != null)
			set_moveSpeed(entity, speed);
		
		object state = entity.getDefinedPropterty("state");
		if(state != null)
			set_state(entity, state);
		
		object modelScale = entity.getDefinedPropterty("modelScale");
		if(modelScale != null)
			set_modelScale(entity, modelScale);
		
		object name = entity.getDefinedPropterty("name");
		if(name != null)
			set_entityName(entity, (string)name);
		
		object hp = entity.getDefinedPropterty("HP");
		if(hp != null)
			set_HP(entity, hp);
	}
	
	public void onLeaveWorld(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		UnityEngine.GameObject.Destroy((UnityEngine.GameObject)entity.renderObj);
		entity.renderObj = null;
	}

	public void set_position(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;

		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destPosition = entity.position;
		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().position = entity.position;
	}

	public void update_position(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.destPosition = entity.position;
		gameEntity.isOnGound = entity.isOnGound;
	}
	
	public void set_direction(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().destDirection = 
			new Vector3(entity.direction.y, entity.direction.z, entity.direction.x); 
	}

	public void set_HP(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = "" + (Int32)v + "/" + (Int32)entity.getDefinedPropterty("HP_Max");
		}
	}
	
	public void set_MP(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_HP_Max(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = (Int32)entity.getDefinedPropterty("HP") + "/" + (Int32)v;
		}
	}
	
	public void set_MP_Max(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_level(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_entityName(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().entity_name = (string)v;
		}
	}
	
	public void set_state(KBEngine.Entity entity, object v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().set_state((SByte)v);
		}
		
		if(entity.isPlayer())
		{
			Debug.Log("player->set_state: " + v);
			
			if(((SByte)v) == 1)
				UI.inst.showReliveGUI = true;
			else
				UI.inst.showReliveGUI = false;
			
			return;
		}
	}

	public void set_moveSpeed(KBEngine.Entity entity, object v)
	{
		float fspeed = ((float)(Byte)v) / 10f;
		
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().speed = fspeed;
		}
	}
	
	public void set_modelScale(KBEngine.Entity entity, object v)
	{
	}
	
	public void set_modelID(KBEngine.Entity entity, object v)
	{
	}
	
	public void recvDamage(KBEngine.Entity entity, KBEngine.Entity attacker, Int32 skillID, Int32 damageType, Int32 damage)
	{
	}
	
	public void otherAvatarOnJump(KBEngine.Entity entity)
	{
		Debug.Log("otherAvatarOnJump: " + entity.id);
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().OnJump();
		}
	}
}
