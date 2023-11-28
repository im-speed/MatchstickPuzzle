using System.Xml.Serialization;

namespace MatchstickPuzzle.Classes;
internal class AppDataHandler
{
    private readonly string folderName;

    /// <summary>
    /// Creates a new AppData folder if it did not yet exist.
    /// </summary>
    /// <param name="folderName">The name of the folder to create in local app data</param>
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

    /// <summary>
    /// Writes an object to an xml file in the programs app data folder.
    /// </summary>
    /// <typeparam name="T">Type of the object to write.</typeparam>
    /// <param name="path">Relative path in the programs app data folder.</param>
    /// <param name="objectToWrite">The object to serialize and write to file.</param>
    public void WriteToXmlFile<T>(string path, T objectToWrite) where T : new()
    {
        XmlSerializer serializer = new(typeof(T));
        using StreamWriter writer = new(FullPath(path));
        serializer.Serialize(writer, objectToWrite);
    }

    /// <summary>
    /// Reads an xml file and tries to deserialize it.
    /// </summary>
    /// <typeparam name="T">Type of the object to try and deserialize to.</typeparam>
    /// <param name="path">Relative path in the programs app data folder.</param>
    /// <returns>The deserialized object or null.</returns>
    public T? ReadFromXmlFile<T>(string path) where T : new()
    {
        XmlSerializer serializer = new(typeof(T));
        using StreamReader reader = new(FullPath(path));
        return (T?)serializer.Deserialize(reader);
    }
}
