using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
 Material mat;
    public float scrollSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //배경이 계속 스크롤 되게 하고 싶다.
        //->매터리얼의 offset속성의 y값을 키워준다.
        //0. Mesh Renderer 컴포넌트 얻어오기
        MeshRenderer mr= gameObject.GetComponent<MeshRenderer>();
        //1. Material이 있어야 한다.
        mat = mr.material;
        //2. offset을 찾아야 한다.(offset의 y값을 찾는다)
        //3. y값을 키워준다.
        mat.mainTextureOffset += Vector2.up * scrollSpeed * Time.deltaTime;
    }

}
