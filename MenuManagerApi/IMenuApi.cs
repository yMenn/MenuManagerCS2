using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;

namespace MenuManagerApi;

public interface IMenuApi
{
    public IMenu NewMenu(string title, Action<CCSPlayerController> back_action = null);
    public IMenu NewMenuForcetype(string title, MenuType type, Action<CCSPlayerController> back_action = null);
    public void CloseMenu(CCSPlayerController player); 
    public MenuType GetMenuType(CCSPlayerController player);
}

public enum MenuType
{
    Default = -1,
    ChatMenu = 0,
    ConsoleMenu = 1,
    CenterMenu = 2,
    ButtonMenu = 3
}
