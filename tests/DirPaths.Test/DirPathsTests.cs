namespace Fmbm.Dir.Test;

using Xunit.Abstractions;
using Xunit.Sdk;
using XAssert = Xunit.Assert;

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase =>
        testCases.OrderBy(testCase => testCase.TestMethod.Method.Name);
}

[TestCaseOrderer("AlphabeticalOrderer", "DirPaths.Testss")]
public class DirPathsTests
{
    public void SetAndCheckPath()
    {
        string path = DateTime.UtcNow.ToString("o");
        DirPaths.SetAppRootPath(path);
        XAssert.Equal(path, DirPaths.AppRoot.Path);
    }

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
        the above tests are mutually exclusive.  Although not as thorough
        nor clear, this compund test should cover many of the cases above.
    */
    [Fact]
    public void Compound_Test()
    {
        var third = "AppRoot";
        XAssert.False(DirPaths.SetAppRootPath(null, null, null));
        DirPaths.AppRoot.Path = "Second";
        DirPaths.ClearAppRoot();
        XAssert.True(DirPaths.SetAppRootPath(null, null, third, "Fourth", null));

        XAssert.Equal(third, DirPaths.AppRoot.Path);

        Action trySet = () => DirPaths.SetAppRootPath(null, "Fifth");
        var _ = XAssert.Throws<InvalidOperationException>(trySet);

        Action tryClear = () => DirPaths.ClearAppRoot();
        var _1 = XAssert.Throws<InvalidOperationException>(tryClear);

        XAssert.Equal(third, DirPaths.AppRoot.Path);
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
            Assert.False(dirInfo.Exists);

            var _1 = dirPath.CheckedPath;
            dirInfo.Refresh();
            Assert.True(dirInfo.Exists);
        }
        finally
        {
            dirInfo.Delete();
        }
    }
}