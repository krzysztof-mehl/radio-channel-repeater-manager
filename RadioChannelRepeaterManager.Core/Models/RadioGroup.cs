using RadioChannelRepeaterManager.Core.Utilities;
using System.Text.Json;

namespace RadioChannelRepeaterManager.Core.Models;

public class RadioGroup
{
    /// <summary>
    /// Represents the maximum allowed length for Name.
    /// </summary>
    private const byte _nameMaxLength = 16;

    /// <summary>
    /// Represents the name of the radio configuration, can be empty.
    /// </summary>
    public string Name
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            value = value.TrimEnd();

            if (value.Length > _nameMaxLength)
            {
                throw new ArgumentException($"Name must be at most {_nameMaxLength} characters long. Actual value: '{value}'.", nameof(Name));
            }

            if (!StringUtilities.IsPrintableString(value))
            {
                throw new ArgumentException($"Name can only contain printable characters. Actual value: '{value}'.", nameof(Name));
            }

            field = value;
        }
    } = string.Empty;

    /// <summary>
    /// Represents the collection of radio configurations.
    /// </summary>
    public Dictionary<uint, RadioConfiguration> Items
    {
        get;
        set;
    } = [];

    /// <summary>
    /// Converts this instance to an indented JSON string.
    /// </summary>
    /// <returns>A JSON string representing the current <see cref="RadioGroup"/>.</returns>
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
    /// <param name="json">A JSON string containing a serialized <see cref="RadioGroup"/>.</param>
    /// <returns><see langword="true"/> if deserialization succeeds and values are applied; otherwise, <see langword="false"/>.</returns>
    public bool FromJson(string json)
    {
        try
        {
            var deserialized = JsonSerializer.Deserialize<RadioGroup>(json);
            if (deserialized is not null)
            {
                Name = deserialized.Name;
                Items = deserialized.Items;
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
