using System.Data;
using Dapper;

namespace Wrecept.Infrastructure;

public sealed class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return value switch
        {
            byte[] bytes => new Guid(bytes),
            string str => Guid.Parse(str),
            Guid g => g,
            _ => Guid.Parse(value.ToString() ?? string.Empty)
        };
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.DbType = DbType.String;
        parameter.Value = value.ToString();
    }
}
