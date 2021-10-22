using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public interface IGenericEntity
    {
        IEnumerable<GenericRate> ToGenericEntity();
    }
}