///<summary>
/// Author: Halen
///
///
///
///</summary>

using UnityEngine;
using UnityEngine.InputSystem;

namespace MysteryDungeon
{
	public class PlayerController : MonoBehaviour
	{
		private enum InputMode
		{
			Move,
			Look,
			Diagonal
		}

		[SerializeField] private Entity m_entityPrefab;
		private Entity m_entity;

        private Vector2 m_moveInput;

        private void Start()
        {
			m_entity = Instantiate(m_entityPrefab);
			m_entity.Init(Vector2Int.zero);
			m_entity.PlayerSetup();
        }

        private void Update()
        {
			if (m_entity.IsBusy)
				return;
			
			if (m_moveInput == Vector2.zero)
				return;

			if (m_moveInput != m_entity.FacingDirection)
				m_entity.FaceDirection(Vector2Int.RoundToInt(m_moveInput));
			else
				m_entity.ProcessMovement(Vector2Int.RoundToInt(m_moveInput));
        }

		public void GetMoveInput(InputAction.CallbackContext context)
		{
			m_moveInput = context.ReadValue<Vector2>();			
		}

		public void GetCheckInput(InputAction.CallbackContext context)
		{
			if (!m_entity.IsBusy && context.performed)
			{
				Tile targetTile = GridManager.Instance.GetTileAt(m_entity.GridPosition + m_entity.FacingDirection);
				switch (targetTile.Content.Type)
				{
					case TileType.Entity:
						m_entity.Attack(targetTile.Content.ObjectOnTile.GetComponent<Entity>());
						break;
					case TileType.Empty:
					case TileType.Wall:
					case TileType.Item:
					case TileType.Stairs:
						m_entity.Attack(null);
						break;
				}
			}
		}
    }
}
