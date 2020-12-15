using System;
using ItSoftware.Syndication;
using ItSoftware.Syndication.Atom;
using ItSoftware.Syndication.Rdf;
using ItSoftware.Syndication.Rss;
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
						PrintAtomItem(item);
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
						PrintRdfItem(item);
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
						PrintRssItem(item);
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

		static void PrintAtomItem(AtomEntry item)
		{
			PrintHeader();
			Console.Write(item.Content.Content);
			PrintFooter(item.Title.Text);
		}

		static void PrintRdfItem(RdfItem item)
		{
			PrintHeader();
			Console.Write(item.Description);
			PrintFooter(item.Title);
		}

		static void PrintRssItem( RssItem item )
		{
			PrintHeader();
			Console.Write(item.Description);
			PrintFooter(item.Title);
		}

		static void PrintHeader()
		{
			Console.WriteLine("== News of the Day ==");
		}
		static void PrintFooter(string text)
		{
			Console.WriteLine();
			Console.WriteLine($"    :: {text}");
		}
	}
}
