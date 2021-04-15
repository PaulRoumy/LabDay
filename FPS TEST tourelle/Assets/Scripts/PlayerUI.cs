
using UnityEngine;
using UnityEngine.UI;
public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuelFill;

    [SerializeField]
    private RectTransform heathBarFill;
    private PlayerController controller;
    [SerializeField]
    private GameObject pauseMenu;
    private Player player;
    [SerializeField]
    private Text ammoText; 
    private WeaponManager weaponManager;



    public void SetPlayer (Player _player)
    {
        player =_player;
        controller = _player.GetComponent<PlayerController>();
        weaponManager =_player.GetComponent<WeaponManager>();
    }

    private void Update() {
        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHeathAmount(player.GetHeathPct());
        SetAmmoAmount(weaponManager.getCurrentWeapon().ammo);
        if(Input.GetKeyDown(KeyCode.Escape)){
            ToggelPauseMenu();
        
    }
        void SetFuelAmount( float _amount)
        {
            thrusterFuelFill.localScale = new Vector3(1f,_amount,1f);
        }

        void SetHeathAmount(float _amount)
        {
            heathBarFill.localScale = new Vector3(1f,_amount,1f);
        }

    
}

    public void ToggelPauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void SetAmmoAmount(int _amount){
        ammoText.text = _amount.ToString();
    }
}
