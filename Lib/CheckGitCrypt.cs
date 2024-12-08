using System.Text;

internal class GitCrypt
{
    public static void Check()
    {
        if (!File.Exists(".git/git-crypt/keys/default"))
        {
            Console.WriteLine("""
                Repository is not unlocked. You need to install and configure git-crypt.
                Check out the README file.
                """);
            Environment.Exit(1);
        }
    }

    public static void CheckFile(string file)
    {
        using var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        fileStream.Seek(1, SeekOrigin.Begin);
        var buffer = new byte[8];
        var bytesRead = fileStream.Read(buffer, 0, buffer.Length);

        // If the file is smaller than the requested byte count, resize the array
        if (bytesRead < buffer.Length)
        {
            return;
        }

        if (Encoding.ASCII.GetBytes("GITCRYPT").SequenceEqual(buffer))
        {
            Console.WriteLine($"""
                File '{file}' is encrypted. You need to install and configure git-crypt.
                Check out the README file.
                """);
            Environment.Exit(1);
        }
    }
}
