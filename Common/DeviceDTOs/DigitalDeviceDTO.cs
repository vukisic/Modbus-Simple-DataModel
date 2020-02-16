namespace Common.DeviceDTOs
{
    public class DigitalDeviceDTO : DeviceDTO
    {
        
        public byte Value { get; set; }

        public DigitalDeviceDTO(string typeOfRegister, int address, byte value, string timeStamp): base(typeOfRegister,address,timeStamp)
        {
            Value = value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

    }
}
