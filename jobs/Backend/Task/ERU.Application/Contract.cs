using System.Diagnostics.CodeAnalysis;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;

namespace ERU.Application;

public static class Contract
{
	[return: NotNull]
	public static TTarget Check<TDto, TTarget>(TDto? dto, IContractObjectMapper<TDto, TTarget> mapper, string errorMessage)
	{
		bool validProperties = dto?.GetType().GetProperties().Any(p => p.GetValue(dto) is not null) ?? false;
		return validProperties ? mapper.Map<TDto, TTarget>(dto!) : throw new ContractException(errorMessage);
	}
}