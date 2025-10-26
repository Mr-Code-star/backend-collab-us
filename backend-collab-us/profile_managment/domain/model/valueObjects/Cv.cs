namespace backend_collab_us.profile_managment.domain.model.valueObjects;

public class CV : IEquatable<CV>
{
    public string FileName { get; }
    public string FileType { get; }
    public long FileSize { get; }
    public string Data { get; } // Base64 string
    public DateTime UploadedAt { get; }

    // Constructor principal
    public CV(string fileName, string fileType, long fileSize, string data)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required");
        
        if (string.IsNullOrWhiteSpace(data))
            throw new ArgumentException("File data is required");

        FileName = fileName;
        FileType = fileType;
        FileSize = fileSize;
        Data = data;
        UploadedAt = DateTime.UtcNow;
    }

    // Constructor para reconstruir desde base de datos
    private CV(string fileName, string fileType, long fileSize, string data, DateTime uploadedAt)
    {
        FileName = fileName;
        FileType = fileType;
        FileSize = fileSize;
        Data = data;
        UploadedAt = uploadedAt;
    }

    // Factory method para crear desde Base64
    public static CV CreateFromBase64(string fileName, string fileType, long fileSize, string base64Data)
    {
        // Validar que sea base64 válido
        if (!IsValidBase64(base64Data))
            throw new ArgumentException("Invalid base64 data");

        return new CV(fileName, fileType, fileSize, base64Data);
    }

    // Factory method para crear desde bytes
    public static CV CreateFromBytes(string fileName, string fileType, byte[] fileData)
    {
        var base64Data = Convert.ToBase64String(fileData);
        return new CV(fileName, fileType, fileData.Length, base64Data);
    }

    // Métodos de utilidad
    public byte[] GetBytes()
    {
        return Convert.FromBase64String(Data);
    }

    public string GetMimeType()
    {
        return FileType.ToLower() switch
        {
            "application/pdf" => "application/pdf",
            "application/msword" => "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            _ => "application/octet-stream"
        };
    }

    public string GetFileExtension()
    {
        return FileType.ToLower() switch
        {
            "application/pdf" => ".pdf",
            "application/msword" => ".doc",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
            _ => Path.GetExtension(FileName)
        };
    }

    public bool IsPdf()
    {
        return FileType == "application/pdf";
    }

    public bool IsWordDocument()
    {
        return FileType == "application/msword" || 
               FileType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
    }

    // Validación
    private static bool IsValidBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64) || base64.Length % 4 != 0)
            return false;

        try
        {
            Convert.FromBase64String(base64);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // Equality implementation
    public bool Equals(CV? other)
    {
        if (other is null) return false;
        return FileName == other.FileName && 
               FileType == other.FileType && 
               FileSize == other.FileSize && 
               Data == other.Data;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as CV);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(FileName, FileType, FileSize, Data);
    }

    // Para reconstruir desde Entity Framework
    public static CV FromDatabase(string fileName, string fileType, long fileSize, string data, DateTime uploadedAt)
    {
        return new CV(fileName, fileType, fileSize, data, uploadedAt);
    }
}