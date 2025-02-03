using Microsoft.AspNetCore.Mvc;
using termPaper.Data;
using termPaper.Models;

[ApiController]
[Route("api/[controller]")]
public class SubscriptionProcessorController : ControllerBase
{
    [HttpPost("GetInformationByActNumberDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<InformationByActNumberDateResponse> GetInformationByActNumberDate([FromBody] List<ActDetailRequest> requests)
    {
        var response = new InformationByActNumberDateResponse();
        foreach (var request in requests)
        {

            try
            {
                if (string.IsNullOrEmpty(request.SubscrptionId))
                {
                    return Unauthorized("Unauthorized access: SubscriptionID is missing.");
                }

                if (request.SiteID == "invalid")
                {
                    return StatusCode(403, "Access is denied. Please check the certificate.");
                }

                var matchingDocuments = MockDatabase.Documents
                    .Where(doc => doc.RegisterNumber == request.DocumentNumber &&
                                  doc.RegisterDate.Date == request.DocumentDate.Date &&
                                  doc.SiteCode == request.SiteID)
                    .ToList();

                response.Documents.AddRange(matchingDocuments);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return StatusCode(500, "Unexpected error occurred. Please try again later.");
            }
        }
        return Ok(response);
    }

    [HttpPost("AddDocument")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult AddDocument([FromBody] Document.Document newDocument)
    {
        try
        {
            if (newDocument == null)
            {
                return BadRequest("Document cannot be null.");
            }

            if (string.IsNullOrEmpty(newDocument.SiteCode) ||
                string.IsNullOrEmpty(newDocument.RegisterNumber) ||
                newDocument.RegisterDate == default)
            {
                return BadRequest("Missing required document fields: SiteCode, RegisterNumber, or RegisterDate.");
            }

            MockDatabase.Documents.Add(newDocument);

            return Ok("Document added successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }


    [HttpDelete("DeleteDocumentBySiteCode/{siteCode}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult DeleteDocumentBySiteCode(string siteCode)
    {
        try
        {
            if (string.IsNullOrEmpty(siteCode))
            {
                return BadRequest("SiteCode cannot be null or empty.");
            }

            var documentToDelete = MockDatabase.Documents.FirstOrDefault(doc => doc.SiteCode == siteCode);

            if (documentToDelete == null)
            {
                return NotFound("Document with the provided SiteCode does not exist.");
            }

            MockDatabase.Documents.Remove(documentToDelete);

            return Ok("Document deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    [HttpGet("GetAllDocuments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetAllDocuments()
    {
        try
        {
            var documents = MockDatabase.Documents.ToList();

            if (documents.Count == 0)
            {
                return NotFound("No documents found.");
            }

            return Ok(documents);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }

    }

    [HttpPatch("UpdateDocument")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult UpdateDocument([FromBody] UpdateRequest updateRequest)
    {
        try
        {
            if (updateRequest == null)
            {
                return BadRequest("UpdateRequest cannot be null.");
            }

            var documentToUpdate = MockDatabase.Documents.FirstOrDefault(doc => doc.SiteCode == updateRequest.SiteCode);

            if (documentToUpdate == null)
            {
                return NotFound("Document with the provided SiteCode does not exist.");
            }

            documentToUpdate.RegisterNumber = updateRequest.RegisterNumber;
            documentToUpdate.Book = updateRequest.Book;
            documentToUpdate.Fee = updateRequest.Fee;

            return Ok("Document updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }
}