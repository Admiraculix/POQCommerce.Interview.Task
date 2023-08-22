using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace PoqCommerce.Tests.Architecture
{
    public class ArchitectureTest
    {
        private const string ApplicationNamespace = "PoqCommerce.Application";
        private const string DomainNamespace = "PoqCommerce.Domain";
        private const string PresentationNamespace = "PoqCommerce.Api";
        private const string InfrastructureNamespace = "PoqCommerce.Mocky.Io";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(DomainNamespace);

            var otherProjects = new[] {
                ApplicationNamespace,
                PresentationNamespace,
                InfrastructureNamespace,
            };

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(ApplicationNamespace);

            var otherProjects = new[] {
                PresentationNamespace,
                InfrastructureNamespace,
            };

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Presentation_Should_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(PresentationNamespace);

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .Should()
            .HaveDependencyOnAll()
            .GetResult();

            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(InfrastructureNamespace);

            var otherProjects = new[] {
                PresentationNamespace,
                DomainNamespace
            };

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .GetResult();

            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Services_Should_Have_DependencyOnDomain()
        {
            // Arrange
            Assembly assembly = Assembly.Load(ApplicationNamespace);

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .That()
            .AreClasses()
            .And()
            .HaveNameEndingWith("Service")
            .Should()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

            // Assert
            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}