using UnityEngine;
using System.Collections.Generic;

public class GameConsts : MonoBehaviour {
	#region Variables
	// layers
	public const string PLAYER_LAYER = "Player";
	public const string PLAYER_HITBOX_LAYER = "PlayerHitbox";
	public const string ENEMY_LAYER = "Enemy";
	public const string LAVA_LAYER = "Lava";

	public const string PLAYER_PROJECTILE_LAYER = "PlayerProjectile";
	public const string ENEMY_PROJECTILE_LAYER = "EnemyProjectile";

	// input
	public const string HORIZONTAL_AXIS_NAME = "Horizontal";
	public const string VERTICAL_AXIS_NAME =   "Vertical";
	#endregion
}