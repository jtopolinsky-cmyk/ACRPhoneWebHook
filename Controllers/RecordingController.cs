using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ACRPhone.Webhook.Authentication;
using ACRPhone.Webhook.Models;
using ACRPhone.Webhook.Repositories;
using ACRPhone.Webhook.ViewModels;
using ACRPhoneWebHook.Models;
using Microsoft.Extensions.Logging;


namespace ACRPhone.Webhook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordingController(ILogger<RecordingController> logger, IRecordingRepository recordingRepository, AppSettings.AppSettings appSettings) : ControllerBase
    {


        [Authorize(AuthenticationSchemes = CustomAuthOptions.DefaultScheme)]
        [HttpPost("all")]
        public IEnumerable<RecordingFormatted> GetAll()
        {
            return recordingRepository
                .GetAll()
                .Select(item => item.AsFormattedRecording(appSettings.UploadPath));

        }

        [HttpGet("{id}", Name = "GetRecording")]
        public ActionResult<Recording> Get(long id)
        {
            var recording = recordingRepository.GetById(id);
            if (recording == null)
            {
                return NotFound();
            }

            return recording;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Recording model)
        {

            logger.LogDebug($"{nameof(Create)} -> Create() -> {model}");

            if (model == null)
            {
                return BadRequest();
            }

            recordingRepository.Add(model);
            recordingRepository.Save();

            return CreatedAtRoute("GetRecording", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Recording model)
        {
            logger.LogDebug($"{nameof(Update)} -> Update() -> {model}");

            if (model == null || model.Id != id)
            {
                return BadRequest();
            }

            var recording = recordingRepository.GetById(id);
            if (recording == null)
            {
                return NotFound();
            }

            recording.Note = model.Note;

            recordingRepository.Update(recording);
            recordingRepository.Save();

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            logger.LogDebug($"{nameof(Delete)} -> Delete() -> {id}");

            var recording = recordingRepository.GetById(id);
            if (recording == null)
            {
                return NotFound();
            }

            recordingRepository.Delete(recording);
            recordingRepository.Save();

            return new NoContentResult();
        }


        [HttpPost("upload")]
        [RequestFormLimits(MultipartBodyLengthLimit = 2147483647)]
        public async Task<IActionResult> Upload([FromForm] UploadRecordViewModel model)
        {
            logger.LogDebug($"{nameof(Upload)} -> Upload() -> {model}");

            if (!string.IsNullOrWhiteSpace(model.Secret) && model.Secret != appSettings.UserCredentials.Secret)
            {
                return StatusCode(401, "User with specified secret " + model.Secret + " doesn't exist");
            }

            try
            {
                var length = 0L;
                var safeFileName = "";

                if (model.File != null)
                {
                    var file = model.File;
                    length = file.Length;
                    safeFileName = SafeFileName(file.FileName);
                    var filePath = Path.Combine(GetUploadPath(), safeFileName);

                    if (length == 0)
                    {
                        return BadRequest("Expected at least 1 byte, but got 0");
                    }

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                var recording = new Recording
                {
                    Source = model.Source,
                    FileName = safeFileName,
                    Note = model.Note ?? "",
                    Date = model.Date ?? DateTimeOffset.Now.ToUnixTimeSeconds(),
                    FileSize = length,
                    Duration = model.Duration ?? 0,
                };
                logger.LogDebug($"{nameof(Upload)} -> Upload() -> {recording}");

                recordingRepository.Add(recording);
                recordingRepository.Save();

                logger.LogDebug($"{nameof(Upload)} -> Upload() -> Ok");
                return Ok();
            }
            catch (Exception e)
            {
                var message = "Error while uploading recording";

                logger.LogError(e, message);

                return StatusCode(500, message);
            }
        }

        private static string GetUploadPath()
        {
            return "uploads";
        }

        private static string SafeFileName(string fileName)
        {

            /* if (fileName.Contains(@"\"))
                 fileName = fileName.Substring(fileName.LastIndexOf(@"\", StringComparison.Ordinal) + 1);

             return fileName;*/

            return string.Join("_", fileName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');

        }
    }
}