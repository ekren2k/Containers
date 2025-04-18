
using Containers.Models;
using Microsoft.Data.SqlClient;

namespace Containers.Application;

public class ContainerService
{
    
    private string connectionString;

    public ContainerService(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public IEnumerable<Container> GetAllContainers()
    {
        List<Container> containers = new List<Container>();
        const string queryString = "SELECT * FROM Containers";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(queryString, connection);
            
            connection.Open();
            
            SqlDataReader reader = command.ExecuteReader();

            try
            {
                if (reader.HasRows)
                {
                    var containerRow = new Container()
                    {
                        ID = reader.GetInt32(0),
                        ContainerTypeId = reader.GetInt32(1),
                        IsHazardous = reader.GetBoolean(2),
                        Name = reader.GetString(3),
                    };
                    containers.Add(containerRow);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        return containers;
    }
    
}