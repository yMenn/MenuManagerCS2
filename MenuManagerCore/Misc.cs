using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerSettings;
using MenuManager;


namespace MenuManagerCore;

internal static class Misc
{

    private static ISettingsApi? settings;
    
    public static void SetSettingApi(ISettingsApi _settings)
    {
        settings = _settings;
    }

    public static List<CCSPlayerController> GetValidPlayers()
    {
        var players = new List<CCSPlayerController>();
        foreach (var player in Utilities.GetPlayers())
        {
            if (player != null && player.IsValid && !player.IsBot && !player.IsHLTV && player.Connected == PlayerConnectedState.PlayerConnected)
                players.Add(player);
        }

        return players;
    }

    public static MenuType GetCurrentPlayerMenu(CCSPlayerController player)
    {         
        var res = settings?.GetPlayerSettingsValue(player, "menutype", "ButtonMenu");
        if (res == null) return MenuType.ButtonMenu;
        return (MenuType)Enum.Parse(typeof(MenuType), res);
    }

    public static void SelectPlayerMenu(CCSPlayerController player, MenuType type)
    {
        var type_name = Enum.GetName(type.GetType(), type) ?? "ButtonMenu";
        settings?.SetPlayerSettingsValue(player, "menutype", type_name);

        player.PrintToChat($"{Control.GetPlugin()?.Localizer["menumanager.selected_type"] ?? "Selected type"} {GetMenuTypeName(type)}");
    }

    public static string GetMenuTypeName(MenuType type)
    {
        switch(type)
        {
            case MenuType.ChatMenu: return Control.GetPlugin()?.Localizer["menumanager.chat"] ?? "Chat";
            case MenuType.ConsoleMenu: return Control.GetPlugin()?.Localizer["menumanager.console"] ?? "Console";
            case MenuType.CenterMenu: return Control.GetPlugin()?.Localizer["menumanager.center"] ?? "Center";
            case MenuType.ButtonMenu: return Control.GetPlugin()?.Localizer["menumanager.control"] ?? "Control";
            default: return "Undefined";
        }
    }

    public static bool IsValidPlayer(CCSPlayerController player)
    {
        if (player.IsValid && player.Connected == PlayerConnectedState.PlayerConnected && !player.IsBot) return true;
        else return false;
    }
}
