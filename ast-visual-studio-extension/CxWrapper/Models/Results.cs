﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace ast_visual_studio_extension.CxCLI.Models
{
    internal class Results
    {
        public int totalCount;
        public List<Result> results;

        [JsonConstructor]
        public Results(int totalCount, List<Result> results)
        {
            this.totalCount = totalCount;
            this.results = results;
        }

    }
}
