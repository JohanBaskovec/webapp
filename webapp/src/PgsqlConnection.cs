using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Npgsql;
using Npgsql.TypeMapping;
using IsolationLevel = System.Data.IsolationLevel;

namespace BlogApp.Framework
{
    public class PgsqlConnection
    {
        private readonly NpgsqlConnection _npgsqlConnection;

        public PgsqlConnection(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection;
        }

        public object GetLifetimeService()
        {
            return _npgsqlConnection.GetLifetimeService();
        }

        public object InitializeLifetimeService()
        {
            return _npgsqlConnection.InitializeLifetimeService();
        }

        public void Dispose()
        {
            _npgsqlConnection.Dispose();
        }

        public IContainer Container => _npgsqlConnection.Container;

        public ISite Site
        {
            get => _npgsqlConnection.Site;
            set => _npgsqlConnection.Site = value;
        }

        public event EventHandler Disposed
        {
            add => _npgsqlConnection.Disposed += value;
            remove => _npgsqlConnection.Disposed -= value;
        }

        public Task OpenAsync()
        {
            return _npgsqlConnection.OpenAsync();
        }

        public event StateChangeEventHandler StateChange
        {
            add => _npgsqlConnection.StateChange += value;
            remove => _npgsqlConnection.StateChange -= value;
        }

        public void Open()
        {
            _npgsqlConnection.Open();
        }

        public Task OpenAsync(CancellationToken cancellationToken)
        {
            return _npgsqlConnection.OpenAsync(cancellationToken);
        }

        public NpgsqlCommand CreateCommand()
        {
            return _npgsqlConnection.CreateCommand();
        }

        public NpgsqlTransaction BeginTransaction()
        {
            return _npgsqlConnection.BeginTransaction();
        }

        public NpgsqlTransaction BeginTransaction(IsolationLevel level)
        {
            return _npgsqlConnection.BeginTransaction(level);
        }

        public void EnlistTransaction(Transaction transaction)
        {
            _npgsqlConnection.EnlistTransaction(transaction);
        }

        public void Close()
        {
            _npgsqlConnection.Close();
        }

        public NpgsqlBinaryImporter BeginBinaryImport(string copyFromCommand)
        {
            return _npgsqlConnection.BeginBinaryImport(copyFromCommand);
        }

        public NpgsqlBinaryExporter BeginBinaryExport(string copyToCommand)
        {
            return _npgsqlConnection.BeginBinaryExport(copyToCommand);
        }

        public TextWriter BeginTextImport(string copyFromCommand)
        {
            return _npgsqlConnection.BeginTextImport(copyFromCommand);
        }

        public TextReader BeginTextExport(string copyToCommand)
        {
            return _npgsqlConnection.BeginTextExport(copyToCommand);
        }

        public NpgsqlRawCopyStream BeginRawBinaryCopy(string copyCommand)
        {
            return _npgsqlConnection.BeginRawBinaryCopy(copyCommand);
        }

        public void MapEnum<TEnum>(string pgName = null, INpgsqlNameTranslator nameTranslator = null) where TEnum : struct
        {
            _npgsqlConnection.MapEnum<TEnum>(pgName, nameTranslator);
        }

        public void MapComposite<T>(string pgName = null, INpgsqlNameTranslator nameTranslator = null) where T : new()
        {
            _npgsqlConnection.MapComposite<T>(pgName, nameTranslator);
        }

        public bool Wait(int timeout)
        {
            return _npgsqlConnection.Wait(timeout);
        }

        public bool Wait(TimeSpan timeout)
        {
            return _npgsqlConnection.Wait(timeout);
        }

        public void Wait()
        {
            _npgsqlConnection.Wait();
        }

        public Task WaitAsync(CancellationToken cancellationToken)
        {
            return _npgsqlConnection.WaitAsync(cancellationToken);
        }

        public Task WaitAsync()
        {
            return _npgsqlConnection.WaitAsync();
        }

        public DataTable GetSchema()
        {
            return _npgsqlConnection.GetSchema();
        }

        public DataTable GetSchema(string collectionName)
        {
            return _npgsqlConnection.GetSchema(collectionName);
        }

        public DataTable GetSchema(string collectionName, string[] restrictions)
        {
            return _npgsqlConnection.GetSchema(collectionName, restrictions);
        }

        public void ChangeDatabase(string dbName)
        {
            _npgsqlConnection.ChangeDatabase(dbName);
        }

        public void UnprepareAll()
        {
            _npgsqlConnection.UnprepareAll();
        }

        public void ReloadTypes()
        {
            _npgsqlConnection.ReloadTypes();
        }

        public INpgsqlTypeMapper TypeMapper => _npgsqlConnection.TypeMapper;

        public string ConnectionString
        {
            get => _npgsqlConnection.ConnectionString;
            set => _npgsqlConnection.ConnectionString = value;
        }

        public string Host => _npgsqlConnection.Host;

        public int Port => _npgsqlConnection.Port;

        public int ConnectionTimeout => _npgsqlConnection.ConnectionTimeout;

        public int CommandTimeout => _npgsqlConnection.CommandTimeout;

        public string Database => _npgsqlConnection.Database;

        public string DataSource => _npgsqlConnection.DataSource;

        public bool IntegratedSecurity => _npgsqlConnection.IntegratedSecurity;

        public string UserName => _npgsqlConnection.UserName;

        public ConnectionState FullState => _npgsqlConnection.FullState;

        public ConnectionState State => _npgsqlConnection.State;

        public ProvideClientCertificatesCallback ProvideClientCertificatesCallback
        {
            get => _npgsqlConnection.ProvideClientCertificatesCallback;
            set => _npgsqlConnection.ProvideClientCertificatesCallback = value;
        }

        public RemoteCertificateValidationCallback UserCertificateValidationCallback
        {
            get => _npgsqlConnection.UserCertificateValidationCallback;
            set => _npgsqlConnection.UserCertificateValidationCallback = value;
        }

        public Version PostgreSqlVersion => _npgsqlConnection.PostgreSqlVersion;

        public string ServerVersion => _npgsqlConnection.ServerVersion;

        public int ProcessID => _npgsqlConnection.ProcessID;

        public bool HasIntegerDateTimes => _npgsqlConnection.HasIntegerDateTimes;

        public string Timezone => _npgsqlConnection.Timezone;

        public IReadOnlyDictionary<string, string> PostgresParameters => _npgsqlConnection.PostgresParameters;

        public event NoticeEventHandler Notice
        {
            add => _npgsqlConnection.Notice += value;
            remove => _npgsqlConnection.Notice -= value;
        }

        public event NotificationEventHandler Notification
        {
            add => _npgsqlConnection.Notification += value;
            remove => _npgsqlConnection.Notification -= value;
        }

        public NpgsqlConnection NpgsqlConnection
        {
            get { return _npgsqlConnection; }
        }
    }

}