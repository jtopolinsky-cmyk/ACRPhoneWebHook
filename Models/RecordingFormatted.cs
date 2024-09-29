namespace ACRPhoneWebHook.Models
{
    public class RecordingFormatted
    {

        public long Id { get; set; }

        public required string Source { get; set; }


        public required string FileName { get; set; }

        public required string DownloadPath { get; set; }


        public required string Note { get; set; }


        public required string Date { get; set; }


        public required string FileSize { get; set; }


        public required string Duration { get; set; }

    }

}
