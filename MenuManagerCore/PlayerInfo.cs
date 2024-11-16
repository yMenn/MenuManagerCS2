﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using Microsoft.Extensions.Logging;

namespace MenuManagerCore;

internal class PlayerInfo
{
    CCSPlayerController player;
    public ButtonMenu menu;

    private int offset;
    private int selected;
    private string prev_buttons;
    private bool closed;

    public PlayerInfo(CCSPlayerController _player, ButtonMenu _menu)
    {
        player = _player;
        menu = _menu;

        closed = false;
        offset = 0;
        selected = 0;
        prev_buttons = player.Buttons.ToString();
    }

    public CCSPlayerController GetPlayer()
    {
        return player;
    }           

    public string GetText()
    {
        string text = $"<font class='mono-spaced-font'>{menu.Title}</font><font class='fontSize-sm stratum-font'>";
        
        if(menu.MenuOptions.Count > 0)            
            for (int i = offset; i < Math.Min(offset + 7, menu.MenuOptions.Count); i++)
            {
                var line = menu.MenuOptions[i].Text;
                if (menu.MenuOptions[i].Disabled) line = $"<font color='#aaaaaa'>{line}</font>";
                if (i == selected) line = $"► {line} ◄";

                text = text + "<br>" + line;
            }
        else
            text = $"{text}<br><font color='#aaaaaa'>{Control.GetPlugin()?.Localizer["menumanager.empty"] ?? "Empty"}</font>";
        //text = text + "<br>" + "W - вверх D - вниз<br>E - выбор R - выход";

        return text + $"</font><br><font class='fontSize-s'>{Control.GetPlugin()?.Localizer["menumanager.footer"] ?? "Footer"}</font>";
    }

    public bool MoveDown(int lines = 1)
    {
        if (selected == menu.MenuOptions.Count - 1) return false;

        selected = Math.Min(selected + lines, menu.MenuOptions.Count-1);
        
        if (selected - offset > 6) offset = selected - 6;

        return true;
    }

    public bool MoveUp(int lines = 1)
    {
        if (selected == 0) return false;

        selected = Math.Max(selected - lines, 0);
        if (selected < offset) offset = selected;

        return true;
    }

    public bool IsEqualButtons(string buttons)
    {
        var flag = prev_buttons.Equals(buttons);
        prev_buttons = buttons;
        return flag;
    }

    public int Selected()
    {
        return selected;
    }

    public void OnSelect()
    {            
        if (selected < menu.MenuOptions.Count && !menu.MenuOptions[selected].Disabled)
        {
            try
            {
                menu.MenuOptions[selected].OnSelect(player, menu.MenuOptions[selected]);                    
            }
            catch(Exception e) 
            {
                Control.GetPlugin()?.Logger.LogInformation("Error was caused in calling plugin [{message}]\n{stacktrace}", e.Message, e.StackTrace);
            }
            if (menu.PostSelectAction == PostSelectAction.Close)
                Close();
        }
    }

    public void Close()
    {            
        closed = true;
    }

    public bool Closed()
    {
        return closed;
    }

}
