namespace Abilities
{
    public class SwanSong : Ability
    {
        public SwanSong()
        {
            ability_name = "Swan Song";
            ability_description = "If this unit dies this turn, each unit on the team can take an extra action.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            user.setActiveEffect(SingleTurnEffects.SwanSong);
            feedback.printMessage(user.getName() + " started singing a sad song.");
        }
        public override bool isHighPriority()
        {
            return true;
        }
        protected override TargetRequired targetRequired()
        {
            return TargetRequired.NoTargetRequired;
        }
    }
}