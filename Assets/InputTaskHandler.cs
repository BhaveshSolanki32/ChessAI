using UnityEngine;

[RequireComponent(typeof(InputReceiver))]
public class InputTaskHandler : MonoBehaviour //forwards the input data to respective script
{
    [SerializeField] GameObject PiecesParent;
    [SerializeField] GridData gridData;
    SelectPiece selectPiece;
    MovePiece movePiece;
    GameObject selectedGameObject;


    private void Awake()
    {
        if (!PiecesParent.TryGetComponent<SelectPiece>(out selectPiece)) Debug.LogError("SelectPiece not found", PiecesParent);
        if (!PiecesParent.TryGetComponent<MovePiece>(out movePiece)) Debug.LogError("MovePiece not found", PiecesParent);

    }


    public void NewInputRecieved(GameObject _newSelectedGameObject)
    {
        selectPiece.Select(_newSelectedGameObject);
        selectedGameObject = _newSelectedGameObject;
    }

    public void NewInputRecieved(Vector3 _mousePost)
    {
        if (selectedGameObject == null) return;

        Vector2Int _post = WorldToGridPostion.Convert(_mousePost, gridData);

        if (gridData.TileExists(_post))
        {
            movePiece.MovePieceTo(selectedGameObject, _post);
        }
    }

}
