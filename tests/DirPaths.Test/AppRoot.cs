namespace Fmbm.Dir.Test;

using XAssert = Xunit.Assert;

public class AppRoot
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
        var third = "Third";
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
}