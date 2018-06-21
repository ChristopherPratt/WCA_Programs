using System;
using System.Collections.Generic;
using System.Text;
using WcaProgrammerLibrary;

namespace WcaInterfaceLibrary
{
    public enum ProtocolFrameType
    {
        REQUEST = 0x00,
        POS_ACK = 0x01,
        NEG_ACK = 0x03,
        NONE = 0xFF
    };

    public enum ProtocolFramePosition
    {
        Start,
        Length,
        Address_and_Code,
        Command,
        Data,
        Checksum
    };

    public class ProtocolFrame
    {
        private static readonly byte m_start_byte = 0x55;
        private static readonly byte m_my_address = 0x03;
        private byte m_destination_address;
        private byte m_source_address;

        private byte m_protocol_code;

        private byte m_command;
        private byte[] m_data;
        private byte[] m_telegram = null;

        public static byte StartByte { get { return m_start_byte; } }

        public static Int16 MinimumTelegramLength { get { return 8; } }

        public static byte MyAddress { get { return m_my_address; } }

        public ProtocolFrame(byte source_address, byte destination_address, byte command, byte code, byte[] data)
        {
            m_source_address = source_address;
            m_destination_address = destination_address;
            m_command = command;
            m_data = data;
            m_protocol_code = code;
            Build();
        }

        public ProtocolFrame(byte source_address, byte destination_address, byte command, byte[] data)
            : this(source_address, destination_address, command, (byte)ProtocolFrameType.REQUEST, data)
        {
            
        }

        public ProtocolFrame(byte destination_address, byte command, byte[] data)
            : this(m_my_address, destination_address, command, data)
        {
            
        }

        public byte SourceAddress { get { return m_source_address; } }
        public byte DestinationAddress { get { return m_destination_address; } }
        public byte Code { get { return m_protocol_code; } }

        public byte Command { get { return m_command; } }
        public byte[] Data { get { return m_data; } }

        public byte[] Telegram { get { return m_telegram; } }

        private void Build()
        {
            Crc16Ccitt crc = new Crc16Ccitt(InitialCrcValue.Zeros);

            List<byte> tt = new List<byte>();
            tt.Add(m_start_byte);
            tt.Add(0x00); //length place holder
            tt.Add(0x00);//length place holder
            tt.Add((byte)((m_source_address << 5) | (m_destination_address << 2) | m_protocol_code));
            tt.Add(m_command);

            if (m_data != null)
            {
                foreach (byte b in m_data)
                {
                    tt.Add(b);
                }
            }
            tt.Add(0x00);//checksum place holder
            tt.Add(0x00);//checksum place holder

            m_telegram = tt.ToArray();
            byte[] ba = BitConverter.GetBytes((UInt16)(tt.Count-3)); //excludes start and length bytes
            m_telegram[1] = ba[0]; //length
            m_telegram[2] = ba[1]; //length

            byte[] crc_v = crc.ComputeChecksumBytes(m_telegram, 0, m_telegram.Length-2);

            m_telegram[m_telegram.Length - 2] = crc_v[0];
            m_telegram[m_telegram.Length - 1] = crc_v[1];
        }

        public static ProtocolFrame AnalyzeQueue(ref Queue<byte> queue)
        {
            Crc16Ccitt crc = new Crc16Ccitt(InitialCrcValue.Zeros);
            ProtocolFramePosition state = ProtocolFramePosition.Start;
            ProtocolFrame result = null;
            
            byte[] temp_tel = queue.ToArray();
            byte b;
            int start_pos = 0;
            int number_of_bytes_to_remove = 0;
            byte source_address=0, destination_address=0, code=0;
            byte command = 0;
            UInt16 length_data = 0;
            int number_of_bytes_to_read = 0;
            List<byte> data = new List<byte>();
            UInt16 crcValue = 0;
            bool first_checksum_byte = false;
           

            for (int i = 0; i < temp_tel.Length;i++)
            {
                b = temp_tel[i];

                switch (state)
                {
                    case ProtocolFramePosition.Start:
                        if (b == StartByte)
                        {
                            start_pos = i;
                            state = ProtocolFramePosition.Length;
                        }
                        break;

                    case ProtocolFramePosition.Length:

                        if ((i - start_pos)==1)
                        {
                            length_data = b;
                        }
                        else
                        {
                            length_data += (UInt16)(b << 8);

                            if (length_data >= 4 && length_data <= 200)
                            {
                                state = ProtocolFramePosition.Address_and_Code;
                                number_of_bytes_to_read = length_data - 4;
                            }
                            else
                            {
                                number_of_bytes_to_remove = i+1;
                                i = temp_tel.Length;
                            }
                        }
                        break;

                    case ProtocolFramePosition.Address_and_Code:

                        source_address = (byte)((b >> 5) & 0x07);
                        destination_address = (byte)((b >> 2) & 0x07);
                        code = (byte)(b & 0x03);

                        if (destination_address == m_my_address)
                        {
                            state = ProtocolFramePosition.Command;
                        }
                        else
                        {
                            number_of_bytes_to_remove = i+1;
                            i = temp_tel.Length;
                        }
                        break;

                    case ProtocolFramePosition.Command:
                        
                        command = b;

                        if (length_data > 4)
                        {
                            state = ProtocolFramePosition.Data;
                        }
                        else
                        {
                            state = ProtocolFramePosition.Checksum;
                        }
                        break;

                    case ProtocolFramePosition.Data:

                        data.Add(b);
                        number_of_bytes_to_read--;
                        
                        if(number_of_bytes_to_read == 0) 
                        {
                            state = ProtocolFramePosition.Checksum;
                        }
                        break;

                    case ProtocolFramePosition.Checksum:

                        if (first_checksum_byte == false)
                        {
                            first_checksum_byte = true;
                            crcValue = b;
                        }
                        else
                        {
                            crcValue += (UInt16)(b << 8);

                            byte[] crc_v = crc.ComputeChecksumBytes(temp_tel, 0, i - 1);

                            if (crcValue == (UInt16)BitConverter.ToInt16(crc_v, 0))
                            {
                                
                                //consider code info
                                result = new ProtocolFrame(source_address, destination_address, command, code, data.ToArray());
                                //Checksumme ok
                                //todo
                            }
                            else
                            { 
                                //Checksumme nicht ok
                                //todo
                            }
                            number_of_bytes_to_remove = i+1;
                        }
                        break;
                }


                while (number_of_bytes_to_remove>0)
                {
                    number_of_bytes_to_remove--;
                    queue.Dequeue();
                }
            }

            return result;
        }
    }
}
