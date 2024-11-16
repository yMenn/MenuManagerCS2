using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace MenuManagerCore;

public class ButtonMenu(string _title) : IMenu
{
    public string Title { get; set; } = _title;

    public List<ChatMenuOption> MenuOptions { get; } = [];

    public bool ExitButton { get; set; }

    public PostSelectAction PostSelectAction { get; set; } = PostSelectAction.Nothing;

    public ChatMenuOption AddMenuOption(string display, Action<CCSPlayerController, ChatMenuOption> onSelect, bool disabled = false)
    {
        var option = new ChatMenuOption(display, disabled, onSelect);
        MenuOptions.Add(option);            
        return option;            
    }

    public void Open(CCSPlayerController player)
    {
        Control.AddMenu(player, this);
    }

    public void OpenToAll()
    {
        Control.AddMenuAll(this);
    }
}
