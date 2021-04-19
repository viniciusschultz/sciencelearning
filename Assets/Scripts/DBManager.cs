public static class DBManager
{
    public static string username;
    public static int score;
    public static int idUsuario;

    public static bool LoggedIn { get { return username != null; } }

    public static void LogOut()
    {
        username = null;
    }

}
