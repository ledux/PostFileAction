using AutoBogus;
using EF.Language.PostFileAction.Config;
using FluentAssertions;
using Xunit;

namespace EF.Language.PostFileAction.Tests.Config.ActionInputsTests;

public class ValidateTests
{
    [Fact]
    public void UseAuthIsFalse_ReturnsTrueAndEmptyArray()
    {
        var testee = new ActionInputs();

        var (isValid , errorMessages) = testee.Validate();

        isValid.Should().BeTrue();
        errorMessages.Should().BeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UseAuthIsTrue_TokenEndpointIsNullOrEmpty_ReturnsFalse(string tokenEndpoint)
    {
        var testee = new AutoFaker<ActionInputs>()
            .RuleFor(inputs => inputs.TokenEndpointString, tokenEndpoint)
            .RuleFor(inputs => inputs.UseAuth, true)
            .Generate();

        var (isValid, errorMessages) = testee.Validate();

        isValid.Should().BeFalse();
        errorMessages.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UseAuthIsTrue_ClientIdIsNullOrEmpty_ReturnsFalse(string clientId)
    {
        var testee = new AutoFaker<ActionInputs>()
            .RuleFor(inputs => inputs.ClientId, clientId)
            .RuleFor(inputs => inputs.UseAuth, true)
            .Generate();

        var (isValid, errorMessages) = testee.Validate();

        isValid.Should().BeFalse();
        errorMessages.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void UseAuthIsTrue_ClientSecretIsNullOrEmpty_ReturnsFalse(string clientSecret)
    {
        var testee = new AutoFaker<ActionInputs>()
            .RuleFor(inputs => inputs.ClientSecret, clientSecret)
            .RuleFor(inputs => inputs.UseAuth, true)
            .Generate();

        var (isValid, errorMessages) = testee.Validate();

        isValid.Should().BeFalse();
        errorMessages.Should().HaveCount(1);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, "")]
    [InlineData("", null)]
    [InlineData("", "")]
    public void UseAuthIsTrue_ClientSecretAndClientIdAreNullOrEmpty_ReturnsFalse(string clientSecret, string clientId)
    {
        var testee = new AutoFaker<ActionInputs>()
            .RuleFor(inputs => inputs.ClientSecret, clientSecret)
            .RuleFor(inputs => inputs.ClientId, clientId)
            .RuleFor(inputs => inputs.UseAuth, true)
            .Generate();

        var (isValid, errorMessages) = testee.Validate();

        isValid.Should().BeFalse();
        errorMessages.Should().HaveCount(2);
    }
}