using System.Data;
using Dapper;

namespace HMS.Infrastructure.Persistence;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.DbType = DbType.Date;
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
    }

    public override DateOnly Parse(object value)
    {
        if (value is DateTime dt)
        {
            return DateOnly.FromDateTime(dt);
        }

        if (value is DateOnly d)
        {
            return d;
        }

        return DateOnly.Parse(value.ToString()!);
    }
}
