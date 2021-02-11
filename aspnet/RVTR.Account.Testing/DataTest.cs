using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RVTR.Account.Context;

namespace RVTR.Account.Testing
{
  public abstract class DataTest : IDisposable
  {
    private readonly SqliteConnection _connection;
    protected readonly DbContextOptions<AccountContext> Options;
    private bool _disposedValue;

    protected DataTest()
    {
      _connection = new SqliteConnection("Data Source=:memory:");
      _connection.Open();
      Options = new DbContextOptionsBuilder<AccountContext>()
        .UseSqlite(_connection)
        .Options;

      using var ctx = new AccountContext(Options);
      ctx.Database.EnsureCreated();
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposedValue)
      {
        if (disposing)
        {
          _connection.Dispose();
        }
        _disposedValue = true;
      }
    }

    public void Dispose()
    {
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
