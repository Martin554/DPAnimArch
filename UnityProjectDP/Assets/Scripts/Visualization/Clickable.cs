using System;
using System.Collections;
using Networking;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { };
public class Clickable : NetworkBehaviour
{
    private GameObjectEvent _triggerHighlightAction;
    private GameObjectEvent _triggerUnhighlightAction;
    private Vector3 _screenPoint;
    private Vector3 _offset;

    private Outline _outline;
    private readonly Color _transparentColor = new Color(0, 0, 0, 0);
    private bool _selectedElement = false;

    private void Start()
    {
        _outline = gameObject.transform.Find("Background").GetComponent<Outline>();
    }

    private void OnMouseDown()
    {
        if (IsHost)
        {
            var color = ClassDiagramView.Instance.SelectedClassColor;
            ClassSelectedClientRpc(color);
            _outline.effectColor = color;
            
        }

        if (IsClient && !IsServer)
        {
            var color = ClassDiagramView.Instance.SelectedClassColor;
            ClassSelectedServerRpc(color);
            _outline.effectColor = color;
        }

        if (ToolManager.Instance.SelectedTool == "DiagramMovement")
        {
            OnClassSelected();
        }
    }

    private void OnClassSelected()
    {
        _selectedElement = true;
        _screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        ClassEditor.Instance.SelectNode(this.gameObject);

        _offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
    }

    void OnMouseUp()
    {
        _selectedElement = false;
        _outline.effectColor = _transparentColor;
        if (IsHost)
        {
            ClassReleasedClientRpc();
        }
        if (IsClient && !IsServer)
        {
            ClassReleasedServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateClassPositionServerRpc(Vector3 position)
    {
        transform.position = position;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClassSelectedServerRpc(Color color)
    {
        _outline.effectColor = color;
    }

    [ClientRpc]
    public void ClassSelectedClientRpc(Color color)
    {
        _outline.effectColor = color;
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClassReleasedServerRpc()
    {
        _outline.effectColor = _transparentColor;
        _selectedElement = false;
    }

    [ClientRpc]
    public void ClassReleasedClientRpc()
    {
        _outline.effectColor = _transparentColor;
        _selectedElement = false;
    }

    void OnMouseDrag()
    {
        if (_selectedElement == false || ToolManager.Instance.SelectedTool != "DiagramMovement" || IsMouseOverUi())
            return;

        var cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
        var cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + _offset;
        cursorPosition.z = transform.position.z;
        if (IsHost)
        {
            transform.position = cursorPosition;
        }
        if (IsClient && !IsServer)
        {
            UpdateClassPositionServerRpc(cursorPosition);
        }
    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && ToolManager.Instance.SelectedTool == "Highlighter" && !IsMouseOverUi())
        {
            _triggerHighlightAction.Invoke(gameObject);
        }
        if (Input.GetMouseButtonDown(1) && ToolManager.Instance.SelectedTool == "Highlighter" && !IsMouseOverUi())
        {
            _triggerUnhighlightAction.Invoke(gameObject);
        }
        if (Input.GetMouseButtonDown(0)&&MenuManager.Instance.isCreating == true)
        {
            MenuManager.Instance.SelectClass(this.gameObject.name);
        }
        if (Input.GetMouseButtonDown(0) && MenuManager.Instance.isPlaying == true)
        {
            MenuManager.Instance.SelectPlayClass(this.gameObject.name);
            Debug.Log("selecting class");
        }
    }
    private bool IsMouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}