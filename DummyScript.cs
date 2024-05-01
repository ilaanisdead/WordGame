using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class DummyScript : MonoBehaviour
{

    AddressableAssetGroup Adss;
    // Start is called before the first frame update
    void Start()
    {
        // AsyncOperationHandle asds = Addressables.LoadAssetsAsync<AddressableAssetGroup>("");  
        // asds.Completed=()=>{Debug.Log("");};  
    }

}
