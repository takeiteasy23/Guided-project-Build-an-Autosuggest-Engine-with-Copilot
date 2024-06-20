namespace TrieDictionaryTest;

[TestClass]
public class TrieTest
{
    // Test that a word is inserted in the trie
    [TestMethod]
    public void InsertWord()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteWord()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Delete("hello");
        Assert.IsFalse(trie.Search("hello"));
    }

    // Test that a word is not inserted twice in the trie
    [TestMethod]
    public void InsertDuplicateWord()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hello");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is deleted from the trie
    [TestMethod]
    public void DeleteNonExistentWord()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Delete("world");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test that a word is not deleted from the trie if it is not present
    [TestMethod]
    public void DeleteWordNotPresent()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Delete("world");
        Assert.IsFalse(trie.Search("world"));
    }

    // Test that a word is deleted from the trie if it is a prefix of another word
    [TestMethod]
    public void DeleteWordPrefix()
    {
        Trie trie = new Trie();
        trie.Insert("hello");
        trie.Insert("hell");
        trie.Delete("hell");
        Assert.IsTrue(trie.Search("hello"));
    }

    // Test AutoSuggest for the prefix "cat" not present in the 
    // trie containing "catastrophe", "catatonic", and "caterpillar"
    [TestMethod]
    public void AutoSuggestPrefixNotPresent()
    {
        Trie trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("catatonic");
        trie.Insert("caterpillar");
        List<string> suggestions = trie.AutoSuggest("cat");
        Assert.AreEqual(3, suggestions.Count);
        Assert.AreEqual("catastrophe", suggestions[0]);
        Assert.AreEqual("catatonic", suggestions[1]);
        Assert.AreEqual("caterpillar", suggestions[2]);
    }

    // Test GetSpellingSuggestions for a word not present in the trie
    [TestMethod]
    public void GetSpellingSuggestionsNotPresent()
    {
        Trie trie = new Trie();
        trie.Insert("catastrophe");
        trie.Insert("cat");
        trie.Insert("caterpillar");
        List<string> suggestions = trie.GetSpellingSuggestions("caterpillar");
        Assert.AreEqual(1, suggestions.Count);
        Assert.AreEqual("caterpillar", suggestions[0]);
    }

}