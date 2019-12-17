using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Canon))]
public class TowerFOVEditor : Editor
{
    private void OnSceneGUI()
    {
        // 캐논 클래스 참조.
        Canon canon = (Canon)target;
        // 원주 위의 시작점 좌표를 계산(주어진 각도의 1/2)
        Vector3 fromAnglePos = canon.CirclePoint(-canon.ViewAngle * 0.5f);
        // 원의 색상을 설정.
        Handles.color = Color.white;
        // 외곽선만 표시하는 원반을 그리기.
        Handles.DrawWireDisc(canon.transform.position, Vector3.up, canon.ViewAngle);
        // 부채꼴 색상을 표시.
        Handles.color = new Color(1, 1, 1, 0.2f);
        // 부채꼴 그리기.
        Handles.DrawSolidArc(canon.transform.position, Vector3.up, fromAnglePos, canon.ViewAngle, canon.AttackRange);
        // 시야각 표시.
        Handles.Label(canon.transform.position + (canon.transform.forward * 2.0f), canon.ViewAngle.ToString());
    }
}
