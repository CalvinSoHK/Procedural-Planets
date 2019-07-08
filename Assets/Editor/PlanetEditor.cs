using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for planet settings so they can be updated in inspector instead of the scriptable object.
/// </summary>
[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    /// <summary>
    /// Reference to current planet.
    /// </summary>
    Planet planet;

    /// <summary>
    /// Save editor so we don't have to remake it every frame.
    /// </summary>
    Editor shapeEditor;

    /// <summary>
    /// Save editor so we don't have to remake it every frame.
    /// </summary>
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        //Check for updates here instead of in our planet scripts.
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            //Use default gui stuff
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        //If we press this button generate planet.
        if(GUILayout.Button("Generate Planet"))
        {
            planet.GeneratePlanet();
        }

        //Simply draw both settings
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colorSettings, planet.OnColorSettingsUpdated, ref planet.colorSettingsFoldout, ref colorEditor);
    }

    /// <summary>
    /// Takes an object and draws its editor to our own window.
    /// Also takes in function to call if a change was made on those editor windows.
    /// In this case, when we edit the shape settings we want the shape to be updated real-time, same with color.
    /// NOTE: Since we want the foldout values in our Planet script to change reflecting on the title bars, they have to be
    /// passed as reference.
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="onSettingsUpdated"></param>
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldOut, ref Editor editor)
    {

        if(settings != null)
        {
            //Creates a title bar for the given object. The bool value is whether or not the titlebar should hide its contents or not.
            //Updates the foldout as well.
            foldOut = EditorGUILayout.InspectorTitlebar(foldOut, settings);

            //Check will be true if something has changed, else false.
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldOut)
                {
                    //Create editor for the given object. By default it will render the best possible match.
                    //Only makes a new editor when it has to.
                    CreateCachedEditor(settings, null, ref editor);

                    //Calls the default GUI. if this was overridden it will call that editor window instead.
                    editor.OnInspectorGUI();

                    //If something has changed in this editor
                    if (check.changed)
                    {
                        //And we have a viable function call, call that function
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }  
    }

    /// <summary>
    /// Assign planet from target
    /// </summary>
    private void OnEnable()
    {
        planet = (Planet)target;
    }
}
