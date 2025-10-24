using System.Data;

namespace DynAmino.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}