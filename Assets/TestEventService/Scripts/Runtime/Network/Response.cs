using TestEventService.Extensions;

namespace TestEventService.Network
{
    public readonly struct Response
    {
        private readonly byte[] _bytes;

        public readonly long ResponseCode;

        public Response(long responseCode, byte[] bytes)
        {
            ResponseCode = responseCode;
            _bytes = bytes;
        }

        public override string ToString()
        {
            return _bytes == null ? string.Empty : _bytes.ToUTF8String();
        }
    }
}