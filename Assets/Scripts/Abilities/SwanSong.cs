namespace Abilities
{
    public class SwanSong : Ability
    {
        public SwanSong()
        {
            ability_name = "Swan Song";
            ability_description = "If this unit dies this turn, each unit on the team can take an extra action.";
        }
        public override void useAbility()
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.SwanSong);
        }
        public override bool isHighPriority()
        {
            return true;
        }
        public override bool getRequiredTarget()
        {
            return true;
        }
    }
}