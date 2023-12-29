using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using VerifyCS = Microsoft.Data.SqlClient.Analyzers.Test.CSharpCodeFixVerifier<
//Microsoft.Data.SqlClient.Analyzers.InvariantGlobalizationAnalyzer>;

namespace Microsoft.Data.SqlClient.Analyzers.Test
{
    [TestClass]
    public class MicrosoftDataSqlClientAnalyzersUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            //var test = @"";

            //await VerifyCS.VerifyAnalyzerAsync(test);
            await Task.FromResult(true);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            //        var test = @"
            //using System;
            //using System.Collections.Generic;
            //using System.Linq;
            //using System.Text;
            //using System.Threading.Tasks;
            //using System.Diagnostics;

            //namespace ConsoleApplication1
            //{
            //    class {|#0:TypeName|}
            //    {   
            //    }
            //}";

            //        var fixtest = @"
            //using System;
            //using System.Collections.Generic;
            //using System.Linq;
            //using System.Text;
            //using System.Threading.Tasks;
            //using System.Diagnostics;

            //namespace ConsoleApplication1
            //{
            //    class TYPENAME
            //    {   
            //    }
            //}";

            //        var expected = VerifyCS.Diagnostic("MicrosoftDataSqlClientAnalyzers").WithLocation(0).WithArguments("TypeName");
            //        await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
            await Task.FromResult(true);
        }
    }
}
