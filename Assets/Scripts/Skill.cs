///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace MysteryDungeon
{
    [System.Serializable]
    public enum SkillType
    {
        Regular,
        Magic,
        Heal,
        Status
    }

    public enum TargetType
    {
        Regular,
        Self,
        Enemies,
        Party,
        Room,
        All
    }

    [CreateAssetMenu(menuName = "MysteryDungeon/Skill", fileName = "Skill", order = 0)]
    public class Skill : ScriptableObject
    {
        [SerializeField] private SkillType m_skillType;
        [SerializeField] private TargetType m_targetType;

        [SerializeField, Min(0)] private int m_baseDamage;
        [SerializeField, Min(1)] private int m_tileRange;
        [SerializeField] private bool m_cutsCorners;

        public SkillType SkillType => m_skillType;
        public TargetType TargetType => m_targetType;
        public int BaseDamage => m_baseDamage;
        public int TileRange => m_tileRange;
        public bool CutsCorners => m_cutsCorners;

        private Entity EntityTileCornerCheck(Entity user, Tile targetTile)
        {
            // if the Skill does not cut corners, and the User is facing a diagonal, it needs to check for corners
            if (!CutsCorners && Mathf.Abs(user.FacingDirection.x + user.FacingDirection.y) != 1)
            {
                Vector2Int firstWall = user.GridPosition + new Vector2Int(user.FacingDirection.x, 0);
                Vector2Int secondWall = user.GridPosition + new Vector2Int(0, user.FacingDirection.y);

                // check the tiles. if they are walls, return null
                if (GridManager.Instance.GetTileAt(firstWall).Content.Type == TileType.Wall)
                    return null;
                if (GridManager.Instance.GetTileAt(secondWall).Content.Type == TileType.Wall)
                    return null;
            }

            return targetTile.Content.ObjectOnTile.GetComponent<Entity>();
        }

        public Entity GetTarget(Entity user)
        {
            switch (m_targetType)
            {
                case TargetType.Regular:
                    // if the skill has a range of 1, then use the simpler tile check method
                    if (m_tileRange == 1)
                    {
                        Tile targetTile = GridManager.Instance.GetTileAt(user.GridPosition + user.FacingDirection);
                        // if there is an entity on the targeted tile
                        if (targetTile.Content.Type == TileType.Entity)
                            return EntityTileCornerCheck(user, targetTile);
                        else
                            return null;
                    }

                    // if the range is not 1, then use the multi-tile check method
                    Tile[] targetTiles = GridManager.Instance.GetTilesInRange(user.GridPosition, user.FacingDirection, m_tileRange);
                    foreach (Tile tile in targetTiles)
                    {
                        if (tile.Content.Type == TileType.Entity)
                            return EntityTileCornerCheck(user, tile);
                    }
                    return null;
                case TargetType.Self:
                    return user;
                default:
                    return null;
            }
        }
    }
}
