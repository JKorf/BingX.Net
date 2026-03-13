using CryptoExchange.Net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingX.Net
{
    public class BingXCredentials : ApiCredentials
    {
        public BingXCredentials(string apiKey, string secret) : this(new HMACCredential(apiKey, secret)) { }
       
        public BingXCredentials(HMACCredential credential) : base(credential) { }

        public override ApiCredentials Copy() => new BingXCredentials(Hmac!);
    }
}
