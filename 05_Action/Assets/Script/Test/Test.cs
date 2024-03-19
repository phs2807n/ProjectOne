using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        GameObject objName = GameObject.Find("이름");                           // 이름으로 찾기
        GameObject objTag1 = GameObject.FindWithTag("태크");                    // 태그로 찾기 - 함수 이름만 줄여 놓은 것
        GameObject objTag2 = GameObject.FindGameObjectWithTag("태크");          // 태그로 찾기
        GameObject[] objTag3 = GameObject.FindGameObjectsWithTag("태그");       // 같은 태그 모두 찾기
        GameObject objType1 = GameObject.FindAnyObjectByType<GameObject>();     // 특정 타입으로 찾기(아무거나 1개만, 뭐가 나울지 예상불가능, FindFirst
        GameObject objType2 = GameObject.FindFirstObjectByType<GameObject>();   // 특정 타입으로 찾기(타입 중 첫 번째)

        // 특정타입 모두 찾기(파라메터로 비활성화된 오브젝트 포함할시 결정가능)
        GameObject[] objType3 = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

        // FindObjectOfType : 비권장

        // 컴포넌트 찾기
        Transform t = GetComponent<Transform>();        // = this.transform;

        // 컴포넌트 추가하기
        this.gameObject.AddComponent<Test>();
    }
}
