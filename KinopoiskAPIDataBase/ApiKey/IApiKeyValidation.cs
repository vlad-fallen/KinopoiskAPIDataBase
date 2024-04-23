namespace KinopoiskAPIDataBase.ApiKey
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string userApiKey);
    }
}
