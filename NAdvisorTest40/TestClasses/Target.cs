namespace NAdvisorTest40
{
    public class Target : ITarget
    {
        public string DoneSomething(string input)
        {
            return input + "done";
        }
    }
}