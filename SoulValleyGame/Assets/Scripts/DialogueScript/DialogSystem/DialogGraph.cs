using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class DialogGraph : EditorWindow
{
    public DialogueGraphView dialogueGraphView;

    private DialogueGraphView _graphView;
    private string _fileName = "New Narrative";
    [MenuItem("Graph/Dialogue Graph")]
   public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMiniMap();
        GenerateBlackBoard();
    }

    private void GenerateBlackBoard()
    {
        var blackBoard = new Blackboard(_graphView);
        blackBoard.Add(new BlackboardSection { title = "Exposed Propertise" });
        blackBoard.addItemRequested = _blackBoard =>
        {
            _graphView.AddPropertyToBlackboard(new ExposedPropertise());
        };
        blackBoard.editTextRequested = (blackboard1, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if (_graphView.ExposedPropertises.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exist, please chose another one!!!", "OK");
                return;
            }

            var propertyIndex = _graphView.ExposedPropertises.FindIndex(x => x.PropertyName == oldPropertyName);
            _graphView.ExposedPropertises[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;

        };
        blackBoard.SetPosition(new Rect(10, 30, 200, 300));

        _graphView.Add(blackBoard);
        _graphView.Blackboard = blackBoard;

    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap {anchored = true };
        var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 30));
        miniMap.SetPosition(new Rect(800, 30, 200, 140)); //set cung không biết làm sao chuyển
        _graphView.Add(miniMap);
        Debug.Log("MiniMap was created");
        
            
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    public void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField("File Name: ");
        fileNameTextField.SetValueWithoutNotify(_fileName);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) {text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });
        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
            return;
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_fileName);
        }
        else
        {
            saveUtility.LoadGraph(_fileName);
        }
    }

    private void OnDisable()
    {
        if (_graphView != null)
        {
            rootVisualElement.Remove(_graphView);
        }
    }  
}
