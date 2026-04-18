using UnityEngine;

public static class LayerUtils
{
    public static readonly int ENEMY_LAYER = LayerMask.NameToLayer("Enemy");
    public static readonly LayerMask ENEMY_LAYER_MASK = 1 << ENEMY_LAYER;

    public static readonly int PLAYER_LAYER = LayerMask.NameToLayer("Player");
    public static readonly LayerMask PLAYER_LAYER_MASK = 1 << PLAYER_LAYER;
}
