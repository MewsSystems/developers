namespace ERU.Domain;

public record Currency(in string Code)
{
	public override string ToString()
	{
		return Code;
	}
}