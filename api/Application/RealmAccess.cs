using System.Text.Json.Serialization;

namespace Reports.Controllers;

public class RealmAccess
{
    [JsonPropertyName("roles")]
    public List<string> Roles { get; set; } = new ();
}
