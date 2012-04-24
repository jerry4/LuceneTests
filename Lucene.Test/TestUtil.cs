using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Search;

namespace Lucene.Test
{
  internal class TestUtil
  {
    public static int hitCount(IndexSearcher searcher, Query query)
    {
      return searcher.Search(query, 1).TotalHits;
    }
  }
}
