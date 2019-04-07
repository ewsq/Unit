using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Unit.Data.Options
{
    public class NoDataOptions
    {
        public NoDataOptions(MongoClientSettings linkSettings)
        {
            LinkSettings = linkSettings;
        }

        public MongoClientSettings LinkSettings{ get;}
    }
}
