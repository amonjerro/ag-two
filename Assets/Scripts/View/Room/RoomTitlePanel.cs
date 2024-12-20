using TMPro;

public class RoomTitlePanel : UIPanel
{
    public TextMeshProUGUI titleText;

    private void OnEnable()
    {
        Show();
    }

    public override void Show()
    {
        RoomInterface roomInterface = GetComponentInParent<RoomInterface>();
        titleText.text = roomInterface.GetRoomTitle();
    }

    public override void Dismiss()
    {
        
    }
}