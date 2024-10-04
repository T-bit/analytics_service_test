using System.Text;

namespace TestEventService.Extensions
{
    public static class ByteExtensions
    {
        public static string ToUTF8String(this byte[] self)
        {
            return Encoding.UTF8.GetString(self);
        }
    }
}