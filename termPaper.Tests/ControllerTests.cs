using Xunit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Document;
using termPaper.Data;
using termPaper.Models;

public class DocumentControllerTests
{
    public DocumentControllerTests()
    {
        MockDatabase.Documents = new List<Document.Document>();
    }

    [Fact]
    public void AddDocument_ShouldReturnOk_WhenDocumentIsValid()
    {
        var controller = new SubscriptionProcessorController();
        var newDocument = new Document.Document
        {
            SiteCode = "SC123",
            RegisterNumber = "RN456",
            RegisterDate = DateTime.Now
        };

        var result = controller.AddDocument(newDocument);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void AddDocument_ShouldReturnBadRequest_WhenDocumentIsNull()
    {
        var controller = new SubscriptionProcessorController();

        var result = controller.AddDocument(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DeleteDocumentBySiteCode_ShouldReturnOk_WhenDocumentExists()
    {
        var controller = new SubscriptionProcessorController();
        var existingDocument = new Document.Document { SiteCode = "SC123", RegisterNumber = "RN789", RegisterDate = DateTime.Now };
        MockDatabase.Documents.Add(existingDocument);

        var result = controller.DeleteDocumentBySiteCode("SC123");

        Assert.IsType<OkObjectResult>(result);
        Assert.DoesNotContain(MockDatabase.Documents, d => d.SiteCode == "SC123");
    }

    [Fact]
    public void DeleteDocumentBySiteCode_ShouldReturnNotFound_WhenDocumentDoesNotExist()
    {
        var controller = new SubscriptionProcessorController();

        var result = controller.DeleteDocumentBySiteCode("SC999");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void GetAllDocuments_ShouldReturnOk_WhenDocumentsExist()
    {
        var controller = new SubscriptionProcessorController();
        MockDatabase.Documents.Add(new Document.Document { SiteCode = "SC001", RegisterNumber = "RN001", RegisterDate = DateTime.Now });

        var result = controller.GetAllDocuments();

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void GetAllDocuments_ShouldReturnNotFound_WhenNoDocumentsExist()
    {
        var controller = new SubscriptionProcessorController();
        MockDatabase.Documents.Clear();

        var result = controller.GetAllDocuments();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void UpdateDocument_ShouldUpdatePartialFields_WhenSomeFieldsAreMissing()
    {
        var controller = new SubscriptionProcessorController();
        MockDatabase.Documents.Add(new Document.Document { SiteCode = "001", RegisterNumber = "RN009", RegisterDate = DateTime.Now });
        var updateRequest = new UpdateRequest
        {
            SiteCode = "001",
            Book = "PartiallyUpdatedBook",
            RegisterNumber = "RN001",
            Fee = 100.0
        };

        var result = controller.UpdateDocument(updateRequest);

        Assert.IsType<OkObjectResult>(result);

        var updatedDocument = MockDatabase.Documents.FirstOrDefault(doc => doc.SiteCode == "001");
        Assert.Equal("RN001", updatedDocument?.RegisterNumber);
        Assert.Equal("PartiallyUpdatedBook", updatedDocument?.Book);
        Assert.Equal(100.0, updatedDocument?.Fee);
    }
}
