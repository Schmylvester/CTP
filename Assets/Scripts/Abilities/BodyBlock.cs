namespace Abilities
{
    public class BodyBlock : Ability
    {
        public BodyBlock()
        {
            ability_name = "Body Block";
            ability_description = "If target is attacked this turn, the user takes the attack instead.";
        }
        public override void useAbility()
        {
            uses--;
            target.defended_by = user;
            new ActionFeedbackText().printMessage(user.getName() + " is standing in front of " + target.getName());
        }

        public override bool isHighPriority()
        {
            return true;
        }

        public override bool getRequiredTarget()
        {
            if (user == target)
                return false;
            return user.getTeam() == target.getTeam() && target.getHealth() > 0;
        }
    }
}