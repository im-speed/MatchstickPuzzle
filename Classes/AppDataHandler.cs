using System.Xml.Serialization;

namespace MatchstickPuzzle.Classes;
internal class AppDataHandler
{
    private readonly string folderName;

    public AppDataHandler(string folderName)
    {
        this.folderName = folderName;

        if (!new FileInfo(FullPath()).Exists)
        {
            Directory.CreateDirectory(FullPath());
        }
    }

    private string FullPath()
        => FullPath("");

    private string FullPath(string path)
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        return Path.Combine(appDataPath, folderName, path);
    }

    public void WriteToXmlFile<T>(string path, T objectToWrite) where T : new()
    {
        XmlSerializer serializer = new(typeof(T));
        using StreamWriter writer = new(FullPath(path));
        serializer.Serialize(writer, objectToWrite);
    }

    public T? ReadFromXmlFile<T>(string path) where T : new()
    {
        XmlSerializer serializer = new(typeof(T));
        using StreamReader reader = new(FullPath(path));
        return (T?)serializer.Deserialize(reader);
    }
}
