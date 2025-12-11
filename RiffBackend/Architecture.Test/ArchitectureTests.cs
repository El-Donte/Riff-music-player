using NetArchTest.Rules;
using System.Reflection;
using FluentAssertions;

namespace Architecture.Test
{
    public class ArchitectureTests
    {
        private readonly string Core = "RiffBackend.Core";
        private readonly string Application = "RiffBackend.Application";
        private readonly string Infrastructure = "RiffBackend.Infrastructure";
        private readonly string Api = "RiffBackend.API";

        [Fact]
        public void Domain_Should_Not_Have_Dependency_On_Other_Projects()
        {
            // Arrange
            var assembly = Assembly.Load(Core);

            var otherProjects = new[]
            {
                Application, Infrastructure, Api,
            };

            // Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_Have_Dependency_On_Other_Projects()
        {
            // Arrange
            var assembly = Assembly.Load(Application);

            var otherProjects = new[]
            {
                Infrastructure, Api,
            };

            // Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_Have_Dependency_On_Other_Projects()
        {
            // Arrange
            var assembly = Assembly.Load(Infrastructure);

            var otherProjects = new[]
            {
               Application, Api
            };

            // Act
            var result = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Api_Should_Not_Have_Dependency_On_Other_Projects()
        {
            // Arrange
            var assembly = Assembly.Load(Api);

            // Act
            var result = Types.InAssembly(assembly)
                .That()
                .ResideInNamespace(Api)
                .ShouldNot()
                .HaveDependencyOn(Infrastructure)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Controllers_Should_Have_Dependency_On_Application_Or_Core()
        {
            // Arrange
            var assembly = Assembly.Load(Api);

            // Act
            var result = Types.InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .HaveDependencyOnAny(Application, Core)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Controllers_Should_Not_Have_Dependency_On_Infrastructure()
        {
            // Arrange
            var assembly = Assembly.Load(Api);

            // Act
            var result = Types.InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Controller")
                .ShouldNot()
                .HaveDependencyOn(Infrastructure)
                .GetResult();

            // Assert
            result.IsSuccessful.Should().BeTrue();
        }
    }
}
