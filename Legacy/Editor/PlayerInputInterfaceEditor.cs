using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using Rewired;

//don't warn us about obsolete functions in here
//#pragma warning disable 0618

namespace nv
{

    [CustomEditor( typeof( PlayerInputInterface ) )]
    public class PlayerInputInterfaceEditor : Editor
    {
    public static List<MethodInfo> gameAvatarMethods;

        string InputAssetPath {
            get {
                return "/Framework/Input/Asset/PlayerInputActions.dat";
            }
        }

        List<string> _input_action_strings;

        PlayerInputInterface _target;

        ReorderableList _action_bindings;

        void SetupActionList()
        {
            if( GameObject.FindObjectOfType<Rewired.InputManager>() == null )
            {
                Dev.LogError( "No ReWired input manager found!" );
                return;
            }

            _input_action_strings = GameObject.FindObjectOfType<Rewired.InputManager>().userData.GetActionNames().ToList();

            _action_bindings = new ReorderableList( _target.INTERNAL_inputBindings, typeof( InputPair ), true, true, true, true );

            _action_bindings.drawHeaderCallback = ( Rect rect ) =>
            {
                EditorGUI.LabelField( rect, "Player Input Actions" );
            };

            _action_bindings.drawElementCallback =
            ( Rect rect, int index, bool isActive, bool isFocused ) =>
            {
                rect.y += 2;

                if( _input_action_strings == null )
                    return;

                int choice_index = _target.INTERNAL_inputBindings[ index]._rw_action_index;

                int dropdown_field_size = 120;
                int enum_field_size = 40;
                int object_field_size = 60;


            //choose the action string
            choice_index = EditorGUI.Popup(
                    new Rect( rect.x, rect.y, dropdown_field_size, EditorGUIUtility.singleLineHeight )
                    , choice_index, _input_action_strings.ToArray() );

                _target.INTERNAL_inputBindings[ index ]._rw_action_index = choice_index;
                _target.INTERNAL_inputBindings[ index ]._rw_action = _input_action_strings[ choice_index ];

            //set the type of button press here (pressed? held?)
            _target.INTERNAL_inputBindings[ index ]._action_type = (InputActionEventType)EditorGUI.EnumPopup(
                    new Rect( rect.x + dropdown_field_size, rect.y, enum_field_size, EditorGUIUtility.singleLineHeight )
                    , _target.INTERNAL_inputBindings[ index ]._action_type );

            //place here an object with an input action container
            InputAction selection = null;

                //if( _target.INTERNAL_inputBindings[ index ]._action_obj != null )
                    selection = _target.INTERNAL_inputBindings[ index ]._action;

                EditorGUI.LabelField(
                    new Rect( rect.x + dropdown_field_size + enum_field_size, rect.y, object_field_size, EditorGUIUtility.singleLineHeight )
                    , selection.name );

                choice_index = _target.INTERNAL_inputBindings[ index ]._action_index;
                

                if(gameAvatarMethods == null)
                {
                    //Dev.LogVarArray( "mth", typeof(GameAvatar).GetMethods().Select( x => x.Name ).ToList() );
                    gameAvatarMethods = typeof( GameAvatar ).GetMethods().Where( x => x.Name.Contains( "Handle" ) ).ToList();
                }

                List<string> ia_choices = gameAvatarMethods.Select(x=>x.Name).ToList();// _target.INTERNAL_inputBindings[ index ]._action_obj.inputActions.Select( s => s.GetActionTypeName() ).ToList();

                //attempt to find the action from our list of choices automatically by searching for a string partial match
                bool found_ia = false;
                int i = 0;
                if( _target.INTERNAL_inputBindings[ index ]._rw_action != string.Empty )
                {
                    for( ; i < ia_choices.Count; ++i )
                    {
                        if( ia_choices[ i ].Contains( _target.INTERNAL_inputBindings[ index ]._rw_action ) )
                        {
                            found_ia = true;
                            break;
                        }
                    }
                }

                if( found_ia )
                {
                    choice_index = i;
                }

                choice_index = EditorGUI.Popup(
                    new Rect( rect.x + dropdown_field_size + enum_field_size + object_field_size, rect.y, rect.width - dropdown_field_size - enum_field_size - object_field_size, EditorGUIUtility.singleLineHeight )
                    , choice_index, ia_choices.ToArray() );

                _target.INTERNAL_inputBindings[ index ]._action_index = choice_index;


                if( _target.INTERNAL_inputBindings[ index ]._action.name != _input_action_strings[ Mathf.Min( _input_action_strings.Count-1, choice_index ) ] )
                {
                    InputAction newAction = new InputAction();

                    newAction.name = _input_action_strings[ _target.INTERNAL_inputBindings[ index ]._rw_action_index ];
                    newAction.methodName = gameAvatarMethods[ choice_index ].Name;

                    Action<InputActionEventData> doAction = (InputActionEventData x) =>
                    {
                        if(newAction.inputInterface == null)
                            return;

                        if(newAction.inputInterface.inputTarget == null)
                            return;


                        var gameAction = newAction.inputInterface.inputTarget.GetType().GetMethod( gameAvatarMethods[choice_index].Name );

                        gameAction.Invoke(newAction.inputInterface.inputTarget, new object[]{ x } );
                    };

                    newAction.DoAction = doAction;

                    _target.INTERNAL_inputBindings[ index ]._action = newAction;

                }
            };

        }

        void OnEnable()
        {
            _target = (PlayerInputInterface)target;

            SetupActionList();
        }

        public override void OnInspectorGUI()
        {
            _target = (PlayerInputInterface)target;

            if( _action_bindings == null )
                SetupActionList();

            // Draw the default inspector
            DrawDefaultInspector();

            //GUILayout.Label( "DataPath: " + Application.dataPath + InputAssetPath );

            //GUILayout.Label( "Press this in play mode" );
            //if( GUILayout.Button( "Generate Action List" ) )
            //{
            //    GenerateActions();
            //}

            //GUILayout.Label( "Press this in edit mode (if not done automatically)" );
            //if( GUILayout.Button( "Load Action File" ) )
            //{
            //    LoadActionsFile();
            //}

            serializedObject.Update();
            _action_bindings.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            // Save the changes back to the object
            EditorUtility.SetDirty( target );
        }

    }
}