using Assets.Scripts;
using Assets.Scripts.Backend.Data;
using Assets.Scripts.Enems;
using Assets.Scripts.InterFaces;
using UnityEngine;
using UnityEngine.UI;

public class UnitDeployButton : MonoBehaviour, IImgeSwichable<UnitType>
{
    [SerializeField] private UnitType _unitType;

    private UnitData _unit;

    private Sprite _sprite;

    private Image _image;

    public UnitType Type => _unitType;

    public Sprite Sprite => _sprite;
    public Image Image => _image;
    public void SetSprite(Sprite sprite)
    {
       _image.sprite = sprite;
    }
    private void Start()
    {
        GetData();
    }
    private void GetData()
    {
        _unit = GameStateManager.Instance.GetFriendlyUnit(_unitType);
        _sprite = GameStateManager.Instance.GetUnitSprite(_unitType);
        _image = GetComponent<Image>();
        _image.sprite = _sprite;
    }



    public void DeployUnit()
    {
        if (PlayerCurrency.Instance.HasEnoughMoney(_unit.Cost))
        {
            PlayerCurrency.Instance.SubtractMoney(_unit.Cost);

            if (GameManager.Instance != null && DeployManager.Instance != null)
            {
                DeployManager.Instance.AddUnitToDeploymentQueue(_unit);
            }
            else
            {
                Debug.LogWarning("DeployUnit called before GameManager or DeployManager were initialized.");
            }
        }


    }
}
