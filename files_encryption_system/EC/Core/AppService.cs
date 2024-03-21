namespace EC.Core
{
    internal class AppService
    {
        private readonly UsersRepository _repository = new();
        private readonly UsersDirectoriesManager _manager = new();
        private readonly HashComputator _hashComputator = new();

        public static AppService Instance { get; } = new AppService();

        internal void CreateNewUser(string username, string password)
        {
            var passwordHash = new HashComputator().ComputeHash(password);
            User newUser = new User(username, passwordHash);
            _repository.CreateNewUser(newUser);
        }

        internal void LogoutUser(string userName)
        {
            _manager.LockUserDirectory(userName);
        }

        internal bool UserExist(string login)
        {
            return _repository.UserExist(login);
        }

        internal bool TryLogin(string username, string password, out string userDirectoryPath)
        {
            userDirectoryPath = string.Empty;
            if (!_repository.UserExist(username))
            {
                return false;
            }
            var passwordHash = _hashComputator.ComputeHash(password);
            if(_repository.UserLoginDataCorrect(username, passwordHash))
            {
                userDirectoryPath = _manager.UnlockUserDirectory(username);
                return true;
            }
            return false;
        }
    }
}
