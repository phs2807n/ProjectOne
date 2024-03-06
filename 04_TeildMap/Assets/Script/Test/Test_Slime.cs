using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime : TestBase
{
    /// <summary>
    /// �����ӵ��� ������(0:�ƿ�����, 1:������, 2:������������)
    /// </summary>
    public Renderer[] slime;

    /// <summary>
    /// �����ӵ��� ���׸���(0:�ƿ�����, 1:������, 2:������������)
    /// </summary>
    Material[] materials;

    /// <summary>
    /// ���̴� ������Ƽ ���� �ӵ�
    /// </summary>
    public float speed = 1.0f;

    // ���̴� ������Ƽ ���� on/off�� ����
    public bool outlineThicknessChange = false;
    public bool phaseSplitChange = false;
    public bool phaseThicknessChange = false;
    public bool innerlineThicknessChange = false;
    public bool dissolveFadeChange = false;

    /// <summary>
    /// �ð� ������(�ﰢ�Լ����� ���)
    /// </summary>
    float timeElapsed = 0.0f;

    /// <summary>
    /// split ����(������, ���������)
    /// </summary>
    [Range(0f, 1f)]
    public float split = 0.0f;

    /// <summary>
    /// ��������� �� �β�
    /// </summary>
    [Range(0.1f, 0.2f)]
    public float phaseThickness = 0.01f;

    /// <summary>
    /// �ƿ������� �β�
    /// </summary>
    [Range(0.0f, 0.01f)]
    public float outlineThickness = 0.005f;

    /// <summary>
    /// �ζ����� �β�
    /// </summary>
    [Range(0.0f, 0.01f)]
    public float innerlineThickness = 0.005f;

    /// <summary>
    /// ��ź�
    /// </summary>
    [Range(0f, 1f)]
    public float dissolveFade = 0.0f;

    // ������Ƽ ID�� ���ڷ� 
    readonly int SplitID = Shader.PropertyToID("_Split");
    readonly int ReverseSplitID = Shader.PropertyToID("_ReverseSplit");
    readonly int OutlineThicknessID = Shader.PropertyToID("_Thickness");
    readonly int PhaseThicknessID = Shader.PropertyToID("_Thickness");
    readonly int ReverseThicknessID = Shader.PropertyToID("_Thickness");
    readonly int InnerlineThicknessID = Shader.PropertyToID("_Thickness");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");


    private void Start()
    {   
        materials = new Material[slime.Length];     // ��Ƽ���� �̸� ã�Ƽ� ����
        for(int i = 0; i < materials.Length; i++)
        {
            materials[i] = slime[i].material;
        }
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        float num = (Mathf.Cos(timeElapsed * speed) + 1.0f) * 0.5f; // �ð� ��ȭ�� ���� num���� 0 ~ 1�� ��� �����ȴ�.

        if(outlineThicknessChange)
        {
            float min = 0.0f;
            float max = 0.01f;
            float outlineNum = min + (max - min) * num;      // num���� ���� �ּ� ~ �ִ�� ����

            // min = 5;
            // max = 10;
            // num�� 0�̸� 5
            // num�� 0.5�̸� 7.5
            // num�� 1�̸� 10

            materials[0].SetFloat(OutlineThicknessID, outlineNum);
            outlineThickness = outlineNum;
        }
        if (phaseSplitChange)
        {
            materials[1].SetFloat(SplitID, num);
            materials[2].SetFloat(ReverseSplitID, num);
            split = num;
        }
        if (phaseThicknessChange)
        {
            float min = 0.1f;
            float max = 0.2f;
            float phaseNum = min + (max - min) * num;
             
            materials[1].SetFloat(PhaseThicknessID, phaseNum);
            materials[2].SetFloat(ReverseThicknessID, phaseNum);
            phaseThickness = phaseNum;
        }
        if(innerlineThicknessChange)
        {
            float min = 0.0f;
            float max = 0.01f;
            float innerlineNum = min + (max - min) * num;      // num���� ���� �ּ� ~ �ִ�� ����
            materials[3].SetFloat(InnerlineThicknessID, innerlineNum);
            innerlineThickness = innerlineNum;
        }
        if(dissolveFadeChange)
        {
            materials[4].SetFloat(DissolveFadeID, num);
            innerlineThickness = num;
            dissolveFade = num;
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Renderer renderer = slime.GetComponent<Renderer>();
        //Material material = renderer.material;
        //int id = Shader.PropertyToID("_Split");
        ////material.SetFloat("_Split", split);
        //material.SetFloat(id, split);

        // �ƿ������� �β� �����غ���
        outlineThicknessChange = !outlineThicknessChange;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        // Phase�� ReversePhase �Ѵ� �����ϱ�
        phaseSplitChange = !phaseSplitChange;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        // Phase�� ReversePhase�� �β� �����ϱ�
        phaseThicknessChange = !phaseThicknessChange;
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        // InnerLine �β� �����ϱ�
        innerlineThicknessChange = !innerlineThicknessChange;
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        // Dissolve�� fade �����ϱ�
        dissolveFadeChange = !dissolveFadeChange;
    }
}
