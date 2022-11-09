using System.Diagnostics.CodeAnalysis;

namespace ERU.Application.Interfaces;

public interface IContractObjectMapper<in TSourceObject, in TTargetObject>
{
	[return: NotNull]
	public TResult Map<TInput, TResult>(TInput inputObject) where TInput : TSourceObject where TResult : TTargetObject;
}