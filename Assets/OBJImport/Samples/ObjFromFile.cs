using Dummiesman;
using System.IO;
using UnityEngine;

public class ObjFromFile : MonoBehaviour
{
    string objPath = string.Empty;
    string error = string.Empty;
    public GameObject loadedObject;
    public ApplicationMenu applicationMenu;

    private void Start()
    {
        
    }

    void OnGUI() {
        //objPath = GUI.TextField(new Rect(0, 0, 256, 32), objPath);
        objPath = MainMenu.objectPath;

        GUI.Label(new Rect(0, 0, 256, 32), "Obj Path:");
        if(GUI.Button(new Rect(256, 32, 64, 32), "Load File"))
        {
            //file path
            if (!File.Exists(objPath))
            {
                error = "File doesn't exist.";
            }else{
                if(loadedObject != null)            
                    Destroy(loadedObject);
                loadedObject = new OBJLoader().Load(objPath);
                loadedObject.AddComponent<MeshCollider>();
                loadedObject.layer = LayerMask.NameToLayer("RayCast");
                foreach (Transform childObject in loadedObject.transform)//transform)
                {
                    // First we get the Mesh attached to the child object
                    Mesh mesh = childObject.gameObject.GetComponent<MeshFilter>().mesh;

                    // If we've found a mesh we can use it to add a collider
                    if (mesh != null)
                    {
                        // Add a new MeshCollider to the child object
                        MeshCollider meshCollider = childObject.gameObject.AddComponent<MeshCollider>();

                        childObject.gameObject.layer = LayerMask.NameToLayer("RayCast");
                        // Finaly we set the Mesh in the MeshCollider
                        meshCollider.sharedMesh = mesh;
                    }
                    else
                    {
                        Debug.Log("no childs found");
                    }
                }


                applicationMenu.InitializeGameObject(loadedObject);
           //ApplicationMenu.
                //rayTest.startScan();
                error = string.Empty;
            }
        }

        if(!string.IsNullOrWhiteSpace(error))
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(0, 64, 256 + 64, 32), error);
            GUI.color = Color.white;
        }
    }
}
