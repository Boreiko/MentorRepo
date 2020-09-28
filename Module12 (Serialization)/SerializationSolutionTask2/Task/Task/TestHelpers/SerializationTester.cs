using System.IO;

namespace Task.TestHelpers
{
    public class SerializationTester<TData>
    {
        readonly ISerializer<TData> serializer;
        public SerializationTester(ISerializer<TData> serializer)
        {
            this.serializer = serializer;         
        }

        public TData SerializeAndDeserialize(TData data)
        {
            var memoryStream = new MemoryStream();
           
            serializer.Serialize(data, memoryStream);               

            memoryStream.Seek(0, SeekOrigin.Begin);
        
            TData result = serializer.Deserialize(memoryStream);
        
            return result;
        }
    }
}
