[assembly: TestCaseOrderer("Fmbm.Dir.Test.AlphabeticOrderer", "DirPaths.Test")]

namespace Fmbm.Dir.Test;

using Xunit.Abstractions;
using Xunit.Sdk;

// I think I picked this up from: 
// https://www.thecodebuzz.com/order-unit-test-cases-or-integration-testing-guidelines/
public class AlphabeticOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        return testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
    }
}

public class DirPathsTests
{
    public void SetAndCheckPath()
    {
        string path = DateTime.UtcNow.ToString("o");
        DirPaths.SetAppRootPath(path);
        XAssert.Equal(path, DirPaths.AppRoot.Path);
    }

    /* Yes, I used static state, deal with it. So most of the following
       tests are commented out because they are mutually incompatible.
    */

    // [Fact]
    // public void WhenNoUserConfig_APathIsReturned()
    // {
    //     XAssert.NotNull(DirPaths.AppRoot.Path);
    // }

    // [Fact]
    // public void WhenPathIsSet_ThatPathIsReturned()
    // {
    //     SetAndCheckPath();
    // }


    // [Fact]
    // public void AfterPathIsRead_ItCannotBeSet()
    // {
    //     var _ = DirPaths.AppRoot.Path;
    //     var action = () => { DirPaths.AppRoot.Path = "Fred"; };
    //     XAssert.Throws<InvalidOperationException>(action);
    // }

    // [Fact]
    // public void WhenPathIsCleared_NoPathIsReturned_AndIfReadIsAttemptedThenItCannotBeSet()
    // {
    //     DirPaths.ClearAppRoot();

    //     var tryRead = (() => DirPaths.AppRoot.Path);
    //     var _ = Assert.Throws<InvalidOperationException>(tryRead);

    //     Action trySet = (() => DirPaths.SetAppRootPath("Second"));
    //     var _1 = Assert.Throws<InvalidOperationException>(trySet);
    // }

    // [Fact]
    // public void WhenPathIsCleared_PathCanBeSet()
    // {
    //     DirPaths.SetAppRootPath("Second");
    //     DirPaths.ClearAppRoot();
    //     SetAndCheckPath();
    // }

    // [Fact]
    // public void AfterPathIsRead_ItCannotBeCleared()
    // {
    //     var _ = DirPaths.AppRoot.Path;
    //     var action = () => DirPaths.ClearAppRoot();
    //     XAssert.Throws<InvalidOperationException>(action);
    // }

    // [Fact]
    // public void SetAppRootPath_WhenAllAreNull_PathIsNotSet_AndReturnsFalse()
    // {
    //     var r = DirPaths.SetAppRootPath(null, null);
    //     Assert.NotNull(DirPaths.AppRoot.Path);
    //     Assert.False(r);
    // }

    // [Fact]
    // public void SetAppRootPath_FirstNonNullStingIsUsed_AndReturnsTrue()
    // {
    //     var second = "Second";
    //     var r = DirPaths.SetAppRootPath(null, null, second, "Third");
    //     XAssert.Equal(second, DirPaths.AppRoot.Path);
    //     XAssert.True(r);
    // }

    /*
        Because I was very naughty and chose to use a static class, most of
        the above tests are mutually incompatible.  Although not as thorough
        nor clear, this compund test should cover many of the cases above.
    */
    [Fact]
    public void Compound_Test()
    {
        var third = "AppRoot";
        XAssert.False(DirPaths.SetAppRootPath(null, null, null));
        DirPaths.AppRoot.Path = "Second";
        DirPaths.ClearAppRootPath();
        XAssert.True(DirPaths.SetAppRootPath(null, null, third, "Fourth", null));

        XAssert.Equal(third, DirPaths.AppRoot.Path);

        Action trySet = () => DirPaths.SetAppRootPath(null, "Fifth");
        var _ = XAssert.Throws<InvalidOperationException>(trySet);

        Action tryClear = () => DirPaths.ClearAppRootPath();
        var _1 = XAssert.Throws<InvalidOperationException>(tryClear);

        XAssert.Equal(third, DirPaths.AppRoot.Path);
    }

    [Fact]
    public void DirectoryCreatedOnlyIfCheckedPath()
    {

        var name = "Fmbm.Dir.Test.TestDir";
        var path = Path.Combine(DirPaths.AppRoot.Path, name);
        var dirInfo = new DirectoryInfo(path);
        if (dirInfo.Exists)
        {
            throw new InvalidOperationException(
                $"Test Dir Already Exists! :{dirInfo.FullName}");
        }
        try
        {
            var dirPath = DirPaths.GetDir(name);
            var _ = dirPath.Path;
            dirInfo.Refresh();
            XAssert.False(dirInfo.Exists);

            var _1 = dirPath.CheckedPath;
            dirInfo.Refresh();
            XAssert.True(dirInfo.Exists);
        }
        finally
        {
            dirInfo.Delete();
        }
    }

    [Fact]
    public void WhenComperableNamesAreUsed_TheSameDirPathIsReturned()
    {
        var child1 = DirPaths.GetDir("CHILD");
        var child2 = DirPaths.GetDir("child");
        var child3 = DirPaths.GetDir(" Child ");
        XAssert.Same(child1, child2);
        XAssert.Same(child2, child3);
    }
}