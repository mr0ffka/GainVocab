using HtmlAgilityPack;
using Scraper.Models;
using Scraper;

HtmlWeb web = new HtmlWeb();
HtmlDocument doc = new HtmlDocument();
Random random = new Random();
string userInput = "";
int wordCount = 0;
int exampleCount = 0;
bool hasData = false;
var words = new List<Word>();
var examples = new List<ExampleSentence>();
var type = "";

Menu();

void Menu()
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine(" --- bab.la scraper --- \n");
        Console.WriteLine(" 1. Get data for POL-ENG");
        if (hasData)
        {
            Console.WriteLine(" 3. Export data to .csv");
        }       
        Console.WriteLine(" q. quit");


        Console.Write("\n Select: ");
        userInput = Console.ReadLine();

        switch(userInput)
        {
            case "1":
                doc = web.Load("https://pl.bab.la/slownik/polski-angielski/");
                type = "POL-ENG";
                GetData(3);
                break;
            case "q":
                Environment.Exit(0);
                break;
            default:
                break;
        }
    }
}

void GetData(int numbersOfLettersInOneFile)
{
    var lettersUlElement = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'nav-pills')]//a[@href]");
    var lettersLinks = new List<string>();
    var wordsLinks = new Dictionary<string, string>();
    var letterIndexStart = 0;

    foreach (var lettersLiElement in lettersUlElement)
    {
        var value = lettersLiElement.Attributes["href"].Value;
        lettersLinks.Add(value);
    }

    var div = lettersLinks.Count / numbersOfLettersInOneFile;

    for (int num = 0; num < lettersLinks.Count; num++)
    {
        words = new List<Word>();
        examples = new List<ExampleSentence>();
        letterIndexStart = num * numbersOfLettersInOneFile;
        HtmlDocument docWordList = null;
        HtmlDocument docWordPage = null;
        for (var letterIndex = letterIndexStart; letterIndex < letterIndexStart + numbersOfLettersInOneFile; letterIndex++)
        {
            if (letterIndex >= lettersLinks.Count()) 
                continue;

            var letter = lettersLinks[letterIndex];
            var startPageIndex = 1;
            var endPageIndex = 0;
            var letterLink = letter.Substring(0, letter.LastIndexOf("/") + 1);
            docWordList = web.Load(letterLink + startPageIndex);

            var lastPageATagTemp = docWordList.DocumentNode.SelectNodes("//a[@class=\"dict-pag-button\"]");
            if (lastPageATagTemp == null)
                continue;

            var lastPageATag = lastPageATagTemp.Where(a => a.InnerText == ">>").FirstOrDefault();
            var linkString = lastPageATag.Attributes["href"].Value;
            var indexOfLastElement = linkString.LastIndexOf("/") + 1;
            endPageIndex = Int32.Parse(linkString.Substring(indexOfLastElement));

            for (var pageIndex = startPageIndex; pageIndex < endPageIndex; pageIndex+=(endPageIndex / 3))
            {
                docWordList = web.Load(letterLink + pageIndex);
                var wordWrapperColumns = docWordList.DocumentNode.SelectNodes("//*[@id=\"letterWordList\"]/div/div[2]/div[@class=\"dict-select-column\"]/ul");

                if (wordWrapperColumns == null)
                    continue;

                foreach (var column in wordWrapperColumns)
                {
                    var childNodesWithOutEmpty = column.ChildNodes.Where(c => c.Name == "li").ToList();
                    var from = random.Next(1, childNodesWithOutEmpty.Count);
                    var to = random.Next(from, childNodesWithOutEmpty.Count);
                    for (int i = from; i < to; i += 2)
                    {
                        var wordListElement = childNodesWithOutEmpty[i];
                        var aTag = wordListElement.ChildNodes.Where(a => a.OriginalName == "a").FirstOrDefault();
                        var link = "https:" + aTag.Attributes["href"].Value;
                        docWordPage = web.Load(link);

                        var wordPair = docWordPage.DocumentNode.SelectNodes("//div[@class=\"wordPair\"]/span");
                        var wordPairList = wordPair != null
                            ? wordPair.Where(s =>
                                (s.Attributes.Contains("class") && !(s.Attributes["class"].Value.Contains("flag") || s.Attributes["class"].Value.Contains("sound-inline") || s.Attributes["class"].Value.Contains("material-icons"))) ||
                                (s.Attributes.Count == 0 && s.InnerHtml != " = ")
                            ).ToList()
                            : new List<HtmlNode>();

                        if (wordPairList.Count > 0)
                        {
                            var sourceWord = wordPairList[0].InnerHtml;
                            var translationWord = wordPairList[1].InnerHtml;
                            var word = new Word(wordCount, sourceWord, translationWord);
                            words.Add(word);

                            var isPracticalExamples = docWordPage.DocumentNode.SelectNodes("//*[@id=\"practicalexamples\"]");
                            if (isPracticalExamples != null)
                            {
                                var practicalExamples = docWordPage.DocumentNode.SelectNodes("//*[@id=\"practicalexamples\"]/div/div[@class=\"sense-group\"]/div/div");

                                var practicalExamplePair = practicalExamples[0].ChildNodes.Where(e => e.Name == "div").ToList();
                                var sourceExample = practicalExamplePair[0].InnerHtml.Trim().Replace("<strong>", "").Replace("</strong>", "");
                                var translationExample = practicalExamplePair[1].ChildNodes[2].InnerHtml;

                                var practicalExample = new ExampleSentence(exampleCount, word.Id, sourceExample, translationExample);

                                examples.Add(practicalExample);

                                exampleCount++;
                            }

                            wordCount++;
                        }

                        Console.Clear();
                        Console.WriteLine($"Progress:\n -> letter page: {letterLink + pageIndex}\n -> endPageIndex: {endPageIndex}\n -> wordCount: {wordCount}\n -> exampleCount: {exampleCount}");
                    }
                }
            }
        }

        var today = DateTime.Today;
        ExportData.ExportCsv(words, $"scrapper_words_{type}_{today.Year}_{today.Month}_{today.Day}_letters_{letterIndexStart}_{letterIndexStart + numbersOfLettersInOneFile}");
        ExportData.ExportCsv(examples, $"scrapper_examples_{type}_{today.Year}_{today.Month}_{today.Day}_letters_{letterIndexStart}_{letterIndexStart + numbersOfLettersInOneFile}");
    }
} 