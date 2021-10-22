using System;

namespace PokemonExtraLifeApiCore.ExtraLife.Responses
{
    public class StreamsResponse
    {
        public Datum[] data { get; set; }

        public class Datum
        {
            public string id { get; set; }
            public string user_id { get; set; }
            public string user_name { get; set; }
            public string game_id { get; set; }
            public string type { get; set; }
            public string title { get; set; }
            public int viewer_count { get; set; }
            public DateTime started_at { get; set; }
            public string language { get; set; }
            public string thumbnail_url { get; set; }
            public string[] tag_ids { get; set; }
        }

    }
}