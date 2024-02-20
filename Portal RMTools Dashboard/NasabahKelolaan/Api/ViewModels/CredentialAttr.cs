namespace Api.ViewModels
{
    public class CredentialAttr
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
    }

    public class DecryptUID
    {
        public int Id { get; set; }

        public bool status { get; set; }

        public string message { get; set; }
    }
}
