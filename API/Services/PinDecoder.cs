using System.Text;

public static class PinDecoder
{

  public static string DecodeBase64String(string base64EncodedData)
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
  public static string DecodePin(string pin)
  {
    var dPin = DecodeBase64String(pin);
    var decodedPin = new StringBuilder();

    if (dPin.Length != 8)
        {
            throw new ArgumentException("Pin must be 8 characters long.");
        }

    for (int i = 0; i < dPin.Length; i++)
    {
      decodedPin.Append(DecodeCharacter(dPin[i], i));
    }

    return decodedPin.ToString();
  }

  private static char DecodeCharacter(char ch, int position)
  {
    byte[] bytes = BitConverter.GetBytes(ch);
    string binaryString = BitConverter.ToString(bytes).Substring(0, 2);

    return position switch
    {
      0 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "32", '0' }, { "33", '1' }, { "30", '2' }, { "31", '3' },
                { "36", '4' }, { "37", '5' }, { "34", '6' }, { "35", '7' },
                { "3A", '8' }, { "3B", '9' }
            }, binaryString),
      1 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "11", '0' }, { "10", '1' }, { "13", '2' }, { "12", '3' },
                { "15", '4' }, { "14", '5' }, { "17", '6' }, { "16", '7' },
                { "19", '8' }, { "18", '9' }
            }, binaryString),
      2 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "05", '0' }, { "04", '1' }, { "07", '2' }, { "06", '3' },
                { "01", '4' }, { "5A", '5' }, { "03", '6' }, { "02", '7' },
                { "0D", '8' }, { "0C", '9' }
            }, binaryString),
      3 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "15", '0' }, { "14", '1' }, { "17", '2' }, { "16", '3' },
                { "11", '4' }, { "10", '5' }, { "13", '6' }, { "12", '7' },
                { "1D", '8' }, { "1C", '9' }
            }, binaryString),
      4 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "02", '0' }, { "03", '1' }, { "5A", '2' }, { "01", '3' },
                { "06", '4' }, { "07", '5' }, { "04", '6' }, { "05", '7' },
                { "0A", '8' }, { "0B", '9' }
            }, binaryString),
      5 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "32", '0' }, { "33", '1' }, { "30", '2' }, { "31", '3' },
                { "36", '4' }, { "37", '5' }, { "34", '6' }, { "35", '7' },
                { "3A", '8' }, { "3B", '9' }
            }, binaryString),
      6 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "11", '0' }, { "10", '1' }, { "13", '2' }, { "12", '3' },
                { "15", '4' }, { "14", '5' }, { "17", '6' }, { "16", '7' },
                { "19", '8' }, { "18", '9' }
            }, binaryString),
      7 => MapBinaryToDigit(new Dictionary<string, char>
            {
                { "08", '0' }, { "09", '1' }, { "0A", '2' }, { "0B", '3' },
                { "0C", '4' }, { "0D", '5' }, { "0E", '6' }, { "0F", '7' },
                { "5A", '8' }, { "01", '9' }
            }, binaryString),
      _ => throw new ArgumentException("Invalid position")
    };
  }

  private static char MapBinaryToDigit(Dictionary<string, char> mapping, string binaryString)
  {
    if (mapping.ContainsKey(binaryString))
    {
      return mapping[binaryString];
    }
    else
    {
      return ' ';
    }
  }
}