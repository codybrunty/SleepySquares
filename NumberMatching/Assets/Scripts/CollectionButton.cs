using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionButton : MonoBehaviour{

    
    public void CollectionButtonOnClick() {
        CollectionManager.CM.NextAssetIndex();
    }


}
