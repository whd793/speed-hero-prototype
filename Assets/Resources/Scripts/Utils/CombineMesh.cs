using UnityEngine;
using System.Collections;
//using UnityEditor;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CombineMesh : MonoBehaviour {

	[SerializeField]
	private bool mergeSubMeshes = true;

	[SerializeField]
	private string folderLocation = "Assets/";

	GameObject highlight;

	private void Start() {
		
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;


//		meshFilters[0].sharedMesh.


		while (i < meshFilters.Length) {
			
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

			i++;
		}



		transform.GetComponent<MeshFilter>().mesh = new Mesh();
		transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, mergeSubMeshes);
		transform.gameObject.active = true;

		SaveMesh ();
	}

	private void SaveMesh() {
		
		Debug.Log ("Saving Mesh");

		Mesh mesh = transform.GetComponent<MeshFilter>().mesh;

//		AssetDatabase.CreateAsset(mesh, folderLocation + transform.name + ".asset"); 
//		AssetDatabase.SaveAssets();
	}
}