namespace ERU.Application.Interfaces;

public interface IDataStringParser<out T>
{
	T Parse(string input);
}