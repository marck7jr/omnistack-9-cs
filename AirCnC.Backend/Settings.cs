namespace AirCnC.Backend
{
    /// <summary>
    /// Usar dotnet user-secrets para definir seus valores do arquivo JSON no seguinte modelo:
    /// dotnet user-secrets set "AirCnC:[chave]" "[valor]"
    /// </summary>
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string DbPassword { get; set; }
    }
}
