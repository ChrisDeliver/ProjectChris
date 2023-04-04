using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SpecificNeighborRuleTile : RuleTile
{
    [SerializeField] private RuleTile specificRuleTile;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleTile && other == specificRuleTile)
        {
            return true;
        }

        return base.RuleMatch(neighbor, other);
    }
}