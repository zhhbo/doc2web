#r ".\Doc2web\bin\Release\netstandard2.0\Doc2web.dll"

using Doc2web;


string sentence = "“Articles” means the articles of incorporation of the Corporation, as amended, replaced, restated or supplemented from time to time;";


var spans = new string[]
{
    "“",
    "Articles",
    "” means the articles of incorporation of the ",
    "Corporation",
    ", as amended, replaced, restated or supplemented from time to time;"
};

var anchors = new string[] {
    "Articles",
    "Corporation"
};

var highlights = new string[]
{
    "Articles"
};

//class MockPlugin
//{
//    public void ProcessP(IElementContext ctx, Paragraph p)
//    {

//    }
//}
