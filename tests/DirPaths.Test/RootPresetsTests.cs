namespace Fmbm.Dir.Test;

public class RootPresetsTests
{
    string? nullStr = null;

    private bool disposedValue;

    readonly string otherEnvVarName = "FMBM_TEST_ENV_NAME";

    string? GetDefault() => Environment.GetEnvironmentVariable(
        RootPresets.DefaultEnvironmentVariableName);
    void SetDefault(string val)
    {
        Environment.SetEnvironmentVariable(
            RootPresets.DefaultEnvironmentVariableName, val);
    }

    string? GetOther() => Environment.GetEnvironmentVariable(otherEnvVarName);
    void SetOther(string val)
    {
        Environment.SetEnvironmentVariable(otherEnvVarName, val);
    }


    [Fact]
    public void BaseTests()
    {
        XAssert.Equal(AppContext.BaseDirectory, RootPresets.Base);
    }

    [Fact]
    public void CurrentTests()
    {
        XAssert.Equal(
            System.Environment.CurrentDirectory, RootPresets.Current);
    }

    [Fact]
    public void EnvironmentVariableTests()
    {
        var value = "bloopy black blobs";
        var value2 = "some stinky streaks";
        SetDefault(null!);
        SetOther(null!);
        XAssert.Null(RootPresets.EnvironmentVariable());
        XAssert.Null(RootPresets.EnvironmentVariable(
            RootPresets.DefaultEnvironmentVariableName));
        XAssert.Null(RootPresets.EnvironmentVariable(otherEnvVarName));

        SetDefault(value);
        XAssert.Equal(value, RootPresets.EnvironmentVariable());
        XAssert.Equal(value, RootPresets.EnvironmentVariable(
            RootPresets.DefaultEnvironmentVariableName));
        XAssert.Null(RootPresets.EnvironmentVariable(otherEnvVarName));

        SetOther(value2);
        XAssert.Equal(value, RootPresets.EnvironmentVariable());
        XAssert.Equal(value2, RootPresets.EnvironmentVariable(otherEnvVarName));
    }

}
