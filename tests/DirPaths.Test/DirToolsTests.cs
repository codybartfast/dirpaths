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
        var path1 = $"/{apple}/{banana}";
        var path2 = $"/{apple}/{banana}/";

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

        Assert.Equal("/Apple", "/Apple/Banana".Parent());
        Assert.Equal("/", "/Apple/".Parent());
        Assert.Null("/".Parent());
    }

    [Fact]
    public void SearchUpTests()
    {
        Assert.Null(nullStr.SearchUp("blah"));
        Assert.Null("/Apple/Banana".SearchUp("cherry"));

        Assert.Equal("/Apple", "/Apple/Banana".SearchUp("Apple"));
        Assert.Equal("/Apple/", "/Apple/".SearchUp("Apple"));
        Assert.Equal("/Apple/Banana/Apple",
            "/Apple/Banana/Apple".SearchUp("Apple"));
    }

    [Fact]
    public void SiblingTests()
    {
        Assert.Null(nullStr.Sibling("blah"));

        Assert.Equal("/Apple/Cherry", "/Apple/Banana".Sibling("Cherry"));
        Assert.Equal("/Apple/Banana", "/Apple/Banana".Sibling("Banana"));

        Assert.Null("/".Sibling("Banana"));
    }

    [Fact]
    public void SubDirTests()
    {
        Assert.Null(nullStr.SubDir("blah"));

        Assert.Equal("/Apple/Banana/Cherry", "/Apple/Banana".SubDir("Cherry"));
        Assert.Equal("/Apple", "/".SubDir("Apple"));
    }
}