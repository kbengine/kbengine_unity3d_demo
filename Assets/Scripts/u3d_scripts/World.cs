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
		KBEngine.Event.registerOut("onEnterWorld", this, "onEnterWorld");
		KBEngine.Event.registerOut("onLeaveWorld", this, "onLeaveWorld");
		KBEngine.Event.registerOut("set_position", this, "set_position");
		KBEngine.Event.registerOut("set_direction", this, "set_direction");
		KBEngine.Event.registerOut("updatePosition", this, "updatePosition");
		KBEngine.Event.registerOut("onControlled", this, "onControlled");
		
		// in world(register by scripts)
		KBEngine.Event.registerOut("onAvatarEnterWorld", this, "onAvatarEnterWorld");
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
			KBEngine.Event.fireIn("jump");
        }
		else if (Input.GetMouseButton (0))     
		{   
			// 射线选择，攻击
			if(Camera.main)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
				RaycastHit hit;   

				if (Physics.Raycast(ray, out hit))     
				{   
					Debug.DrawLine (ray.origin, hit.point); 
					UnityEngine.GameObject gameObj = hit.collider.gameObject;
					if(gameObj.name.IndexOf("terrain") == -1)
					{
						string[] s = gameObj.name.Split(new char[]{'_' });
						
						if(s.Length > 0)
						{
							int targetEntityID = Convert.ToInt32(s[s.Length - 1]);
							KBEngine.Event.fireIn("useTargetSkill", (Int32)1, (Int32)targetEntityID);	
						}	
					}
				}  
			}
		}    
	}
	
	public void addSpaceGeometryMapping(string respath)
	{
		// 这个事件可以理解为服务器通知客户端加载指定的场景资源
		// 通过服务器的api KBEngine.addSpaceGeometryMapping设置到spaceData中，进入space的玩家就会被同步spaceData里面的内容
		Debug.Log("loading scene(" + respath + ")...");

		UI.inst.info("scene(" + respath + "), spaceID=" + KBEngineApp.app.spaceID);
		if(terrain == null)
			terrain = Instantiate(terrainPerfab) as UnityEngine.GameObject;

		if(player)
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
	}

	public void createPlayer()
	{
		// 需要等场景加载完毕再显示玩家
		if (player != null)
		{
			if(terrain != null)
				player.GetComponent<GameEntity>().entityEnable();
			return;
		}
		
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
		if(avatar.isOnGround)
			y = 1.3f;
		
		player = Instantiate(avatarPerfab, new Vector3(avatar.position.x, y, avatar.position.z), 
		                     Quaternion.Euler(new Vector3(avatar.direction.y, avatar.direction.z, avatar.direction.x))) as UnityEngine.GameObject;

		player.GetComponent<GameEntity>().entityDisable();
		avatar.renderObj = player;
		((UnityEngine.GameObject)avatar.renderObj).GetComponent<GameEntity>().isPlayer = true;
		
		// 有必要设置一下，由于该接口由Update异步调用，有可能set_position等初始化信息已经先触发了
		// 那么如果不设置renderObj的位置和方向将为0，人物会陷入地下
		set_position(avatar);
		set_direction(avatar);
        set_entityName(avatar, avatar.name);
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
		if(entity.isOnGround)
			y = 1.3f;
		
		entity.renderObj = Instantiate(entityPerfab, new Vector3(entity.position.x, y, entity.position.z), 
			Quaternion.Euler(new Vector3(entity.direction.y, entity.direction.z, entity.direction.x))) as UnityEngine.GameObject;

		((UnityEngine.GameObject)entity.renderObj).name = entity.className + "_" + entity.id;
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

		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.destPosition = entity.position;
		gameEntity.position = entity.position;
		gameEntity.spaceID = KBEngineApp.app.spaceID;
	}

	public void updatePosition(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.destPosition = entity.position;
		gameEntity.isOnGround = entity.isOnGround;
		gameEntity.spaceID = KBEngineApp.app.spaceID;
	}
	
	public void onControlled(KBEngine.Entity entity, bool isControlled)
	{
		if(entity.renderObj == null)
			return;
		
		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.isControlled = isControlled;
	}
	
	public void set_direction(KBEngine.Entity entity)
	{
		if(entity.renderObj == null)
			return;
		
		GameEntity gameEntity = ((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>();
		gameEntity.destDirection = new Vector3(entity.direction.y, entity.direction.z, entity.direction.x); 
		gameEntity.spaceID = KBEngineApp.app.spaceID;
	}

	public void set_HP(KBEngine.Entity entity, Int32 v, Int32 HP_Max)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = "" + v + "/" + HP_Max;
		}
	}
	
	public void set_MP(KBEngine.Entity entity, Int32 v, Int32 MP_Max)
	{
	}
	
	public void set_HP_Max(KBEngine.Entity entity, Int32 v, Int32 HP)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().hp = HP + "/" + v;
		}
	}
	
	public void set_MP_Max(KBEngine.Entity entity, Int32 v, Int32 MP)
	{
	}
	
	public void set_level(KBEngine.Entity entity, UInt16 v)
	{
	}
	
	public void set_entityName(KBEngine.Entity entity, string v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().entity_name = v;
		}
	}
	
	public void set_state(KBEngine.Entity entity, SByte v)
	{
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().set_state(v);
		}
		
		if(entity.isPlayer())
		{
			Debug.Log("player->set_state: " + v);
			
			if(v == 1)
				UI.inst.showReliveGUI = true;
			else
				UI.inst.showReliveGUI = false;
			
			return;
		}
	}

	public void set_moveSpeed(KBEngine.Entity entity, Byte v)
	{
		float fspeed = ((float)(Byte)v) / 10f;
		
		if(entity.renderObj != null)
		{
			((UnityEngine.GameObject)entity.renderObj).GetComponent<GameEntity>().speed = fspeed;
		}
	}
	
	public void set_modelScale(KBEngine.Entity entity, Byte v)
	{
	}
	
	public void set_modelID(KBEngine.Entity entity, UInt32 v)
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
