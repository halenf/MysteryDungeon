///<summary>
/// Author:
///
///
///
///</summary>

using UnityEngine;

namespace MysteryDungeon
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class Tile : MonoBehaviour
	{
		[SerializeField] private Color m_baseColour, m_highlightColour;
		[SerializeField] private SpriteRenderer m_renderer;

		private TileContent m_content;
		public TileContent Content => m_content;

		public void Init(TileContent content)
		{
			m_content = content;
            m_renderer.color = m_baseColour;
        }

        public void SetHighlighted(bool isHighlighted)
		{
			m_renderer.color = isHighlighted ? m_highlightColour : m_baseColour;
		}

		public void SetContent(TileContent content)
		{
			m_content = content;
		}
	}
}
