using Aspire.Hosting;
using Aspire.Hosting.Testing;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace Frontend.A11y.Tests;

[TestFixture]
public class AccessibilityTests : PageTest
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

    [Test]
    public async Task Home_Page_Should_Be_Accessible()
    {
        Assert.That(_frontendUrl, Is.Not.Null);
        await Page.GotoAsync(_frontendUrl!);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var axeResults = await Page.RunAxe();

        Assert.That(axeResults.Violations, Is.Empty, $"Accessibility violations found on Home page: {axeResults.ToString()}");
    }

    [Test]
    public async Task Backlog_Page_Should_Be_Accessible()
    {
        Assert.That(_frontendUrl, Is.Not.Null);
        await Page.GotoAsync($"{_frontendUrl!}/backlog");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var axeResults = await Page.RunAxe();

        Assert.That(axeResults.Violations, Is.Empty, $"Accessibility violations found on Backlog page: {axeResults.ToString()}");
    }
}
