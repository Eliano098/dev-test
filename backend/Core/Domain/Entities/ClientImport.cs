using Domain.Entities;

namespace Domain
{
    public class ClientImport : BaseEntity
    {
        public string FileName { get; private set; }
        public string FilePath { get; private set; }
        public ClientImportStatus Status { get; private set; }
        public int ProcessedRows { get; private set; }
        public int SuccessfulRows { get; private set; }
        public int FailedRows { get; private set; }
        public string ErrorMessage { get; private set; }

        public ClientImport() { }

        public ClientImport(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
            Status = ClientImportStatus.Pending;
        }

        public void Start()
        {
            Status = ClientImportStatus.Processing;
            ErrorMessage = null;
        }

        public void RegisterSuccess()
        {
            ProcessedRows++;
            SuccessfulRows++;
        }

        public void RegisterFailure()
        {
            ProcessedRows++;
            FailedRows++;
        }

        public void Complete()
        {
            Status = ClientImportStatus.Completed;
        }

        public void Fail(string errorMessage)
        {
            Status = ClientImportStatus.Failed;
            ErrorMessage = errorMessage;
        }
    }
}
