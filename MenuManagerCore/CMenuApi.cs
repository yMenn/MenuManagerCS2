using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using MenuManager;

namespace MenuManagerCore;

internal class CMenuApi(MenuManagerCore _plugin) : IMenuApi
{
    readonly MenuManagerCore plugin = _plugin;

    public IMenu NewMenu(string title, Action<CCSPlayerController> back_action)
    {
        return new MenuInstance(title, back_action);
    }

    public IMenu NewMenuForcetype(string title, MenuType type, Action<CCSPlayerController> back_action)
    {
        return new MenuInstance(title, back_action, type);
    }

    public void CloseMenu(CCSPlayerController player)
    {
        Control.CloseMenu(player);
    }

    public MenuType GetMenuType(CCSPlayerController player)
    {
        return Misc.GetCurrentPlayerMenu(player);
    }
    
}
