using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using API;
using API.Models;
using Core.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class PostControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PostControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        // _client = factory.CreateClient();
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });


    }

    [Fact]
    public async Task Create_ShouldReturnBadValidation()
    {
        // Arrange
        
    }

}
