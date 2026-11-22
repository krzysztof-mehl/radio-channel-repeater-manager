using RadioChannelRepeaterManager.Core.Utilities;
using System.Text.Json;

namespace RadioChannelRepeaterManager.Core.Models;

public class RadioConfiguration
{
    /// <summary>
    /// Represents the maximum allowed length for Name.
    /// </summary>
    public const byte NameMaxLength = 16;

    /// <summary>
    /// Represents the minimum supported RxFrequency in MHz.
    /// </summary>
    public const decimal MinRxFrequencyMhz = 0.030m;

    /// <summary>
    /// Represents the maximum supported RxFrequency in MHz.
    /// </summary>
    public const decimal MaxRxFrequencyMhz = 10_500.0m;

    /// <summary>
    /// Represents the minimum supported TxFrequency in MHz.
    /// </summary>
    public const decimal MinTxFrequencyMhz = MinRxFrequencyMhz;

    /// <summary>
    /// Represents the maximum supported TxFrequency in MHz.
    /// </summary>
    public const decimal MaxTxFrequencyMhz = MaxRxFrequencyMhz;

    /// <summary>
    /// Represents available transmit power levels, including the default option
    /// </summary>
    public enum PowerLevels
    {
        Default = 0,
        Lowest = 1,
        Low = 2,
        Medium = 3,
        High = 4,
        Highest = 5
    }

    /// <summary>
    /// Represents available bandwidth options for FM modulation.
    /// </summary>
    public enum FmBandwidthType
    {
        Narrow = 0,
        Wide = 1
    }

    /// <summary>
    /// Represents standard CTCSS tones used for analog squelch signaling.
    /// </summary>
    public static IReadOnlyList<decimal> CtcssTones { get; } = [67.0m, 69.3m, 71.9m, 74.4m, 77.0m, 79.7m, 82.5m, 85.4m, 88.5m, 91.5m, 94.8m, 97.4m, 100.0m, 103.5m, 107.2m, 110.9m, 114.8m, 118.8m, 123.0m, 127.3m, 131.8m, 136.5m, 141.3m, 146.2m, 150.0m, 151.4m, 156.7m, 159.8m, 162.2m, 165.5m, 167.9m, 171.3m, 173.8m, 177.3m, 179.9m, 183.5m, 186.2m, 189.9m, 192.8m, 196.6m, 199.5m, 203.5m, 206.5m, 210.7m, 218.1m, 225.7m, 229.1m, 233.6m, 241.8m, 250.3m, 254.1m];

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

            if (value.Length > NameMaxLength)
            {
                throw new ArgumentException($"Name must be at most {NameMaxLength} characters long. Actual value: '{value}'.", nameof(Name));
            }

            if (!StringUtilities.IsPrintableString(value))
            {
                throw new ArgumentException($"Name can only contain printable characters. Actual value: '{value}'.", nameof(Name));
            }

            field = value;
        }
    } = string.Empty;

    /// <summary>
    /// Represents the receive frequency in MHz. A <see langword="null"/> value means receive is disabled.
    /// </summary>
    public decimal? RxFrequency
    {
        get;
        set
        {
            if (!IsFrequencyInRangeOrNull(value, MinRxFrequencyMhz, MaxRxFrequencyMhz))
            {
                throw new ArgumentOutOfRangeException(nameof(RxFrequency), value, $"RxFrequency must be between {MinRxFrequencyMhz:N6} and {MaxRxFrequencyMhz:N6} MHz.");
            }
            field = value;
        }
    } = null;

    /// <summary>
    /// Represents the transmit frequency in MHz. A <see langword="null"/> value means transmit is disabled.
    /// </summary>
    public decimal? TxFrequency
    {
        get;
        set
        {
            if (!IsFrequencyInRangeOrNull(value, MinTxFrequencyMhz, MaxTxFrequencyMhz))
            {
                throw new ArgumentOutOfRangeException(nameof(TxFrequency), value, $"TxFrequency must be between {MinTxFrequencyMhz:N6} and {MaxTxFrequencyMhz:N6} MHz.");
            }
            field = value;
        }
    } = null;

    /// <summary>
    /// Represents the transmit power level.
    /// </summary>
    public PowerLevels PowerLevel
    {
        get;
        set;
    } = PowerLevels.Default;

    /// <summary>
    /// Represents the FM bandwidth setting. A <see langword="null"/> value means FM modulation is inactive.
    /// </summary>
    public FmBandwidthType? Bandwidth
    {
        get;
        set;
    } = null;

    /// <summary>
    /// Represents the FM receive CTCSS tone. A <see langword="null"/> value means FM receive CTCSS tone is inactive.
    /// </summary>
    public decimal? FmRxCtcssTone
    {
        get;
        set
        {
            if (value is not null && !CtcssTones.Contains(value.Value))
            {
                throw new ArgumentOutOfRangeException(nameof(FmRxCtcssTone), value, $"FmRxCtcssTone must be one of the following values: {string.Join(", ", CtcssTones)}");
            }
            field = value;
        }
    } = null;

    /// <summary>
    /// Represents the FM transmit CTCSS tone. A <see langword="null"/> value means FM transmit CTCSS tone is inactive.
    /// </summary>
    public decimal? FmTxCtcssTone
    {
        get;
        set
        {
            if (value is not null && !CtcssTones.Contains(value.Value))
            {
                throw new ArgumentOutOfRangeException(nameof(FmTxCtcssTone), value, $"FmTxCtcssTone must be one of the following values: {string.Join(", ", CtcssTones)}");
            }
            field = value;
        }
    } = null;

    /// <summary>
    /// Converts this instance to an indented JSON string.
    /// </summary>
    /// <returns>A JSON string representing the current <see cref="RadioConfiguration"/>.</returns>
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
    /// <param name="json">A JSON string containing a serialized <see cref="RadioConfiguration"/>.</param>
    /// <returns><see langword="true"/> if deserialization succeeds and values are applied; otherwise, <see langword="false"/>.</returns>
    public bool FromJson(string json)
    {
        try
        {
            var deserialized = JsonSerializer.Deserialize<RadioConfiguration>(json);
            if (deserialized is not null)
            {
                Name = deserialized.Name;
                RxFrequency = deserialized.RxFrequency;
                TxFrequency = deserialized.TxFrequency;
                PowerLevel = deserialized.PowerLevel;
                Bandwidth = deserialized.Bandwidth;
                FmRxCtcssTone = deserialized.FmRxCtcssTone;
                FmTxCtcssTone = deserialized.FmTxCtcssTone;
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

    /// <summary>
    /// Determines whether an optional frequency is within the specified inclusive range.
    /// </summary>
    /// <param name="frequency">The frequency to evaluate, or <see langword="null"/> to treat the value as in range.</param>
    /// <param name="minFrequency">The inclusive minimum allowed frequency.</param>
    /// <param name="maxFrequency">The inclusive maximum allowed frequency.</param>
    /// <returns><see langword="true"/> if <paramref name="frequency"/> is <see langword="null"/> or between <paramref
    /// name="minFrequency"/> and <paramref name="maxFrequency"/>, inclusive; otherwise, <see langword="false"/>.</returns>
    private static bool IsFrequencyInRangeOrNull(decimal? frequency, decimal minFrequency, decimal maxFrequency)
    {
        if (frequency.HasValue)
        {
            return frequency.Value >= minFrequency && frequency.Value <= maxFrequency;
        }
        else
        {
            return true;
        }
    }
}