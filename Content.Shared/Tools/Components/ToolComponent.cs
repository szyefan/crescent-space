using Content.Shared.Tools.Systems;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Tools.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
// Trauma - removed Access
public sealed partial class ToolComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<ProtoId<ToolQualityPrototype>> Qualities = [];

    /// <summary>
    /// For tool interactions that have a delay before action this will modify the rate, time to wait is divided by this value
    /// </summary>
    [DataField, AutoNetworkedField]
    public float SpeedModifier = 1f;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? UseSound;

    // Frontier: hide qualities
    [DataField]
    public bool HideQualities;
    // End Frontier
}

/// <summary>
/// Attempt event called *before* any do afters to see if the tool usage should succeed or not.
/// Raised on both the tool and then target.
/// </summary>
public sealed class ToolUseAttemptEvent(EntityUid user, float fuel, EntityUid tool, IEnumerable<ProtoId<ToolQualityPrototype>> qualities) : CancellableEntityEventArgs // Frontier: added tool, qualities
{
    public EntityUid User { get; } = user;
    public float Fuel = fuel;
    public EntityUid Tool { get; } = tool; // Frontier: the tool being used
    public IEnumerable<ProtoId<ToolQualityPrototype>> Qualities { get; } = qualities; // Frontier: the tool qualities being used //Crescent-Space: changes were made turning this into the ToolQualityPrototype. hope this wont break function later if it will i suppose ill have to fix it
}

/// <summary>
/// Event raised on the user of a tool to see if they can actually use it.
/// </summary>
[ByRefEvent]
public struct ToolUserAttemptUseEvent(EntityUid? target)
{
    public EntityUid? Target = target;
    public bool Cancelled = false;
}
