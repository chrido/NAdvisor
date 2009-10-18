namespace NAdvisor.Contrib.Test
{
    public interface IMethodInvokesShouldBeCachedClass
    {
        int AwfulLongCacheAbleComputation(string inputParameter);

        string IllegalMethodWithOut(out string inputParam);
    }
}