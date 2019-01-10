namespace Abilities
{
    public class Summon : Ability
    {
        public Summon()
        {
            ability_name = "Raise Dead";
            ability_description = "Summons some skeletons to fight alongside you.";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            int summoned = user.getTeam().addSkeletons(user.grid_pos.x);
            feedback.printMessage(user.getName() + " summons " + summoned + " skeletons.");
        }
        public override bool isHighPriority()
        {
            return false;
        }
        protected override TargetRequired targetRequired()
        {
            return TargetRequired.NoTargetRequired;
        }
    }
}