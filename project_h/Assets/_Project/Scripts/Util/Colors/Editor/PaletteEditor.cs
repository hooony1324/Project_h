using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 자주 사용하는 색상 선택
/// </summary>
[CustomEditor(typeof(Image))]
[CanEditMultipleObjects] 
public class ImageEditor : Editor
{
    private ColorPalette colorPalette; // ColorPalette 참조

    public static Texture2D CreateColorTexture(Color color)
    {
        // 1x1 크기의 텍스쳐 생성
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply(); // 텍스쳐 업데이트
        return texture;
    }

    private void OnEnable()
    {
        // ColorPalette 초기화
        if (colorPalette == null)
            colorPalette = Resources.Load<ColorPalette>("ProjectColorPalette");
    }

    public override void OnInspectorGUI()
    {
        // 기본 Inspector UI 그리기
        base.OnInspectorGUI();

        // ColorPalette 필드 생성
        colorPalette = (ColorPalette)EditorGUILayout.ObjectField("Color Palette", colorPalette, typeof(ColorPalette), false);

        // ColorPalette가 설정되어 있을 때만 색상 버튼 그리기
        if (colorPalette != null && colorPalette.colors.Length > 0)
        {
            GUILayout.Label("Select a Color:");

            // Inspector의 넓이 계산
            float inspectorWidth = EditorGUIUtility.currentViewWidth;
            int columns = Mathf.FloorToInt(inspectorWidth / 55); // 버튼의 너비를 55로 설정, 열 수 계산

            for (int i = 0; i < colorPalette.colors.Length; i++)
            {
                if (i % columns == 0)
                {
                    GUILayout.BeginHorizontal(); // 새로운 행 시작
                }

                // 버튼의 배경색을 설정
                Color buttonColor = colorPalette.colors[i].color;

                // 새로운 GUIStyle 생성
                GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
                {
                    normal = { background = CreateColorTexture(buttonColor) },
                    fixedWidth = 50,
                    fixedHeight = 50
                };

                // 색상 버튼 생성
                if (GUILayout.Button("", buttonStyle))
                {
                    // 선택된 모든 Image 오브젝트에 색상 적용
                    foreach (Object targetObject in targets)
                    {
                        Image img = (Image)targetObject;
                        img.color = buttonColor; // 색상 적용
                        EditorUtility.SetDirty(img); // 변경 사항을 반영
                    }
                }

                if (i % columns == columns - 1 || i == colorPalette.colors.Length - 1)
                {
                    GUILayout.EndHorizontal(); // 행 종료
                }
            }
        }
    }
}

// [CustomEditor(typeof(SpriteRenderer))]
// [CanEditMultipleObjects] // 다중 오브젝트 편집 가능
// public class SpriteRendererEditor : Editor
// {
//     private ColorPalette colorPalette; // ColorPalette 참조
//     private void OnEnable()
//     {
//         // ColorPalette 초기화
//         if (colorPalette == null)
//             colorPalette = Resources.Load<ColorPalette>("ProjectColorPalette");
//     }
//     public static Texture2D CreateColorTexture(Color color)
//     {
//         // 1x1 크기의 텍스쳐 생성
//         Texture2D texture = new Texture2D(1, 1);
//         texture.SetPixel(0, 0, color);
//         texture.Apply(); // 텍스쳐 업데이트
//         return texture;
//     }

//     public override void OnInspectorGUI()
//     {
//         // 기본 Inspector UI 그리기
//         base.OnInspectorGUI();

//         // ColorPalette 필드 생성
//         colorPalette = (ColorPalette)EditorGUILayout.ObjectField("Color Palette", colorPalette, typeof(ColorPalette), false);

//         // ColorPalette가 설정되어 있을 때만 색상 버튼 그리기
//         if (colorPalette != null && colorPalette.colors.Length > 0)
//         {
//             GUILayout.Label("Select a Color:");

//             // Inspector의 넓이 계산
//             float inspectorWidth = EditorGUIUtility.currentViewWidth;
//             int columns = Mathf.FloorToInt(inspectorWidth / 55); // 버튼의 너비를 55로 설정, 열 수 계산

//             for (int i = 0; i < colorPalette.colors.Length; i++)
//             {
//                 if (i % columns == 0)
//                 {
//                     GUILayout.BeginHorizontal(); // 새로운 행 시작
//                 }

//                 // 버튼의 배경색을 설정
//                 Color buttonColor = colorPalette.colors[i].color;

//                 // 새로운 GUIStyle 생성
//                 GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
//                 {
//                     normal = { background = CreateColorTexture(buttonColor) },
//                     fixedWidth = 50,
//                     fixedHeight = 50
//                 };

//                 // 색상 버튼 생성
//                 if (GUILayout.Button("", buttonStyle))
//                 {
//                     // 선택된 모든 SpriteRenderer 오브젝트에 색상 적용
//                     foreach (Object targetObject in targets)
//                     {
//                         SpriteRenderer sr = (SpriteRenderer)targetObject;
//                         sr.color = buttonColor; // 색상 적용
//                         EditorUtility.SetDirty(sr); // 변경 사항을 반영
//                     }
//                 }

//                 if (i % columns == columns - 1 || i == colorPalette.colors.Length - 1)
//                 {
//                     GUILayout.EndHorizontal(); // 행 종료
//                 }
//             }
//         }
//     }
// }
