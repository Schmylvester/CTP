using UnityEngine;
namespace Abilities
{
    public class Charm : Ability
    {
        public Charm()
        {
            ability_name = "Charm";
            ability_description = "May prevent target from performing action this turn";
        }
        public override void useAbility()
        {
            uses--;
            float chance = Mathf.Pow(0.9f, (target.getStat(Stat.Defence) + target.getStat(Stat.Intelligence) / 2));
            if (Random.Range(0.0f, 1.0f) < chance)
            {
                target.setActiveEffect(SingleTurnEffects.Charmed);
                new ActionFeedbackText().printMessage(user.getName() + " has charmed " + target.getName());
            }
            else
            {
                new ActionFeedbackText().printMessage(user.getName() + " tried to put the moves on " + target.getName() + " but they weren't into it.");
            }
        }

        public override bool isHighPriority()
        {
            return true;
        }
        public override bool getRequiredTarget()
        {
            return user.getTeam() != target.getTeam() && target.getHealth() > 0;
        }
    }
}