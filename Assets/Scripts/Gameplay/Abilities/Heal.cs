﻿namespace Abilities
{
    public class Heal : Ability
    {
        public Heal()
        {
            ability_name = "Heal";
            ability_description = "Recovers some of the target's health";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            uses--;
            int healed = target.changeHealth(user.getStat(Stat.Intelligence), feedback);
            feedback.printMessage(user.getName() + " healed " + target.getName() + " by " + healed + " health");
        }

        public override bool isHighPriority()
        {
            return false;
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingAlly;
        }
    }
}