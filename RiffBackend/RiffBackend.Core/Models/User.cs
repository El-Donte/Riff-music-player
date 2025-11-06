namespace RiffBackend.Core.Models
{
    public class User
    {
        const int MAX_NAME_LENGTH = 250;

        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Email { get; private set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public string AvatarUrl { get; private set; } = string.Empty;

        private User(Guid id, string name, string email, string password, string avatarUrl)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            AvatarUrl = avatarUrl;
        }

        public static (User user, string Error) Create(Guid id, string name, string email, string password, string avatarUrl)
        {
            string error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Name cant be empty or longer 250 symbols";
            }

            //TO-DO переработать валидацию
            
            User user = new User(id, name, email, password, avatarUrl);

            return (user, error);
        }
    }
}
