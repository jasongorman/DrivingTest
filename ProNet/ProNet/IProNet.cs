namespace ProNet
{
    public interface IProNet
    {
        string[] Skills(string programmer);
        string[] Recommendations(string programmer);
        double Rank(string programmer);
        int DegreesOfSeparation(string programmer1, string programmer2);
        double TeamStrength(string language, string[] team);
        string[] FindStrongestTeam(string language, int teamSize);
    }
}