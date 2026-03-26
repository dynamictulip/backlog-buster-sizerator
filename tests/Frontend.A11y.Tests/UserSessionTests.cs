using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Frontend.A11y.Tests;

[TestFixture]
public class UserSessionTests : PageTest
{
    private IDistributedApplicationTestingBuilder? _appBuilder;
    private DistributedApplication? _app;
    private string? _frontendUrl;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _appBuilder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AppHost>();
        _app = await _appBuilder.BuildAsync();
        await _app.StartAsync();

        var frontend = _app.GetEndpoint("frontend");
        _frontendUrl = frontend.ToString();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (_app != null)
        {
            await _app.DisposeAsync();
        }
    }

    [SetUp]
    public async Task ClearSession()
    {
        await Page.GotoAsync(_frontendUrl!);
        await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        await Page.EvaluateAsync("localStorage.clear()");
    }

    [Test]
    public async Task Home_Page_Shows_Name_Prompt_On_First_Visit()
    {
        Assert.That(_frontendUrl, Is.Not.Null);
        await Page.GotoAsync(_frontendUrl!);

        var modal = Page.GetByText("Welcome to Backlog Buster!");
        await Expect(modal).ToBeVisibleAsync(new() { Timeout = 15000 });
    }

    [Test]
    public async Task Backlog_Page_Shows_Name_Prompt_On_First_Visit()
    {
        Assert.That(_frontendUrl, Is.Not.Null);
        await Page.GotoAsync($"{_frontendUrl!}/backlog");

        var modal = Page.GetByText("Welcome to Backlog Buster!");
        await Expect(modal).ToBeVisibleAsync(new() { Timeout = 15000 });
    }

    [Test]
    public async Task Name_Prompt_Disappears_After_Name_Is_Entered()
    {
        Assert.That(_frontendUrl, Is.Not.Null);
        await Page.GotoAsync(_frontendUrl!);

        await Page.GetByPlaceholder("Enter your name").FillAsync("TestUser", new() { Timeout = 15000 });
        await Page.GetByRole(AriaRole.Button, new() { Name = "Save Name" }).ClickAsync();

        var modal = Page.GetByText("Welcome to Backlog Buster!");
        await Expect(modal).ToBeHiddenAsync();
    }
}
