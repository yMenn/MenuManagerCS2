using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;

namespace MenuManagerCore;

public static class Extensions
{
    public static void Freeze(this CCSPlayerController player)
    {
        if (player.Pawn.Value != null)
        {
            player.Pawn.Value.MoveType = MoveType_t.MOVETYPE_OBSOLETE;
            Schema.SetSchemaValue(player.Pawn.Value.Handle, "CBaseEntity", "m_nActualMoveType", 1); // obsolete
            Utilities.SetStateChanged(player.Pawn.Value, "CBaseEntity", "m_MoveType");
        }
    }

    public static void Unfreeze(this CCSPlayerController player)
    {
        if (player.Pawn.Value != null)
        {
            player.Pawn.Value.MoveType = MoveType_t.MOVETYPE_WALK;
            Schema.SetSchemaValue(player.Pawn.Value.Handle, "CBaseEntity", "m_nActualMoveType", 2); // walk
            Utilities.SetStateChanged(player.Pawn.Value, "CBaseEntity", "m_MoveType");
        }
    }
}