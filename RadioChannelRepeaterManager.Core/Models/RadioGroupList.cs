using System.Text.Json;

namespace RadioChannelRepeaterManager.Core.Models;

public class RadioGroupList
{
    public Dictionary<uint, RadioGroup> Groups
    {
        get;
        set;
    } = [];

    /// <summary>
    /// Converts this instance to an indented JSON string.
    /// </summary>
    /// <returns>A JSON string representing the current <see cref="RadioGroupList"/>.</returns>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    /// <summary>
    /// Updates this instance from a JSON string.
    /// </summary>
    /// <param name="json">A JSON string containing a serialized <see cref="RadioGroupList"/>.</param>
    /// <returns><see langword="true"/> if deserialization succeeds and values are applied; otherwise, <see langword="false"/>.</returns>
    public bool FromJson(string json)
    {
        try
        {
            var deserialized = JsonSerializer.Deserialize<RadioGroupList>(json);
            if (deserialized is not null)
            {
                Groups = deserialized.Groups;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}
