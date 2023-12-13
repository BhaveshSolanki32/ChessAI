using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
public class InputTaskHandler : MonoBehaviour //forwards the input data to respective script
{
    [SerializeField] GameObject _piecesParent;
    [SerializeField] GridData _gridData;
    SelectPiece _selectPiece;
    MovePiece _movePiece;
    GameObject _selectedGameObject;


    private void Awake()
    {
        if (!_piecesParent.TryGetComponent<SelectPiece>(out _selectPiece)) Debug.LogError("SelectPiece not found", _piecesParent);
        if (!_piecesParent.TryGetComponent<MovePiece>(out _movePiece)) Debug.LogError("MovePiece not found", _piecesParent);

    }


    public void NewInputRecieved(GameObject newSelectedGameObject)
    {
        _selectPiece.Select(newSelectedGameObject);
        _selectedGameObject = newSelectedGameObject;
    }

    public void NewInputRecieved(Vector3 mousePost)
    {
        if (_selectedGameObject == null) return;

        var post = WorldToGridPostion.Convert(mousePost, _gridData);

        if (_gridData.TileExists(post))
        {
            _movePiece.MovePieceTo(_selectedGameObject, post);
        }
    }

}
