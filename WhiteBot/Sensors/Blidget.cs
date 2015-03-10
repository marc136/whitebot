using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;

namespace WhiteBot
{
    public class ConnectionException : Exception
    {

    }

    public class Blidget
    {
        private static Guid uuidBGS             = Guid.Parse("31300000-5347-4233-3074-656764696C42");
        
        private static Guid uuidBGSCommand      = Guid.Parse("31300102-5347-4233-3074-656764696C42");
        private static Guid uuidBGSResponse     = Guid.Parse("31300103-5347-4233-3074-656764696C42");
        
        private static Guid uuidBGSDigitalIn    = Guid.Parse("31300301-5347-4233-3074-656764696C42");
        private static Guid uuidBGSAnalogIn     = Guid.Parse("31300302-5347-4233-3074-656764696C42");

        private DeviceInformation deviceInformation;
        private GattDeviceService deviceService;
        public PinConfig pinConfig;
        public DigitalOut digitalOut;

        public Blidget(DeviceInformation deviceInformation)
        {
            this.deviceInformation = deviceInformation;

            deviceService = GattDeviceService.FromIdAsync(deviceInformation.Id).AsTask().Result;
            pinConfig = new PinConfig(deviceService);
            digitalOut = new DigitalOut(deviceService);
        }

        public class PinConfig
        {
            GattCharacteristic charactersistic;

            private static Guid uuidBGSPinConfig = Guid.Parse("31300101-5347-4233-3074-656764696C42");
            public PinConfig(GattDeviceService service)
            {
                var charactersistics = service.GetCharacteristics(uuidBGSPinConfig);

                charactersistic = charactersistics[0];
            }

            byte[] pinModes = new byte[18];

            public void setDigitalOut(int pin)
            {
                pinModes[pin] = 0x15;
                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(pinModes);
                var res = charactersistic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }

            public void setPwm(int pin, int timer, int channel)
            {
                pinModes[pin] = (byte)(32 | ((timer & 1) << 2) | (channel & 3));
                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(pinModes);
                var res = charactersistic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }

            public void refresh()
            {
                var res = charactersistic.ReadValueAsync().AsTask().Result;
                
                Windows.Storage.Streams.DataReader.FromBuffer(res.Value).ReadBytes(pinModes);
            }
        }

        public Serial createSerial(int txPin, int rxPin)
        {
            var serial = new Serial(deviceService, txPin, rxPin);
            pinConfig.refresh();
            return serial;
        }

        public Pwm createPwm(int timer, params int[] pins)
        {
            for (int i = 0; i < pins.Length; ++i )
            {
                pinConfig.setPwm(pins[i], timer, i);
                Thread.Sleep(200);
            }
            
            return new Pwm(deviceService, timer);
        }

        public class Pwm
        {
            GattCharacteristic characteristic;

            private static Guid uuidBGSCommand = Guid.Parse("31300102-5347-4233-3074-656764696C42");

            public int Timer { get; private set; }
            public int Prescaler { get; private set; }
            public int MaxValue { get; private set; }

            public Pwm(GattDeviceService service, int timer)
            {
                var charactersistics = service.GetCharacteristics(uuidBGSCommand);
                characteristic = charactersistics[0];

                this.Timer = timer;
                
                reset(0, 65535);
            }

            public void reset(int prescaler, int maxValue)
            {
                Prescaler = prescaler;
                MaxValue = maxValue;

                reset();
            }
            /*
            public int setFrequency(int frequency)
            {
                int remainingFrequency = (16000000 / 65535);

            }*/

            private void reset()
            {
                byte[] data = new byte[] { 0x21, (byte)Timer, (byte)Prescaler, (byte)(MaxValue), (byte)(MaxValue >> 8), 0, 0, 0, 0, 0, 0 };

                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(data);
                var res = characteristic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }

            public void setValue(int channel, int value)
            {
                if(value > MaxValue)
                {
                    throw new ArgumentOutOfRangeException("PWM Value must be less than or equal to timer maximum value: " + MaxValue);
                }

                byte[] data = new byte[] { 0x22, (byte)Timer, (byte)channel, (byte)(value & 255), (byte)(value >> 8) };

                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(data);
                var res = characteristic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }
        }

        public class Serial
        {
            GattCharacteristic characteristic;

            private static Guid uuidBGSCommand = Guid.Parse("31300102-5347-4233-3074-656764696C42");
            int txPin;
            int rxPin;

            public Serial(GattDeviceService service, int txPin, int rxPin)
            {
                var charactersistics = service.GetCharacteristics(uuidBGSCommand);
                characteristic = charactersistics[0];

                this.txPin = txPin;
                
                this.rxPin = rxPin;

                byte[] data = new byte[] { 0x80, (byte)rxPin, (byte)txPin, 255, 255};

                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(data);
                var res = characteristic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }
            public void sendData(byte[] data)
            {
                if (data.Length > 17)
                {
                    throw new NotImplementedException();
                }
                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteByte(0x82);
                buffer.WriteByte((byte)(data.Length & 0xFF));
                buffer.WriteByte((byte)((data.Length >> 8) & 0xFF));
                buffer.WriteBytes(data);
                var res = characteristic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }

            public void receiveData(byte[] data)
            {
                if (data.Length > 17)
                {
                    throw new NotImplementedException();
                }
                throw new NotImplementedException();
            }
        }

        public class DigitalOut
        {
            GattCharacteristic charactersistic;

            private static Guid uuidBGSDigitalOut = Guid.Parse("31300201-5347-4233-3074-656764696C42");

            public DigitalOut(GattDeviceService service)
            {
                var charactersistics = service.GetCharacteristics(uuidBGSDigitalOut);

                charactersistic = charactersistics[0];
            }

            byte[] pins = new byte[3];

            public void setPin(int pin, bool value)
            {
                if(value)
                {
                    pins[pin / 8] |= (byte)(1 << (pin % 8));
                }
                else
                {
                    pins[pin / 8] &= (byte)~(1 << (pin % 8));
                }
                var buffer = new Windows.Storage.Streams.DataWriter();
                buffer.WriteBytes(pins);
                var res = charactersistic.WriteValueAsync(buffer.DetachBuffer(), GattWriteOption.WriteWithoutResponse).AsTask().Result;
            }

        }

        public static Blidget createFromIndex(int i)
        {
            var blidgets = DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(uuidBGS)).AsTask().Result;
            if(i >= blidgets.Count)
            {
                return null;
            }
            else
            {
                return new Blidget(blidgets[i]);
            }
        }

        public static Blidget createFromName(string name)
        {
            var blidgets = DeviceInformation.FindAllAsync(GattDeviceService.GetDeviceSelectorFromUuid(uuidBGS)).AsTask().Result;
            var robot = (from b in blidgets
                          where b.Name == name
                          select new Blidget(b)).FirstOrDefault();
            if(robot == null)
            {
                throw new ConnectionException();
            }
            return robot;
        }
    }
}
