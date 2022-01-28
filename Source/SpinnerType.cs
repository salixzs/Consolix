namespace Salix.Extensions;

/// <summary>
/// Enumerates the possible types of spinner look.
/// </summary>
public enum SpinnerType
{
    /// <summary>
    /// Uses cycling symbols of \ | / -
    /// </summary>
    Lines,

    /// <summary>
    /// Pulsating .oOo.
    /// </summary>
    Circles,

    /// <summary>
    /// Interchanging + x
    /// </summary>
    Cross,

    /// <summary>
    /// Swapping between symbols ▄ and ▀
    /// </summary>
    Cubes,

    /// <summary>
    /// Growing dots (from 1 to 4).
    /// </summary>
    GrowingDots,

    /// <summary>
    /// Growing =====> arrow.
    /// </summary>
    GrowingArrow
}
