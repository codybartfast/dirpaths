using System.Text.RegularExpressions;

using Fmbm.IO.StringExtensions;

namespace Fmbm.IO.Test;

public static class XPlat{
    public static string? Psx(this string? text){
        if(text is null){
            return null;
        }
        text = Regex.Replace(text, @"^\w:\\", "/");
        text = text.Replace(@"\", "/");
        return text;
    }
}

public class PathToolsTests
{
    string? nullStr = null;

    [Fact]
    public void FirstNonNullTests()
    {
        XAssert.Null(PathTools.FirstNonNull(new string[] { }));
        XAssert.Null(PathTools.FirstNonNull(new string[] { null! }));
        XAssert.Null(PathTools.FirstNonNull(new string[] { null!, null!, null! }));
        var firstStr = "BlabbyBigMouth";
        var actual = PathTools.FirstNonNull(
            new string[] { null!, null!, firstStr, "second" });
        XAssert.Equal(firstStr, actual);
    }

    [Fact]
    public void NamesAreEqualTests()
    {
        XAssert.False(PathTools.NamesAreEqual(null!, null!));
        XAssert.False(PathTools.NamesAreEqual("Apple", null!));
        XAssert.False(PathTools.NamesAreEqual(null!, "Apple"));
        XAssert.False(PathTools.NamesAreEqual("Apple", "Banana"));
        XAssert.False(PathTools.NamesAreEqual("Apple", " Apple"));

        XAssert.True(PathTools.NamesAreEqual("Apple", "Apple"));
        XAssert.True(PathTools.NamesAreEqual("Apple", "aPPLE"));
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
        XAssert.Null(nullStr.Parent());

        XAssert.Equal("/Apple", "/Apple/Banana".Parent().Psx());
        XAssert.Equal("/", "/Apple/".Parent().Psx());
        XAssert.Null("/".Parent().Psx());
    }

    [Fact]
    public void SearchUpTests()
    {
        XAssert.Null(nullStr.SearchUp("blah"));
        XAssert.Null("/Apple/Banana".SearchUp("cherry"));

        XAssert.Equal("/Apple", "/Apple/Banana".SearchUp("Apple").Psx());
        XAssert.Equal("/Apple/", "/Apple/".SearchUp("Apple").Psx());
        XAssert.Equal("/Apple/Banana/Apple",
            "/Apple/Banana/Apple".SearchUp("Apple").Psx());
    }

    [Fact]
    public void SiblingTests()
    {
        XAssert.Null(nullStr.Sibling("blah"));

        XAssert.Equal("/Apple/Cherry", "/Apple/Banana".Sibling("Cherry").Psx());
        XAssert.Equal("/Apple/Banana", "/Apple/Banana".Sibling("Banana").Psx());

        XAssert.Null("/".Sibling("Banana"));
    }

    [Fact]
    public void SubDirTests()
    {
        XAssert.Null(nullStr.SubDir("blah"));

        XAssert.Equal("/Apple/Banana/Cherry", "/Apple/Banana".SubDir("Cherry").Psx());
        XAssert.Equal("/Apple", "/".SubDir("Apple").Psx());
    }
}
