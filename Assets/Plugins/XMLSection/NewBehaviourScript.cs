using UnityEngine;
using System.Collections;

using DataSection;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		XMLParser parser = new XMLParser();
		XMLSection root = parser.loadXML( "<root>" +
		                                 "	<row>" +
		                                 "		<Item>100</Item>" +
		                                 "		<Item>200</Item>" +
		                                 "		<Item>300</Item>" +
		                                 "	</row>" +
		                                 "	<row>" +
		                                 "		<Item> 123 456 789 </Item>" +
		                                 "		<Item> 444 555 666 </Item>" +
		                                 "		<Item> 777 888 999 </Item>" +
		                                 "	</row>" +
		                                 "</root>" );
		//XMLSection root = parser.loadFile( "test" );
		Debug.Log( "root.child[0].name = " + root.child(0).name );
		Debug.Log( "root.child[1].name = " + root.child(1).name );
		Debug.Log( "row/Item = " + root.readInt("row/Item") );
		Debug.Log( "not/in/path = " + root.readVector2("not/in/path") );
		XMLSection childSection = root.child(0);
		foreach ( int i in childSection.readInts ( "Item" ) )
		{
			Debug.Log( "root.child[0] item = " + i );
		}

		childSection = root.child(1);
		foreach ( Vector3 i in childSection.readVector3s( "Item" ) )
		{
			Debug.Log( "root.child[1] item = " + i );
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
