using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MidtermCSI350.Infrastructure
{
    public class Ranks
    {
        public static SelectList GetManagerRankList()
        {
            var ranks = new List<string> { "CTO", "CEO", "Rank 1", "CFO" };
            return new SelectList(ranks);
        }
    }
}
