using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using MenuManager;

namespace MenuManagerCore;

public class MenuInstance(string _title, Action<CCSPlayerController> _back_action, MenuType _forcetype = MenuType.Default) : IMenu
{
    readonly Action<CCSPlayerController> BackAction = _back_action;

    MenuType forcetype = _forcetype;

    public string Title { get; set; } = _title;

    public List<ChatMenuOption> MenuOptions { get; } = [];

    public bool ExitButton { get; set; } = true;

    public PostSelectAction PostSelectAction { get; set; } = PostSelectAction.Nothing;

    public ChatMenuOption AddMenuOption(string display, Action<CCSPlayerController, ChatMenuOption> onSelect, bool disabled = false)
    {
        var option = new ChatMenuOption(display, disabled, (player, opt) => { onSelect(player, opt); });
        MenuOptions.Add(option);
        return option;            
    }

    private void OnBackAction(CCSPlayerController player, ChatMenuOption option)
    {
        BackAction(player);
    }

    public void Open(CCSPlayerController player)
    {
        if (forcetype == MenuType.Default)
            forcetype = Misc.GetCurrentPlayerMenu(player);

        IMenu menu = forcetype switch
        {
            MenuType.ChatMenu => new ChatMenu(Title),
            MenuType.ConsoleMenu => new ConsoleMenu(Title),
            MenuType.CenterMenu => new CenterHtmlMenu(Title, Control.GetPlugin() ?? throw new Exception("Plugin is not loaded")),
            MenuType.ButtonMenu => new ButtonMenu(Title),
            _ => new ChatMenu(Title),
        };

        menu.ExitButton = ExitButton;
        menu.PostSelectAction = PostSelectAction;            

        if (BackAction != null)
            menu.AddMenuOption(Control.GetPlugin()?.Localizer["menumanager.back"] ?? "Back", OnBackAction);

        foreach(var option in MenuOptions)
            menu.AddMenuOption(option.Text, option.OnSelect, option.Disabled);

        //Control.AddMenu(player, this, forcetype);
        menu.Open(player);
    }

    public void OpenToAll()
    {
        foreach (var player in Misc.GetValidPlayers())
            Open(player);
    }
}
