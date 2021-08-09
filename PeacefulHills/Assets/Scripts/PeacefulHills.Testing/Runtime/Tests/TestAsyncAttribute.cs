using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using UnityEngine;

namespace PeacefulHills.Testing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class TestAsyncAttribute : NUnitAttribute, ISimpleTestBuilder, IImplyFixture
    {
        private readonly NUnitTestCaseBuilder _builder = new NUnitTestCaseBuilder();

        public TestMethod BuildFrom(IMethodInfo method, Test suite)
        {
            ExecuteParameters executeParams = HandleExecuteParams(method.MethodInfo);
            var parms = new TestCaseParameters(new object[] {method, suite, executeParams})
            {
                ExpectedResult = new object(), 
                HasExpectedResult = true
            };

            Type type = GetType();
            MethodInfo proxyMethod = type.GetMethod(nameof(AsyncMethodProxy), BindingFlags.Static | BindingFlags.Public);
            var proxyMethodWrapper = new MethodWrapper(type, proxyMethod);
            suite.Method = proxyMethodWrapper;

            TestMethod proxyTestMethod = _builder.BuildTestMethod(proxyMethodWrapper, suite, parms);
            proxyTestMethod.Name = method.Name;
            proxyTestMethod.parms.HasExpectedResult = false;
            
            return proxyTestMethod;
        }

        private ExecuteParameters HandleExecuteParams(MethodInfo method)
        {
            var parameters = new ExecuteParameters();
            
            var repeatAttr = method.GetCustomAttribute<RepeatAttribute>();
            if (repeatAttr != null)
            {
                parameters.Repeat = (int)repeatAttr.Properties.Get("Repeat");
            }

            return parameters;
        }

        public static IEnumerator AsyncMethodProxy(IMethodInfo method, Test suite, ExecuteParameters execParams)
        {
            for (int i = 0; i < execParams.Repeat; i++)
            {
                IEnumerator enumerator = AsyncSupport.RunAsEnumerator(() => (Task) method.Invoke(suite.Fixture));

                while (enumerator.MoveNext())
                {
                    yield return null;
                }
            }
        }

        public class ExecuteParameters
        {
            public int Repeat = 1;
        }
    }
}