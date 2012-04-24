using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lucene.Test
{
  [TestClass]
  public class SimpleIndexReadWriteTest
  {
    protected String[] ids = { "1", "2" };
    protected String[] unindexed = { "Netherlands", "Italy" };
    protected String[] unstored = {"Amsterdam has lots of bridges",
                                   "Venice has lots of canals"};
    protected String[] text = { "Amsterdam", "Venice" };
    private Directory directory;

    [TestInitialize]
    public void TestInitialize()
    {
      directory = new RAMDirectory();
      IndexWriter writer = getWriter(); //2
      for (int i = 0; i < ids.Length; i++)
      { //3
        Document doc = new Document();
        doc.Add(new Field("id", ids[i],
        Field.Store.YES,
        Field.Index.NOT_ANALYZED));
        doc.Add(new Field("country", unindexed[i],
        Field.Store.YES,
        Field.Index.NO));
        doc.Add(new Field("contents", unstored[i],
        Field.Store.NO,
        Field.Index.ANALYZED));
        doc.Add(new Field("city", text[i],
        Field.Store.YES,
        Field.Index.ANALYZED));
        writer.AddDocument(doc);
      }
      writer.Close();
    }

    private IndexWriter getWriter()
    {
      return new IndexWriter(directory, new WhitespaceAnalyzer(), // 2
      IndexWriter.MaxFieldLength.UNLIMITED); // 2
    }

    protected int getHitCount(String fieldName, String searchString)
    {
      IndexSearcher searcher = new IndexSearcher(directory, true); //4
      Term t = new Term(fieldName, searchString);
      Query query = new TermQuery(t); //5
      int hitCount = TestUtil.hitCount(searcher, query); //6
      searcher.Close();
      return hitCount;
    }

    [TestMethod]
    public void TestIndexWriter()
    {
      IndexWriter writer = getWriter();
      Assert.AreEqual(ids.Length, writer.NumDocs()); //7
      writer.Close();
    }

    [TestMethod]
    public void TestIndexReader()
    {
      IndexReader reader = IndexReader.Open(directory);
      Assert.AreEqual(ids.Length, reader.MaxDoc()); //8
      Assert.AreEqual(ids.Length, reader.NumDocs()); //8
      reader.Close();
    }
  }
}
