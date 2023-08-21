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
        private const string InfrastructureMockyNamespace = "PoqCommerce.Mocky.Io";
        private const string InfrastructurePersistenceNamespace = "PoqCommerce.Persistence";
        private const string InfrastructurePersistenceEfNamespace = "PoqCommerce.Persistence.EF";
        private const string SharedNamespace = "Persistence.Abstraction";

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(DomainNamespace);

            var otherProjects = new[] {
                ApplicationNamespace,
                PresentationNamespace,
                InfrastructureMockyNamespace,
                InfrastructurePersistenceNamespace,
                InfrastructurePersistenceEfNamespace,
                SharedNamespace
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
                InfrastructureMockyNamespace,
                InfrastructurePersistenceNamespace,
                InfrastructurePersistenceEfNamespace,
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
        public void Infrastructure_Mocky_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(InfrastructureMockyNamespace);

            var otherProjects = new[] {
                PresentationNamespace,
                DomainNamespace,
                InfrastructurePersistenceNamespace,
                InfrastructurePersistenceEfNamespace
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
        public void Infrastructure_PersistenceEf_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(InfrastructurePersistenceEfNamespace);

            var otherProjects = new[] {
                PresentationNamespace,
                DomainNamespace,
                InfrastructureMockyNamespace,
                InfrastructurePersistenceNamespace,
            };

            var referenceProjects = new[] {
                SharedNamespace,
                ApplicationNamespace
            };

            // Act
            var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAll(otherProjects)
            .And().HaveDependencyOnAll(referenceProjects)
            .GetResult();

            //Assert
            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(InfrastructurePersistenceNamespace);

            var otherProjects = new[] {
                PresentationNamespace,
                DomainNamespace,
                InfrastructureMockyNamespace,
                SharedNamespace,
                ApplicationNamespace
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
        public void NuGet_Persistence_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange
            Assembly assembly = Assembly.Load(InfrastructurePersistenceNamespace);

            var otherProjects = new[] {
                ApplicationNamespace,
                DomainNamespace,
                PresentationNamespace,
                InfrastructureMockyNamespace,
                InfrastructurePersistenceNamespace,
                InfrastructurePersistenceEfNamespace,
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