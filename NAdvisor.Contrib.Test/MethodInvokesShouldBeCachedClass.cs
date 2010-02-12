namespace TechTalk.NAdvisor.Contrib.Test
{
    public class MethodInvokesShouldBeCachedClass : IMethodInvokesShouldBeCachedClass
    {

        public int AwfulLongCacheAbleComputation(string inputParameter)
        {
            int count = 0;

            foreach (char c in inputParameter.ToCharArray())
            {
                count += (int)c;
            }

            return count;
        }

        public string IllegalMethodWithOut(out string inputParam)
        {
            inputParam = "test";
            return "bammoida";
        }
    }
}