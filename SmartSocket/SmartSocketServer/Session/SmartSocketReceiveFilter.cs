using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.Common;
using SuperSocket.Facility.Protocol;
using SuperSocket.SocketBase.Protocol;

namespace SmartSocketServer.Session
{
    class SmartSocketReceiveFilter : FixedHeaderReceiveFilter<BinaryRequestInfo> 
    {
        public SmartSocketReceiveFilter()
            : base(6)
        {

        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 4] * 256 + (int)header[offset + 5];
        }

        protected override BinaryRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            return new BinaryRequestInfo(Encoding.UTF8.GetString(header.Array, header.Offset, 4)
                , bodyBuffer.CloneRange(offset, length));
        }
    }
}
