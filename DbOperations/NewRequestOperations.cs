using System.Data;
using Project0.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
public class NewRequestOperations
{
    P0UsmanBankingDbContext db = new P0UsmanBankingDbContext();

    //Method to approve a service request by the request ID
    public void ApproveRequest(int request_id)
    {
        try
        {
        //Find the request using its ID
        var request = db.NewServiceRequests.Find(request_id);
        if(request != null)
        {
            //Update the request status to Approved and save changes
            request.RequestStatus = "Approved";
            db.SaveChanges();
            Console.WriteLine("Request approved");
        }
        else
        {
            Console.WriteLine("Request not found");
        }
        }
        catch (Exception ex)
        {
            //Handle errors 
            Console.WriteLine("There was an error approving the request: " + ex.Message);
        }
    }

    //Method to reject a service request by its ID
    public void RejectRequest(int request_id)
    {
        try
        {
        var request = db.NewServiceRequests.Find(request_id);
        if(request != null)
        {
            //Update the request status to Rejected and save changes
            request.RequestStatus = "Rejected";
            db.SaveChanges();
            Console.WriteLine("Request rejected");
        }
        else
        {
            Console.WriteLine("Request not found");
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was an error rejecting the request: " + ex.Message);
        }
    }

    //Method to view all pending service requests
    public void ViewPendingRequests()
    {
        try
        {
        //Get all requests with a status of Pending
        var pendingRequests = db.NewServiceRequests
            .Where(r => r.RequestStatus == "Pending")
            .ToList();

        //Display the details of each pending request
        foreach(var request in pendingRequests)
        {
            Console.WriteLine($"Request ID: {request.RequestId}, Account No: {request.AccNo}, Service Type: {request.ServiceType}, Status: {request.RequestStatus}, Date: {request.RequestDate}");
        }
        }
        catch (Exception ex)
        {
            Console.WriteLine("There was an error viewing pending requests: " + ex.Message);

        }
    }
}