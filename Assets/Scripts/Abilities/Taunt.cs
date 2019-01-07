namespace Abilities
{
    public class Taunt : Ability
    {
        public Taunt()
        {
            ability_name = "Taunt";
            ability_description = "Target's attack increases, accuracy decreases and they must attack this unit as their next action.";
        }
        public override bool isHighPriority()
        {
            return true;
        }
        public override void useAbility()
        {
            uses--;
            target.taunted_by = user;
        }
        public override bool getRequiredTarget()
        {
            return target.getTeam() != user.getTeam() && target.getHealth() > 0;
        }
    }
}