using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TweenAnimator))]
public class TweenAnimatorEditor : Editor
{
    static readonly List<bool> showLayout = new List<bool>();

    public override void OnInspectorGUI()
    {

        EditorGUI.BeginChangeCheck();
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.fontStyle = FontStyle.Normal;
        TweenAnimator _tweenAnimator = (TweenAnimator) target;
        Undo.RecordObject(_tweenAnimator, $"Tween changed for {_tweenAnimator.name}");

        if (_tweenAnimator.data == null)
        {
            _tweenAnimator.data = new List<TweenData>();
        }

        if (_tweenAnimator.customBeginEvents == null)
        {
            _tweenAnimator.customBeginEvents = new List<UnityEvent>();
        }
        
        if (_tweenAnimator.customCompleteEvents == null)
        {
            _tweenAnimator.customCompleteEvents = new List<UnityEvent>();
        }
        
        for (int _i = 0; _i < _tweenAnimator.data.Count; _i++)
        {
            if (showLayout.Count <= _i)
            {
                showLayout.Add(false);
            }

            EditorGUILayout.Separator();
            EditorGUILayout.BeginVertical();
            
            GUIStyle _myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            Color _myStyleColor = Color.green;
            _myFoldoutStyle.fontStyle = FontStyle.Bold;
            _myFoldoutStyle.fontSize = 14;
            _myFoldoutStyle.normal.textColor = _myStyleColor;
            _myFoldoutStyle.onNormal.textColor = _myStyleColor;
            _myFoldoutStyle.hover.textColor = _myStyleColor;
            _myFoldoutStyle.onHover.textColor = _myStyleColor;
            _myFoldoutStyle.focused.textColor = _myStyleColor;
            _myFoldoutStyle.onFocused.textColor = _myStyleColor;
            _myFoldoutStyle.active.textColor = _myStyleColor;
            _myFoldoutStyle.onActive.textColor = _myStyleColor;

            showLayout[_i] = EditorGUILayout.Foldout(showLayout[_i], $"Tween data #{_i.ToString()}", _myFoldoutStyle);
            while (_tweenAnimator.customBeginEvents.Count <= _i)
            {
                _tweenAnimator.customBeginEvents.Add(new UnityEvent());
            }

            while (_tweenAnimator.customCompleteEvents.Count <= _i)
            {
                _tweenAnimator.customCompleteEvents.Add(new UnityEvent());
            }

            SerializedProperty _customBeginEventsProperty = serializedObject.FindProperty("customBeginEvents");
            SerializedProperty _customCompleteEvents = serializedObject.FindProperty("customCompleteEvents");
            
            if (showLayout[_i])
            {
                GUI.skin.label.fontSize = 16;
                GUI.skin.label.fontStyle = FontStyle.Bold;
              
                _tweenAnimator.data[_i].performBeginEvent = 
                    GUILayout.Toggle(_tweenAnimator.data[_i].performBeginEvent, "Perform begin event");

                if (_tweenAnimator.data[_i].performBeginEvent)
                {
                    GUILayout.Label("Tween begin events");
              
                    GUI.skin.label.fontSize = 10;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
              
                    EditorGUILayout.PropertyField(_customBeginEventsProperty.GetArrayElementAtIndex(_i), 
                        new GUIContent($"Custom begin event #{_i.ToString()}"));
              
                    GUI.skin.label.fontSize = 16;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                }
              
                _tweenAnimator.data[_i].performCompleteEvent = 
                    GUILayout.Toggle(_tweenAnimator.data[_i].performCompleteEvent, "Perform complete event");

                if (_tweenAnimator.data[_i].performCompleteEvent)
                {
                    GUILayout.Label("Tween complete events");
              
                    GUI.skin.label.fontSize = 10;
                    GUI.skin.label.fontStyle = FontStyle.Normal;
              
                    EditorGUILayout.PropertyField(_customCompleteEvents.GetArrayElementAtIndex(_i), 
                        new GUIContent($"Custom complete event #{_i.ToString()}"));
              
                    GUI.skin.label.fontSize = 16;
                    GUI.skin.label.fontStyle = FontStyle.Bold;
                }

                GUILayout.Label("Actions");

                _tweenAnimator.data[_i].move = GUILayout.Toggle(_tweenAnimator.data[_i].move, "Move");
              
                if (_tweenAnimator.data[_i].move)
                {
                    _tweenAnimator.data[_i].position = EditorGUILayout.Vector3Field("Position: ", 
                        _tweenAnimator.data[_i].position);
                }
            
                _tweenAnimator.data[_i].rotate = GUILayout.Toggle(_tweenAnimator.data[_i].rotate, "Rotate");
              
                if (_tweenAnimator.data[_i].rotate)
                {
                    _tweenAnimator.data[_i].rotation = EditorGUILayout.Vector3Field("Rotation: ", 
                        _tweenAnimator.data[_i].rotation);
                }
              
                _tweenAnimator.data[_i].changeScale = GUILayout.Toggle(_tweenAnimator.data[_i].changeScale, 
                    "Change scale");
              
                if (_tweenAnimator.data[_i].changeScale)
                {
                    _tweenAnimator.data[_i].scale = EditorGUILayout.Vector3Field("Scale: ", 
                        _tweenAnimator.data[_i].scale);
                }
              
                _tweenAnimator.data[_i].changeColor = GUILayout.Toggle(_tweenAnimator.data[_i].changeColor, 
                    "Change color");
              
                if (_tweenAnimator.data[_i].changeColor)
                {
                    _tweenAnimator.data[_i].color = EditorGUILayout.ColorField("Color: ", 
                        _tweenAnimator.data[_i].color);
                }
                
                _tweenAnimator.data[_i].changeAlpha = GUILayout.Toggle(_tweenAnimator.data[_i].changeAlpha, 
                    "Change alpha");

                if (_tweenAnimator.data[_i].changeAlpha)
                {
                    _tweenAnimator.data[_i].alpha = EditorGUILayout.Slider("Alpha: ", 
                        _tweenAnimator.data[_i].alpha, 0f, 1f);
                }
              
                EditorGUILayout.Separator();
              
                GUILayout.Label("Settings");
                
                _tweenAnimator.data[_i].doNotWaitForComplete = GUILayout.Toggle(_tweenAnimator.data[_i].doNotWaitForComplete, 
                    "Do not wait for complete");
                
                _tweenAnimator.data[_i].performOnEnable = GUILayout.Toggle(_tweenAnimator.data[_i].performOnEnable, 
                    "Perform on enable");
              
                _tweenAnimator.data[_i].duration = EditorGUILayout.Slider("Duration: ", 
                    _tweenAnimator.data[_i].duration, 0f, 60f);
              
                _tweenAnimator.data[_i].delay = EditorGUILayout.Slider("Delay: ", 
                    _tweenAnimator.data[_i].delay, 0f, 120f);
              
                _tweenAnimator.data[_i].loopCount = EditorGUILayout.IntField("Loop count:", 
                    _tweenAnimator.data[_i].loopCount);
              
                _tweenAnimator.data[_i].easeType = (LeanTweenType)EditorGUILayout.EnumPopup("Easing method:", 
                    _tweenAnimator.data[_i].easeType);

                if (GUILayout.Button("Collect current data"))
                {
                    _tweenAnimator.data[_i] = _tweenAnimator.CollectCurrentData(_i);
                }
                
                if (GUILayout.Button("Apply current data"))
                {
                    _tweenAnimator.ApplyCurrentData(_i);
                }
                
                if (GUILayout.Button("Perform tween"))
                {
                    _tweenAnimator.PerformTween(_i);
                }
            }
            EditorGUILayout.EndVertical();
        }
        
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        
        if (GUILayout.Button("Add new entry"))
        {
            _tweenAnimator.data.Add(_tweenAnimator.data.Count == 0 ? _tweenAnimator.CollectCurrentData(0) : new TweenData());
            _tweenAnimator.customBeginEvents.Add(new UnityEvent());
            _tweenAnimator.customCompleteEvents.Add(new UnityEvent());
        }

        if (_tweenAnimator.data.Count > 0)
        {
            if (GUILayout.Button("Remove last entry"))
            {
                _tweenAnimator.data.RemoveAt(_tweenAnimator.data.Count - 1);
            }
        }
        
        if(GUI.changed && !EditorApplication.isPlaying)
        {
            EditorUtility.SetDirty(_tweenAnimator);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }

        EditorGUI.EndChangeCheck();
        serializedObject.ApplyModifiedProperties();
    }
}
