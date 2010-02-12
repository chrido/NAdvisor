using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace NAdvisorTest
{
    public static class MSTestHelper
    {
        public static void ShouldThrowException<TypeOfException>(Action exceptionThrowingAction)
        {
            try
            {
                exceptionThrowingAction();
            }
            catch(Exception ex)
            {
                if(!(ex is TypeOfException))
                {
                    Assert.Fail("Should throw a different kind of exception");
                }
            }
        }
    }
}
