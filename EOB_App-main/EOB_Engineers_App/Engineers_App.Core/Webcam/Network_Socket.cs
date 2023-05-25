using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Webcam
{
    public class Data_Event_Args : EventArgs
    {
        public Data_Event_Args(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; set; }
    }

    public class Network_Socket
    {
        private const int BYTE_PACKET = 4 * 1024;
        private const int PAYLOAD_SIZE = 8;
        private const int HEADER_SIZE = 8;

        private IPEndPoint _server_Ip;

        private bool _running;
        private static byte[]? _frame_Data;
        private List<byte> _data = new List<byte>();
        private bool _has_Data = false;

        public event EventHandler<Data_Event_Args> Data_Received;
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler Server_Accepted;

        private TcpClient _client;
        private NetworkStream _stream;

        public Network_Socket(string server_Ip, int port)
        {
            var ip = IPAddress.Parse(server_Ip);
            _server_Ip = new IPEndPoint(ip, port);
        }

        public async Task Receive()
        {
            // Make sure the stream exists so we don't try to read data from null
            if (_stream == null)
                return;

            while (_running)
            {
                // Grab the first 8 bytes of the data being sent
                byte[] packedMsgSize = new byte[PAYLOAD_SIZE];

                try
                {
                    await _stream.ReadAsync(packedMsgSize);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Disconnected?.Invoke(this, EventArgs.Empty);
                }

                // Convert the 8 bytes into a 64 bit long which is the total amount of bytes being sent minus the first 8
                ulong msgSize = BitConverter.ToUInt64(packedMsgSize);

                // Loop through until all data is collected
                // Or break if it finds the closing bytes

                while (_data.Count < (int)msgSize && _running)
                {
                    int b = -1;
                    // Normal reading doesn't get all bytestry
                    try
                    {
                        b = _stream.ReadByte();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        Disconnected?.Invoke(this, EventArgs.Empty);
                    }

                    // If there's still bytes left to read add them to the list and check if they're the last byte
                    if (b != -1)
                    {
                        if (_data.Count > 0)
                        {
                            byte prevByte = _data.Last();
                            _data.Add((byte)b);

                            if (prevByte == 255 && b == 217)
                            {
                                break;
                            }
                        }
                        else
                        { 
                            _data.Add((byte)b);
                        }
                    }
                }

                // Change data list to array to load the image
                _frame_Data = _data.ToArray();
                _has_Data = true;
                // Clear the list
                _data.Clear();

                if (_frame_Data != null && _frame_Data.Length > 0)
                    Data_Received?.Invoke(this, new Data_Event_Args(_frame_Data));
                else
                    Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task Send(byte[] data)
        {
            if (_stream == null || data == null || !_has_Data || !_running) return;

            byte[] header = BitConverter.GetBytes(Convert.ToUInt64(data.Length));
            byte[] concat = new byte[header.Length + data.Length];

            header.CopyTo(concat, 0);
            data.CopyTo(concat, header.Length);

            _client.SendBufferSize = concat.Length;
            try
            {
                await _stream.WriteAsync(concat, 0, concat.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        //public struct Header
        //{
        //    uint imgLength;
        //    uint txtLength;
        //}

        //public struct Data
        //{
        //    Header header;
        //    byte[] imageData;
        //    string textData;
        //}

        public async Task<bool> Start()
        {
            try
            {
                _client = new TcpClient(_server_Ip.Address.AddressFamily);
                await _client.ConnectAsync(_server_Ip);
                _stream = _client.GetStream();
            }
            catch
            {
                return false;
            }

            _running = true;
            Connected.Invoke(this, EventArgs.Empty);

            await _stream.ReadAsync(null); // wait until server accepts the client
            Server_Accepted.Invoke(this, EventArgs.Empty);

            await Receive();
            return true;
        }

        public void Dispose()
        {
            _running = false;
            _client?.Dispose();
            _stream?.Dispose();
        }
    }
}
