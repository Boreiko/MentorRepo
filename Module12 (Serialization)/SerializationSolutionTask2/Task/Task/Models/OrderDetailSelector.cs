using System;
using System.Runtime.Serialization;
using Task.DB;

namespace Task.Surrogate
{
    public class OrderDetailSelector : ISurrogateSelector
    {
        ISurrogateSelector _nextSelector;
        public void ChainSelector(ISurrogateSelector selector)
        {
            _nextSelector = selector;
        }

        public ISurrogateSelector GetNextSelector()
        {
            return _nextSelector;
        }

        public ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            selector = this;

            if (type == typeof(Order_Detail))          
                return new OrderDetailSerialization();
            
            return null;
        }
    }
}
