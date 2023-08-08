using AutoFixture;
using AutoMapper;
using FluentAssertions;
using System.Reflection;

namespace Mews.ExchangeRate.API.UnitTests.Mappers;
public class ContractToDomainProfileTests
{
    private readonly IMapper _sut;
    private readonly Fixture _fixture;

    public ContractToDomainProfileTests() 
    {
        _fixture = new Fixture();

        var assemblyName = typeof(API.Mappers.DtoToDomainProfile).Assembly.GetName();
        var configuration = new MapperConfiguration(cfg => cfg.AddMaps(Assembly.Load(assemblyName)));
        _sut = new Mapper(configuration);
    }

    [Fact]
    public void GivenACurrencyDto_Mapper_MapsToCurrencyDomainObject()
    {
        var dto = new API.Dtos.Currency("EUR");

        var result = _sut.Map<Domain.Currency>(dto);

        result.Code
            .Should()
            .Be(dto.Code);
    }
}
