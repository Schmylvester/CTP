using UnityEngine;

public enum TargetRequired
{
    AnyLivingEnemy,
    AnyLivingAlly,
    AnyOtherLivingAlly,
    AnyDeadAlly,
    AnythingButSelf,
    Direction,
    NoTargetRequired,
}

public abstract class Ability
{
    protected Field field;
    protected Unit user;
    protected Unit target;
    public string ability_name = "invalid";
    public string ability_description;
    protected int uses = 5;
    protected int max_uses = 5;

    public abstract bool isHighPriority();
    public abstract void useAbility(ActionFeedbackText feedback);
    protected abstract TargetRequired targetRequired();

    public bool noTarget()
    {
        return targetRequired() == TargetRequired.NoTargetRequired;
    }

    public Unit getUser()
    {
        return user;
    }
    public Unit getTarget()
    {
        return target;
    }
    public bool canUse()
    {
        return uses > 0;
    }
    public void setUser(Unit _user)
    {
        user = _user;
    }
    public void setTarget(Unit _target)
    {
        target = _target;
    }
    public bool userAndTargetStillAlive()
    {
        return (user.getHealth() > 0 && target.getHealth() > 0);
    }
    public string getRemainingUses()
    {
        return uses + "/" + max_uses;
    }
    public void setField(Field _field)
    {
        field = _field;
    }

    public bool targetValid(ActionFeedbackText feedback)
    {
        TargetRequired required = targetRequired();
        if (required != TargetRequired.NoTargetRequired && required != TargetRequired.Direction && target == null)
        {
            return false;
        }
        switch (required)
        {
            case TargetRequired.Direction:
                return user.moveValid((this as Abilities.Move).direction, feedback);
            case TargetRequired.AnythingButSelf:
                return target != user && target.getHealth() > 0;
            case TargetRequired.AnyLivingAlly:
                return target.getTeam() == user.getTeam() && target.getHealth() > 0;
            case TargetRequired.AnyDeadAlly:
                return target.getTeam() == user.getTeam() && target.getHealth() <= 0;
            case TargetRequired.AnyLivingEnemy:
                return target.getTeam() != user.getTeam() && target.getHealth() > 0;
            case TargetRequired.AnyOtherLivingAlly:
                return target.getTeam() == user.getTeam() && target.getHealth() > 0 && user != target;
            case TargetRequired.NoTargetRequired:
                return true;
            default:
                Debug.LogError("There is a target requirement type not checked in get valid target");
                return false;
        }
    }

    public Color getTextColour()
    {
        switch (targetRequired())
        {
            case TargetRequired.AnyLivingEnemy:
                return Color.red * Color.gray;
            case TargetRequired.AnyLivingAlly:
                return Color.cyan * Color.gray;
            case TargetRequired.AnyOtherLivingAlly:
                return Color.green * Color.gray;
            case TargetRequired.AnyDeadAlly:
                return Color.yellow * Color.gray;
            case TargetRequired.AnythingButSelf:
                return Color.magenta * Color.gray;
            case TargetRequired.Direction:
                return Color.black;
            case TargetRequired.NoTargetRequired:
                return Color.blue * Color.gray;
            default:
                return Color.black;
        }
    }
}
namespace Abilities
{
    public class Attack : Ability
    {
        public Attack()
        {
            ability_name = "Attack";
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            int dam = user.attack(target, feedback);
            if (user == target)
                feedback.printMessage(user.getName() + " was caught in an ambush and took " + dam + " damage.");
            else if (dam > 0)
                feedback.printMessage(user.getName() + " attacked " + target.getName() + " for " + dam + " damage.");
        }

        public override bool isHighPriority()
        {
            return false;
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.AnyLivingEnemy;
        }
    }
    public class Move : Ability
    {
        public char direction = '\0';
        public Move()
        {
            ability_name = "Move";
        }
        public override bool isHighPriority()
        {
            return false;
        }
        public override void useAbility(ActionFeedbackText feedback)
        {
            user.move(direction, feedback);
            if (direction == 'b')
                feedback.printMessage(user.getName() + " moved away from the enemy.");
            if (direction == 'f')
                feedback.printMessage(user.getName() + " moved towards the enemy.");
        }

        protected override TargetRequired targetRequired()
        {
            return TargetRequired.Direction;
        }
    }
}