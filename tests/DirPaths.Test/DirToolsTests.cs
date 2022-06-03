namespace Fmbm.Dir.Test;

public class DirToolsTests
{
    string? nullStr = null;

    [Fact]
    public void FirstNonNullTests()
    {
        XAssert.Null(DirTools.FirstNonNull(new string[] { }));
        XAssert.Null(DirTools.FirstNonNull(new string[] { null! }));
        XAssert.Null(DirTools.FirstNonNull(new string[] { null!, null!, null! }));
        var firstStr = "BlabbyBigMouth";
        var actual = DirTools.FirstNonNull(
            new string[] { null!, null!, firstStr, "second" });
        XAssert.Equal(firstStr, actual);
    }

    [Fact]
    public void NamesAreEqualTests()
    {
        XAssert.False(DirTools.NamesAreEqual(null!, null!));
        XAssert.False(DirTools.NamesAreEqual("Apple", null!));
        XAssert.False(DirTools.NamesAreEqual(null!, "Apple"));
        XAssert.False(DirTools.NamesAreEqual("Apple", "Banana"));
        XAssert.False(DirTools.NamesAreEqual("Apple", " Apple"));

        XAssert.True(DirTools.NamesAreEqual("Apple", "Apple"));
        XAssert.True(DirTools.NamesAreEqual("Apple", "aPPLE"));
    }

    [Fact]
    public void NameIsTests()
    {
        var apple = "Apple";
        var banana = "Banana";
        var path1 = @$"C:\{apple}\{banana}";
        var path2 = @$"C:\{apple}\{banana}\";

        XAssert.Null(nullStr.NameIs("blah"));
        XAssert.Null(nullStr.NameIs(null!));
        XAssert.Null(path1.NameIs(null!));
        XAssert.Null(path1.NameIs(""));
        XAssert.Null(path1.NameIs(path1));
        XAssert.Null(path1.NameIs(apple));
        XAssert.Null(path1.NameIs(banana.Substring(1)));
    }

    [Fact]
    public void ParentTests()
    {
        Assert.Null(nullStr.Parent());

        Assert.Equal(@"C:\Apple", @"C:\Apple\Banana".Parent());
        Assert.Equal(@"C:\", @"C:\Apple\".Parent());
        Assert.Null(@"C:\".Parent());
    }

    [Fact]
    public void SearchUpTests()
    {
        Assert.Null(nullStr.SearchUp("blah"));
        Assert.Null(@"C:\Apple\Banana".SearchUp("cherry"));

        Assert.Equal(@"C:\Apple", @"C:\Apple\Banana".SearchUp("Apple"));
        Assert.Equal(@"C:\Apple\", @"C:\Apple\".SearchUp("Apple"));
        Assert.Equal(@"C:\Apple\Banana\Apple",
            @"C:\Apple\Banana\Apple".SearchUp("Apple"));
    }

    [Fact]
    public void SiblingTests()
    {
        Assert.Null(nullStr.Sibling("blah"));

        Assert.Equal(@"C:\Apple\Cherry", @"C:\Apple\Banana".Sibling("Cherry"));
        Assert.Equal(@"C:\Apple\Banana", @"C:\Apple\Banana".Sibling("Banana"));

        Assert.Null(@"C:\".Sibling("Banana"));
    }

    [Fact]
    public void SubDirTests()
    {
        Assert.Null(nullStr.SubDir("blah"));

        Assert.Equal(@"C:\Apple\Banana\Cherry", @"C:\Apple\Banana".SubDir("Cherry"));
        Assert.Equal(@"C:\Apple", @"C:\".SubDir("Apple"));
    }
}