using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MenuManagerCore;


namespace MenuManagerCore;
internal static class Control
{
    public static List<PlayerInfo> menus = [];
    private static BasePlugin? hPlugin;

    public static void AddMenu(CCSPlayerController player, ButtonMenu inst)
    {
        for(int i = 0; i < menus.Count; i++)
            if (menus[i].GetPlayer() == player)
            {
                menus.Remove(menus[i]);
                i++;
            }

        var menu = new PlayerInfo(player, inst);
        player.Freeze();
        menus.Add(menu);
    }

    public static void AddMenuAll(ButtonMenu inst)
    {
        var players = Utilities.GetPlayers();
        foreach (var player in players)
        {
            if(player != null && player.IsValid && !player.IsBot && !player.IsHLTV && player.Connected == PlayerConnectedState.PlayerConnected)
                AddMenu(player, inst);
        }
    }

    public static void Clear()
    {            
        menus.RemoveAll(x => true);
    }

    public static void OnPluginTick()
    {
        if(menus.Count > 0)
        {
            //foreach(var menu in menus)
            for(int i = 0; i < menus.Count; i++)
            {
                var menu = menus[i];
                if(menu == null)
                {
                    menus.RemoveAt(i);
                    i--;
                    continue;
                }
                var player = menu.GetPlayer();
                if(!Misc.IsValidPlayer(player))
                {
                    menus.RemoveAt(i);
                    i--;
                    continue;
                }
                var buttons = player.Buttons;
                // player.PlayerPawn.Value.VelocityModifier = 0.0f;
                // For ButtonMenu
                //menu.GetPlayer().PrintToChat("Вот тебе меню .!.");
                
                if (!menu.IsEqualButtons(buttons.ToString()))
                {

                    if (buttons.HasFlag(PlayerButtons.Forward))
                        menu.MoveUp();
                    else if (buttons.HasFlag(PlayerButtons.Back))
                        menu.MoveDown();
                    else if (buttons.HasFlag(PlayerButtons.Moveleft))
                        menu.MoveUp(7);
                    else if (buttons.HasFlag(PlayerButtons.Moveright))
                        menu.MoveDown(7);
                    else if (buttons.HasFlag(PlayerButtons.Use))
                        menu.OnSelect();

                    if (buttons.HasFlag(PlayerButtons.Reload) || menu.Closed())
                    {
                        player.Unfreeze();
                        menus.RemoveAt(i);
                        i--;
                        continue;
                    }
                }

                menu.GetPlayer().PrintToCenterHtml(menu.GetText());
            }
        }
    }

    public static void CloseMenu(CCSPlayerController player)
    {
        CounterStrikeSharp.API.Modules.Menu.MenuManager.CloseActiveMenu(player);
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].GetPlayer() == player)
            {
                player.Unfreeze();
                menus[i].Close();
            }
        }            
    }
    
    internal static void Init(BasePlugin _hPlugin)
    {
        hPlugin = _hPlugin;
    }

    internal static BasePlugin? GetPlugin()
    {
        return hPlugin;
    }
}
