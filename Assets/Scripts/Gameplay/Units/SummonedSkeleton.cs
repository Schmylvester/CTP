public class SummonedSkeleton : Unit
{
    public override string getName()
    {
        return "Skeleton";
    }
    protected override void setAbilities()
    {
    }

    protected override void setStats()
    {
        base_stats[(int)Stat.Accuracy] = 90;
        base_stats[(int)Stat.Agility] = 0;
        base_stats[(int)Stat.Attack] = 70;
        base_stats[(int)Stat.Defence] = 0;
        base_stats[(int)Stat.Intelligence] = 0;
        base_stats[(int)Stat.Max_HP] = -90;
        base_stats[(int)Stat.Speed] = 0;

        //validateStats(190);
    }
}
