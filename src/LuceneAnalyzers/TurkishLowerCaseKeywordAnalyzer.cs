using Lucene.Net.Analysis;
using System.IO;

namespace LuceneAnalyzers
{
    public class TurkishLowerCaseKeywordAnalyzer : Analyzer
    {
        public override TokenStream ReusableTokenStream(string fieldName, TextReader reader)
        {
            var previousTokenStream = (TutkishLowerCaseKeywordTokenizer)PreviousTokenStream;
            if (previousTokenStream == null)
            {
                return TokenStream(fieldName, reader);
            }

            previousTokenStream.Reset(reader);
            return previousTokenStream;
        }

        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            return new TutkishLowerCaseKeywordTokenizer(reader);
        }
    }
}
