namespace Modelo.API.Models
{
    public class Token
    {
        public string AccessToken { get; set; }

        public double ExpiresIn { get; set; }
    }
}