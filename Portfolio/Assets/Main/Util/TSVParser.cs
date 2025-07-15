using System.Collections.Generic;
using System.IO;

public class TSVParser {
	public static List<string[]> Parse(string tsv) {
		var result = new List<string[]>();

		using var reader = new StringReader(tsv);
		string line;
		while ((line = reader.ReadLine()) != null) {
			result.Add(ParseLine(line));
		}

		return result;
	}

	private static string[] ParseLine(string line) {
		var fields = new List<string>();
		var start = 0;
		for (var i = 0; i < line.Length; i++) {
			if (line[i] != '\t') {
				continue;
			}

			fields.Add(line.Substring(start, i - start));
			start = i + 1;
		}

		if (start <= line.Length) {
			fields.Add(line[start..]);
		}

		return fields.ToArray();
	}
}