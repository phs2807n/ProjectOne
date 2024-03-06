using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Slime2 : TestBase
{
    /// <summary>
    /// �ڵ�� ����Ʈ�� ������ ��Ƽ������ ���� ������
    /// </summary>
    public Renderer slimeRenderer;

    /// <summary>
    /// �ڵ�� ������ ��Ƽ����
    /// </summary>
    Material mainMaterial;

    /// <summary>
    /// �ƿ������� ���� ���� �β�
    /// </summary>
    const float VisibleOutlinethickness = 0.004f;

    /// <summary>
    /// ����� ���� ���� �β�
    /// </summary>
    const float VisiblePhaseThickness = 0.1f;

    // ���̴� ������Ƽ ���̵��
    readonly int OutlineThicknessID = Shader.PropertyToID("_OutlineThickness");
    readonly int PhaseSplitID = Shader.PropertyToID("_PhaseSplit");
    readonly int PhaseThicknessID = Shader.PropertyToID("_PhaseThinknes");
    readonly int DissolveFadeID = Shader.PropertyToID("_DissolveFade");

    private void Start()
    {
        mainMaterial = slimeRenderer.material; // ��Ƽ���� ��������
    }

    void ResetShaderProperty()
    {
        //  - ����
        ShowOutline(false);                         // �ƿ����� ����
        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �ϱ�
        mainMaterial.SetFloat(PhaseSplitID, 0);     // ���� ���̰� �ϱ�
        mainMaterial.SetFloat(DissolveFadeID, 1);   // ������ �Ⱥ��̰� �ϱ�
    }

    /// <summary>
    /// �ƿ����� �Ѱ� ���� �Լ�
    /// </summary>
    /// <param name="isShow"></param>
    void ShowOutline(bool isShow)
    {
        //  - Outline on/off
        if(isShow)
        {
            mainMaterial.SetFloat(OutlineThicknessID, VisibleOutlinethickness); // ���̴� ���� �β��� �����ϴ� ������ ���̰� ����
        }
        else
        {
            mainMaterial.SetFloat(OutlineThicknessID, 0);   // �Ⱥ��̴� ���� �β��� 0���� ���� �� ���̰� ����
        }
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ(�Ⱥ��� -> ���̱�
    /// </summary>
    /// <returns></returns>
    IEnumerator StartPhase()
    {
        //  - PhaseReverse�� �Ⱥ��̴� ���¿��� ���̰� ����� (1->0)

        float phaseDuration = 0.5f;                     // ������ ����ð�
        float phaseNormalize = 1.0f / phaseDuration;    // ������ ����� ���̱� ���� �̸� ���

        float timeElapsed = 0.0f;   // �ð� ������

        mainMaterial.SetFloat(PhaseThicknessID, VisiblePhaseThickness); // ������ ���� ���̰� �����

        while(timeElapsed < phaseDuration)
        {
            timeElapsed += Time.deltaTime;

            //mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed * phaseNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(PhaseThicknessID, 0); // ������ �� �Ⱥ��̰� �����
        mainMaterial.SetFloat(PhaseSplitID, 0);     // ���ڸ� ����ϰ� �����ϱ� ���� ��
    }

    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator StartDissolve()
    {
        //  - Dissolve �����Ű��(1->0)
        float dissolveDuration = 0.5f;
        float dissolveNormalize = 1.0f / dissolveDuration;

        float timeElapsed = 0.0f;

        while (timeElapsed < dissolveDuration)
        {
            timeElapsed += Time.deltaTime;

            //mainMaterial.SetFloat(PhaseSplitID, 1 - (timeElapsed / dissolveDuration));
            mainMaterial.SetFloat(DissolveFadeID, 1 - (timeElapsed * dissolveNormalize));

            yield return null;
        }

        mainMaterial.SetFloat(DissolveFadeID, 0); 
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ResetShaderProperty();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        ShowOutline(true);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        ShowOutline(false);
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        StartCoroutine(StartPhase());
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        StartCoroutine(StartDissolve());
    }
}
