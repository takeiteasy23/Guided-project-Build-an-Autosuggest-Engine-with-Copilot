public class TrieNode
{
    public Dictionary<char, TrieNode> Children { get; set; }
    public bool IsEndOfWord { get; set; }

    public char _value;

    public TrieNode(char value = ' ')
    {
        Children = new Dictionary<char, TrieNode>();
        IsEndOfWord = false;
        _value = value;
    }

    public bool HasChild(char c)
    {
        return Children.ContainsKey(c);
    }
}

public class Trie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode();
    }

    public bool Insert(string word)
    {
        TrieNode current = root;
        // for each character in the word
        foreach (char c in word)
        {
            // if the character is not in the children of the current node
            if (!current.HasChild(c))
            {
                // add the character to the children of the current node
                current.Children[c] = new TrieNode(c);
            }
            // move to the next node
            current = current.Children[c];
        }
        if (current.IsEndOfWord)
        {
            // if the word is already in the trie
            return false;
        }

        // mark the end of the word
        current.IsEndOfWord = true;
        return true;
    }
    
    /// <summary>
    /// Retrieves a list of suggested words based on the given prefix.
    /// </summary>
    /// <param name="prefix">The prefix to search for.</param>
    /// <returns>A list of suggested words.</returns>
    public List<string> AutoSuggest(string prefix)
    {
        TrieNode currentNode = root;
        foreach (char c in prefix)
        {
            if (!currentNode.HasChild(c))
            {
                return new List<string>();
            }
            currentNode = currentNode.Children[c];
        }
        return GetAllWordsWithPrefix(currentNode, prefix);
    }

    private List<string> GetAllWordsWithPrefix(TrieNode root, string prefix)
    {
        List<string> words = new List<string>();
        if (root == null)
        {
            return words;
        }

        if (root.IsEndOfWord)
        {
            words.Add(prefix);
        }

        foreach (var child in root.Children)
        {
            words.AddRange(GetAllWordsWithPrefix(child.Value, prefix + child.Key));
        }

        return words;
    }

    // Helper method to delete a word from the trie by recursively removing its nodes
    private bool _delete(TrieNode current, string word, int index)
    {
        // If the index is equal to the length of the word
        if (index == word.Length)
        {
            // If the current node is not the end of a word
            if (!current.IsEndOfWord)
            {
                // The word does not exist in the trie
                return false;
            }

            // Mark the current node as not the end of a word
            current.IsEndOfWord = false;

            // If the current node has no children
            if (current.Children.Count == 0)
            {
                // Indicate that the node can be deleted
                return true;
            }

            // The node has children, do not delete
            return false;
        }

        // Get the character at the current index
        char ch = word[index];

        // If the current node does not contain the character
        if (!current.HasChild(ch))
        {
            // The word does not exist in the trie
            return false;
        }

        // Get the child node for the character
        TrieNode child = current.Children[ch];

        // Recursively call delete for the child node
        bool shouldDeleteCurrentNode = _delete(child, word, index + 1);

        // If the child node can be deleted
        if (shouldDeleteCurrentNode)
        {
            // Remove the child node from the current node's children
            current.Children.Remove(ch);

            // If the current node is not the end of a word and has no children
            if (!current.IsEndOfWord && current.Children.Count == 0)
            {
                // Indicate that the node can be deleted
                return true;
            }
        }

        // Do not delete the current node
        return false;
    }

    // Public method to delete a word from the trie
    public bool Delete(string word)
    {
        // Call the recursive delete method starting at the root node
        return _delete(root, word, 0);
    }

    public List<string> GetAllWords()
    {
        return GetAllWordsWithPrefix(root, "");
    }

    public void PrintTrieStructure()
    {
        Console.WriteLine("\nroot");
        _printTrieNodes(root);
    }

    private void _printTrieNodes(TrieNode root, string format = " ", bool isLastChild = true) 
    {
        if (root == null)
            return;

        Console.Write($"{format}");

        if (isLastChild)
        {
            Console.Write("└─");
            format += "  ";
        }
        else
        {
            Console.Write("├─");
            format += "│ ";
        }

        Console.WriteLine($"{root._value}");

        int childCount = root.Children.Count;
        int i = 0;
        var children = root.Children.OrderBy(x => x.Key);

        foreach(var child in children)
        {
            i++;
            bool isLast = i == childCount;
            _printTrieNodes(child.Value, format, isLast);
        }
    }

    public List<string> GetSpellingSuggestions(string word)
    {
        char firstLetter = word[0];
        List<string> suggestions = new();
        List<string> words = GetAllWordsWithPrefix(root.Children[firstLetter], firstLetter.ToString());
        
        foreach (string w in words)
        {
            int distance = LevenshteinDistance(word, w);
            if (distance <= 2)
            {
                suggestions.Add(w);
            }
        }

        return suggestions;
    }

    private int LevenshteinDistance(string s, string t)
    {
        int m = s.Length;
        int n = t.Length;
        int[,] d = new int[m + 1, n + 1];

        if (m == 0)
        {
            return n;
        }

        if (n == 0)
        {
            return m;
        }

        for (int i = 0; i <= m; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= n; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= n; j++)
        {
            for (int i = 1; i <= m; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
            }
        }

        return d[m, n];
    }

    // Search for a word in the trie
    public bool Search(string word)
    {
        TrieNode current = root;
        foreach (char c in word)
        {
            if (!current.HasChild(c))
            {
                return false;
            }
            current = current.Children[c];
        }
        return current.IsEndOfWord;
    }
}