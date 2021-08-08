using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace PeacefulHills.Testing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestAsyncAttribute : NUnitAttribute, ISimpleTestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        public TestMethod BuildFrom(IMethodInfo method, Test suite)
        {
            var parms = new TestCaseParameters(new object[] {method, suite})
            {
                ExpectedResult = new object(), 
                HasExpectedResult = true
            };

            MethodInfo proxyMethodInfo = GetType().GetMethod("MethodProxy", BindingFlags.Static | BindingFlags.Public);
            var proxyMethodWrapper = new MethodWrapper(GetType(), proxyMethodInfo);
            suite.Method = proxyMethodWrapper;

            TestMethod proxyMethod = _builder.BuildTestMethod(proxyMethodWrapper, suite, parms);
            proxyMethod.Name = method.Name;
            proxyMethod.parms.HasExpectedResult = false;
            
            return proxyMethod;
        }

        public static IEnumerator MethodProxy(IMethodInfo method, Test suite)
        {
            EnumerableSynchronizationContext context = AsyncHelpers.RunSync(() => (Task) method.Invoke(suite.Fixture));
            return context.BeginMessageLoop();
        }
    }
}