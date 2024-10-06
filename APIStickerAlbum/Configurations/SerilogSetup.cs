using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace APIStickerAlbum.Configurations;

public class SerilogSetup
{
    public static void ConfigureSerilog(IConfiguration configuration) 
    {
        var columnOptions = new ColumnOptions();
        columnOptions.Store.Add(StandardColumn.LogEvent);
        columnOptions.TimeStamp.ColumnName = "Timestamp";
        columnOptions.AdditionalColumns = new List<SqlColumn>
        {
            new SqlColumn("MachineName", SqlDbType.VarChar),
            new SqlColumn("ExceptionType", SqlDbType.VarChar),
            new SqlColumn("UserName", SqlDbType.VarChar)
        };

        Log.Logger = new LoggerConfiguration()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Console()
            .WriteTo.MSSqlServer(
                connectionString: configuration.GetConnectionString("StickerAlbum:SqlDb"),
                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true },
                columnOptions: columnOptions,
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
            .CreateLogger();
    }
}
