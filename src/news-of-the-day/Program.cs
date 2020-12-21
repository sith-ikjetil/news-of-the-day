using System;
using ItSoftware.Syndication;
using ItSoftware.Syndication.Atom;
using ItSoftware.Syndication.Rdf;
using ItSoftware.Syndication.Rss;
using ItSoftware.Core.Extensions;
namespace news_of_the_day
{
	class Program
	{
		static void Main(string[] args)
		{
			var url = "http://feeds.bbci.co.uk/news/world/rss.xml";
			
			if ( args.Length <= 1 )
			{
				if (args.Length == 1)
				{
					url = args[0];
				}
				LoadAndPrintRssRandomNewsItem(url);
				return;
			}

			Console.WriteLine("news-of-the-day error: too many arguments.");
		}

		static void LoadAndPrintRssRandomNewsItem(string url)
		{
			try
			{
				var sb = Syndication.Load(new Uri(url));
				if ( sb is Atom )
				{
					var atom = sb as Atom;

					var numItems = atom.Entries.Count;
					if ( numItems > 0 )
					{
						var rnd = new Random();
						var item = atom.Entries[rnd.Next(0, numItems - 1)];
						PrintAtomItem(atom, item);
					}
					return;
				}
				else if ( sb is Rdf)
				{
					var rdf = sb as Rdf;

					var numItems = rdf.Items.Count;
					if ( numItems > 0 )
					{
						var rnd = new Random();
						var item = rdf.Items[rnd.Next(0, numItems - 1)];
						PrintRdfItem(rdf, item);
					}
					return;
				}
				else if ( sb is Rss )
				{
					var rss = sb as Rss;

					var numItems = rss.Channel.Items.Count;
					if ( numItems > 0 )
					{
						var rnd = new Random();
						var item = rss.Channel.Items[rnd.Next(0, numItems - 1)];
						PrintRssItem(rss, item);
					}
					return;
				}

				Console.WriteLine("news-of-the-day invalid rss");
			}
			catch ( Exception x )
			{
				Console.WriteLine($"news-of-the-day error: {x.Message}");
			}
		}

		static void PrintAtomItem(Atom atom, AtomEntry item)
		{
			PrintHeader();
			Console.Write(NormalizeContent(item.Content.Content));
			PrintFooter(item.Title.Text, atom.Title.Text);
		}

		static void PrintRdfItem(Rdf rdf, RdfItem item)
		{
			PrintHeader();
			Console.Write(NormalizeContent(item.Description));
			PrintFooter(item.Title, rdf.Channel.Title);
		}

		static void PrintRssItem( Rss rss, RssItem item )
		{
			PrintHeader();
			Console.Write(NormalizeContent(item.Description));
			PrintFooter(item.Title, rss.Channel.Title);
		}

		static void PrintHeader()
		{
			Console.WriteLine("== News of the Day ==");
		}

		static void PrintFooter(string footer1, string footer2)
		{
			Console.WriteLine();
			Console.WriteLine($"    :: {footer1}");
			Console.WriteLine($"    :: {footer2}");
		}

		static string NormalizeContent(string content)
		{
			if ( string.IsNullOrEmpty(content)
				|| string.IsNullOrWhiteSpace(content))
			{
				return string.Empty;
			}

			var strip = content.ItsRegExPatternMatchesAsArray(@"<[^>]*>");
			foreach ( var s in strip )
			{
				content = content.ItsRemove(s);
			}

			return content;
		}
	}
}
